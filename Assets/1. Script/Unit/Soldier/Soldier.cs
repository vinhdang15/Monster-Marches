using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Soldier : UnitBase
{
    // soldier infor
    public BarrackTowerView     barrackTowerView;
    public int                  index;
    public Vector2              guardPos;
    private Vector2             barrackGatePos;
    private bool                isReachGuardPos = false;
    public List<Enemy>          enemiesInRange = new List<Enemy>();
    private float               revivalTime;
    public bool                 isBarrackUpgrade = false;

    // target infor
    private bool                hasTarget;
    public Enemy                targetEnemy = null;
    private Vector2             targetEnemyFontPos;
    public bool                 isReachTargetEnemyFrontPos = false;

    public SoldierMovementHandler      Movement { get; private set; }
    public SoldierReturnToPoolHandler  ReturnToPool { get; private set; }
    public SoldierRevivalHandler       RevivalHandler { get; private set; }

    // soldier state
    public event Action<Soldier>    OnSoldierDeath;
    public SoldierState             currentState;
    private Vector2                 offsetPos = new Vector2(0, 0);

    public enum SoldierState
    {
        MovingToGuardPos,
        MovingToEnemy,
        AttackingEnemy,
        Die,
    }

    private void InitComponents()
    {
        Movement = gameObject.AddComponent<SoldierMovementHandler>();
        Movement.Init(this);

        ReturnToPool = gameObject.AddComponent<SoldierReturnToPoolHandler>();
        ReturnToPool.Init(this);

        RevivalHandler = gameObject.AddComponent<SoldierRevivalHandler>();
        RevivalHandler.Init(this);
    }

    protected override void Awake()
    {
        base.Awake();
        InitComponents();
    }

    public void SoldierInitInfor(BarrackTowerView barrackTowerView, int index, Vector2 barrackGatePos, float revivalTime)
    {
        this.barrackTowerView = barrackTowerView;
        this.index = index;
        this.revivalTime = revivalTime;
        this.barrackGatePos = barrackGatePos;
        GetOffsetPos();
    }

    public void SoldierAction()
    {
        switch (currentState)
        {
            case SoldierState.MovingToGuardPos:
                if(CurrentHp == 0) return;
                MovingToGuardPosStateToMovingToEnemy();
                UnTargetEnemy();
                SetSoldierDirect(guardPos);
                MoveToStandingPos();
                break;
            
            case SoldierState.MovingToEnemy:
                if(CurrentHp == 0) return;
                SetTargetEnemy();
                if(targetEnemy != null) SetSoldierDirect(targetEnemy.transform.position);
                MoveToTargetEnemy(); 
                MovingToEnemyToAttackState();
                break;

            case SoldierState.AttackingEnemy:
                AttackingTargetEnemy();
                AttackToMovingToEnemyState();
                break;

            case SoldierState.Die:
                break;
        }

        base.ApplySkill();
    }

    private void GetOffsetPos()
    {
        switch (index)
        {
            case 0: offsetPos = new Vector2(-0.075f, -0.075f);
            break;
            case 1: offsetPos = new Vector2(0, 0);
            break;
            case 2: offsetPos = new Vector2(-0.075f, 0.075f);
            break;
        }
    }

    #region MOVING TO STANDING POSITION
    private void UnTargetEnemy()
    {
        if(targetEnemy == null) return;
        targetEnemy.targetSoldier = null;
        targetEnemy = null;
        hasTarget = false;
    }

    private void SetSoldierDirect(Vector2 targetPos)
    {
        if(transform.position.x < targetPos.x)
        {
            transform.localScale = new Vector2(1,1);
        }
        else if(transform.position.x > targetPos.x)
        {
            transform.localScale = new Vector2(-1,1);
        }
    }

    private void MoveToStandingPos()
    {
        if((Vector2)transform.position != guardPos)
        {
            // wait for GuardPoint reset isReachStandingPointPos
            // (time to reset isReachStandingPointPos of each soldier is different to make it good looking)
            if(isReachGuardPos == true) return;
            Movement.MoveTo(guardPos);
        }
        else
        {
            isReachGuardPos = true;
            unitAnimation.UnitPlayIdle();
        }
    }

    private void MovingToGuardPosStateToMovingToEnemy()
    {
        if(isReachGuardPos == false && enemiesInRange.Count == 0) return;
        if(CanSoldierMovingToEnemy())
        {
            currentState = SoldierState.MovingToEnemy;
        }
    }

    private bool CanSoldierMovingToEnemy()
    {
        if(enemiesInRange.Count == 0) return false;

        // can not use isReachGuardPos to check because it just change value only a start coroutine with delay time at GuardPoint 
        return (Vector2)transform.position == guardPos;
    }
    #endregion

    #region MOVING TO TARGET ENEMY
    public void SetTargetEnemy()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (hasTarget == false && enemiesInRange[i].targetSoldier == null)
            {
                enemiesInRange[i].targetSoldier = this;
                targetEnemy = enemiesInRange[i];
                hasTarget = true;
                break;
            } 
        }

        // If there are no unoccupied enemies, this soldier will assist in attacking other soldier's occupied enemy
        if(!TargetEnemyActive())
        {
            hasTarget = false;
            isReachTargetEnemyFrontPos = false; 
            if(enemiesInRange.Count > 0) targetEnemy = enemiesInRange[0];
        }
    }

    private void MoveToTargetEnemy()
    {
        if(!TargetEnemyActive())
        {
            return;
        }

        targetEnemyFontPos = (Vector2)targetEnemy.fontPoint.position + offsetPos;

        if (Vector2.SqrMagnitude((Vector2)transform.position - targetEnemyFontPos) > 0.005f)
        {
            isReachTargetEnemyFrontPos = false;
            Movement.MoveTo(targetEnemyFontPos);
        }
        else
        {
            isReachTargetEnemyFrontPos = true;
            unitAnimation.UnitPlayIdle();
        }
    }

    private void MovingToEnemyToAttackState()
    {
        if(TargetEnemyActive() && isReachTargetEnemyFrontPos == true)
        {
            currentState = SoldierState.AttackingEnemy;
        }
    }
    #endregion

    #region ACTTACKING ENEMY
    private void AttackToMovingToEnemyState()
    {
        if(hasTarget == true) return;
        if(enemiesInRange.Count == 0) return;
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i].targetSoldier == null)
            {
                currentState = SoldierState.MovingToEnemy;
            } 
        }
    }

    private void AttackingTargetEnemy()
    {
        if(!TargetEnemyActive())
        {
            ResetSoldierState();
            currentState = SoldierState.MovingToEnemy;
            return;
        }

        if(attackCooldown <= 0)
        {
            unitAnimation.UnitPlayAttack();
            attackCooldown = ResetTimeDelay(AttackSpeed);
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public override void DealDamage()
    {
        if(TargetEnemyActive())
        {
            AudioManager.Instance.PlaySoundTurnPitch(audioSource, soundEffectSO.GetRandomSwordSound());
            targetEnemy.TakeDamage(Damage);
        }
    }
    #endregion

    #region TAKE DAMAGE AND RETURN TO BARRACK WHEN DIE
    // take damage and Die
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(CurrentHp == 0)
        {
            ProcessSoldierDead();
        }
    }

    private void ProcessSoldierDead()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.GetRandomSoldierDie());
        base.HideHealthBar();
        base.isDead = true;
        OnSoldierDeath?.Invoke(this);
    }

    public IEnumerator RevivalCoroutine()
    {
        yield return RevivalHandler.RevivalCoroutine(revivalTime);
    }
    #endregion

    #region RETURN TO POOL WHEN BARACK DESTROY
    public void ReturnToUnitPool()
    {
        ResetSoldierState();
        UnitPool.Instance.ReturnToUnitPool(this);
    }

    public override void ResetUnit()
    {
        base.ResetUnit();
        ResetSoldierState();
        currentState = SoldierState.MovingToGuardPos;
    }

    public void ResetSoldierState()
    {
        isReachGuardPos = false;
        isReachTargetEnemyFrontPos = false;
        hasTarget = false;
        if (targetEnemy != null)
        {
            targetEnemy.ResetEnemyTargetState();
            targetEnemy = null;
        }
    }
    #endregion

    public void GuardPointChange()
    {
        ResetSoldierState();
        currentState = SoldierState.MovingToGuardPos;
    }

    private bool TargetEnemyActive()
    {
        return targetEnemy != null && targetEnemy.CurrentHp > 0;
    }

    public void ReturnToBarrackGatePos()
    {
        transform.position = barrackGatePos;
    }

    public bool IsInGuardPos()
    {
        return (Vector2)transform.position == guardPos;
    }

    public IEnumerator FadeOut(float time, Action onComplete)
    {     
        yield return ReturnToPool.FadeOut(time, onComplete);
    }
}