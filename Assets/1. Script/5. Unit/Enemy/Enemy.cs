using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : UnitBase
{
    [SerializeField] PathFinder pathFinder;
    private List<Enemy> surroundingEnemies = new List<Enemy>();
    public Transform fontPoint;
    public Soldier targetSoldier;
    public bool islockByEnemy;
    public event Action<Enemy> OnEnemyDeath;

    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
    }

    // bullet instantitate in pool and bullet event register every time tower GetBullet,
    // so needed to OnEnemyDeath = null; every time it OnDisable()
    private void OnDisable()
    {
        OnEnemyDeath = null;
    }

    // Get Pathway
    public void GetPathConfigSO(PathConfigSO pathConfigSO)
    {
        pathFinder.PathConfigSO = pathConfigSO;
    }

    public void SetPosInPathWave(int _pathWaveIndex)
    {
        pathFinder.OnSetPosInPathWay(_pathWaveIndex);
    }

    // Move
    public void EnemyAction()
    {
        if(CurrentHp == 0) return;
        // else if (targetSoldier != null && !targetSoldier.isReachTargetEnemyFontPos && targetSoldier.isReachGuardPos)
        else if (targetSoldier != null && targetSoldier.currentState == Soldier.SoldierState.MovingToEnemy)
        {
            unitAnimation.UnitPlayIdle();
        }
        else if (targetSoldier != null && targetSoldier.isReachTargetEnemyFontPos)
        {
            unitAnimation.UnitPlayAttack();
        }
        else
        {
            unitAnimation.UnitPlayWalk();
            pathFinder.FollowPath(CurrentSpeed);
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

        if(CurrentHp == 0)
        {
            OnEnemyDeath?.Invoke(this);
        }
    }

    public override void DealDamage()
    {
        if(targetSoldier != null && targetSoldier.CurrentHp > 0)
        {
            targetSoldier.TakeDamage(Damage);
        }
    }

    public IEnumerator ReturnPoolAfterPlayAnimation(UnitPool unitPool)
    {
        yield return new WaitForSeconds(unitAnimation.GetCurrentAnimationLength());
        unitPool.ReturnEnemy(this);
        yield break;
    }

    // use ResetEnemyState when soldier untarget this enemy
    public void ResetEnemyState()
    {
        targetSoldier = null;
        islockByEnemy = false;
    }

    // use ResetUnit when return to pool
    public override void ResetUnit()
    {
        targetSoldier = null;
        islockByEnemy = false;
        base.ResetUnit();
    }

}
