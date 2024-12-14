using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Soldier : UnitBase
{
    public int index;
    public List<Enemy> enemiesInRange = new List<Enemy>();
    public bool hadTarget;
    public Enemy targetEnemy = null;
    public Vector2 guardPos;
    public Vector2 targetEnemyFontPos;
    public bool isReachGuardPos = false;
    public bool isReachTargetEnemyFontPos = false;
    private Vector2 offsetPos = new Vector2(0, 0);
    public event Action<Soldier> OnSoldierDeath;

    private void OnDisable()
    {
        OnSoldierDeath = null;
    }

    public enum SoldierState
    {
        MovingToGuardPos,
        MovingToEnemy,
        ActtackingEnemy,
        Die,
    }

    public SoldierState currentState;
    public void SoldierAction()
    {
        // Debug.Log(currentState);
        switch (currentState)
        {
            case SoldierState.MovingToGuardPos:
                ChangeMovingToGuardposStateTo();
                UnTargetEnemy();
                SetSoldierDirect(guardPos);
                MoveToStandingPos();
                break;
            
            case SoldierState.MovingToEnemy:
                SetTargetEnemy();
                if(targetEnemy != null) SetSoldierDirect(targetEnemy.transform.position);
                MoveToTargetEnemy(); 
                CheckChangeMovingToEnemyStateTo();
                break;

            case SoldierState.ActtackingEnemy:
                ActtkingTargetEnemy();
                CheckChangeAttackingEnemyStateTo();
                break;
            case SoldierState.Die:
                break;
        }
    }

    public void GetOffsetPos()
    {
        switch (index)
        {
            case 0: offsetPos = new Vector2(-0.15f, -0.15f);
            break;
            case 1: offsetPos = new Vector2(0, 0);
            break;
            case 2: offsetPos = new Vector2(-0.15f, 0.15f);
            break;
        }
    }

    #region MOVING TO STANDING POSITION
    private void UnTargetEnemy()
    {
        if(targetEnemy == null) return;
        targetEnemy.targetSoldier = null;
        targetEnemy = null;
        hadTarget = false;
    }
    void SetSoldierDirect(Vector2 targetPos)
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
            MoveTo(guardPos);
        }
        else
        {
            isReachGuardPos = true;
            unitAnimation.UnitPlayIdle();
        }
    }

    private void ChangeMovingToGuardposStateTo()
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
        if((Vector2)transform.position == guardPos) return true;
        else return false;
    }
    #endregion

    #region MOVING TO TARGET ENEMY
    public void SetTargetEnemy()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (hadTarget == false && enemiesInRange[i].targetSoldier == null)
            {
                enemiesInRange[i].targetSoldier = this;
                targetEnemy = enemiesInRange[i];
                hadTarget = true;
                break;
            } 
        }

        // If there are no unoccupied enemies, this soldier will assist in attacking other soldier's occupied enemy
        if(TargetEnemyNotActive())
        {
            hadTarget = false;
            isReachTargetEnemyFontPos = false; 
            if(enemiesInRange.Count > 0) targetEnemy = enemiesInRange[0];
        }
    }

    private void MoveToTargetEnemy()
    {
        if(TargetEnemyNotActive())
        {
            return;
        }

        targetEnemyFontPos = (Vector2)targetEnemy.fontPoint.position + offsetPos;

        if((Vector2)transform.position != targetEnemyFontPos)
        {
            isReachTargetEnemyFontPos = false;
            MoveTo(targetEnemyFontPos);
        }
        else
        {
            targetEnemy.islockByEnemy = true;
            isReachTargetEnemyFontPos = true;
            unitAnimation.UnitPlayIdle();
        }
    }

    private void CheckChangeMovingToEnemyStateTo()
    {
        if(TargetEnemyActive() && isReachTargetEnemyFontPos == true)
        {
            currentState = SoldierState.ActtackingEnemy;
        }
    }
    #endregion

    #region ACTTACKING ENEMY
    private void CheckChangeAttackingEnemyStateTo()
    {
        if(hadTarget == true) return;

        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i].targetSoldier == null)
            {
                currentState = SoldierState.MovingToEnemy;
            } 
        }
    }

    private void ActtkingTargetEnemy()
    {
        if(TargetEnemyActive())
        {
            unitAnimation.UnitPlayAttack();
        }
        else if(TargetEnemyNotActive())
        {
            ResetSoldierState();
            currentState = SoldierState.MovingToEnemy;
        }      
    }

    public override void DealDamage()
    {
        if(targetEnemy != null && targetEnemy.CurrentHp > 0)
        {
            targetEnemy.TakeDamage(Damage);
        }
    }
    #endregion

    #region TAKE DAMAGE AND RETURN TO POOL WHEN DIE
    // take damage and Die
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(CurrentHp == 0)
        {
            OnSoldierDeath?.Invoke(this);
        }
    }

    public IEnumerator ReturnPoolAfterPlayAnimation(UnitPool unitPool)
    {
        currentState = SoldierState.ActtackingEnemy;
        ResetSoldierState();
        yield return new WaitForSeconds(unitAnimation.GetCurrentAnimationLength());
        unitPool.ReturnSoldier(this);
        yield break;
    }

    public override void ResetUnit()
    {   
        base.ResetUnit();
    }
    #endregion

    private void MoveTo(Vector2 pos)
    {
        unitAnimation.SpriteSortingOrder();
        unitAnimation.UnitPlayWalk();
        transform.position = Vector2.MoveTowards(transform.position, pos, MoveSpeed *Time.deltaTime);
    }

    public void GuardPointChange()
    {
        ResetSoldierState();
        currentState = SoldierState.MovingToGuardPos;
    }

    private bool TargetEnemyNotActive()
    {
        return targetEnemy == null || targetEnemy.CurrentHp == 0;
    }

    private bool TargetEnemyActive()
    {
        return targetEnemy != null && targetEnemy.CurrentHp > 0;
    }

    public void ResetSoldierState()
    {
        isReachGuardPos = false;
        isReachTargetEnemyFontPos = false;
        hadTarget = false;

        if(targetEnemy != null)
        {
            targetEnemy.ResetEnemyState();
            targetEnemy = null;
        }
    }
}
