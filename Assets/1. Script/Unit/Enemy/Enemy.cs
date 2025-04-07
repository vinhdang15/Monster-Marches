using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : UnitBase, IEnemy
{
    [SerializeField] PathFinder pathFinder;
    public Transform fontPoint;
    public Soldier targetSoldier;
    public event Action<Enemy> OnEnemyDeath;
    public event Action<Enemy> OnEnemyReachEndPoint;
    private bool isProcessDead = false;

    protected override void Awake()
    {
        base.Awake();
        pathFinder = GetComponent<PathFinder>();
    }

    // bullet instantitate in pool and bullet event register every time tower GetBullet,
    // so needed to OnEnemyDeath = null; every time it OnDisable()
    private void OnDisable()
    {
        OnEnemyDeath = null;
        OnEnemyReachEndPoint = null;
    }

    // Get Pathway, prepare game
    public void PrepareGame(List<PathWaySegment> pathWaySegmentList, int pathWaySegmentIndex)
    {
        pathFinder.PrepareGame(pathWaySegmentList,pathWaySegmentIndex);
    }

    public void EnemyAction()
    {
        if(CurrentHp == 0) return;

        if (IsTargetSoldierComming())
        {
            unitAnimation.UnitPlayIdle();
        }
        else if (IsTargetSoldierAttacking())
        {
            AttackSoldier();
        }
        else
        {
            KeepMoving();
        }
    }

    private bool IsTargetSoldierComming()
    {
        return targetSoldier != null && targetSoldier.currentState == Soldier.SoldierState.MovingToEnemy;
    }

    private bool IsTargetSoldierAttacking()
    {
        return targetSoldier != null && targetSoldier.currentState == Soldier.SoldierState.AttackingEnemy;
    }

    private void KeepMoving()
    {
        unitAnimation.UnitPlayWalk();
        pathFinder.FollowPath(CurrentSpeed);
    }

    private void AttackSoldier()
    {
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

    public void SetMovingDirection()
    {
        if(CurrentPos == null || CurrentHp == 0) return;
        float x = transform.position.x - CurrentPos.x;
        if(x < 0) transform.localScale = new Vector2(-1,1);
        else if(x < 0) transform.localScale = new Vector2(1,1);
        CurrentPos = transform.position;
    }

    // take damage and Die
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        ProcessIfEnemyDead();
    }

    // using isProcessDead to prevent OnEnemyDeath call multi time
    private void ProcessIfEnemyDead()
    {
        if(CurrentHp > 0) return;
        if(isProcessDead) return;
        isProcessDead = true;
        AudioManager.Instance.PlaySound(soundEffectSO.GetRandomMonsterDie());
        base.HideHealthBar();
        base.isdead = true;
        OnEnemyDeath?.Invoke(this);
    }

    public override void DealDamage()
    {
        if(targetSoldier != null && targetSoldier.CurrentHp > 0)
        {
            AudioManager.Instance.PlaySoundTurnPitch(audioSource, soundEffectSO.GetRandomSwordSound());
            targetSoldier.TakeDamage(Damage);
        }
    }

    public IEnumerator ReturnPoolAfterPlayAnimation()
    {
        yield return null;
        yield return new WaitForSeconds(unitAnimation.GetCurrentAnimationLength());
        UnitPool.Instance.ReturnEnemy(this);
        yield break;
    }

    // use ResetEnemyState when soldier untarget this enemy
    public void ResetEnemyTargetState()
    {
        targetSoldier = null;
    }

    // use ResetUnit when return to pool
    public override void ResetUnit()
    {
        isProcessDead = false;
        ResetEnemyTargetState();
        base.ResetUnit();
    }

    public void OnReachEndPoint()
    {
        base.ResetUnit();
        OnEnemyReachEndPoint?.Invoke(this);
    }
}
