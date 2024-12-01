using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitBase
{
    [SerializeField] PathFinder pathFinder;
    private List<Enemy> surroundingEnemies = new List<Enemy>();
    public event Action<Enemy> OnEnemyDeath;

    // Get Pathway
    public void GetPathConfigSO(PathConfigSO _pathFinder)
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.PathConfigSO = _pathFinder;
    }

    public void SetPosInPathWave(int _pathWaveIndex)
    {
        pathFinder.OnSetPosInPathWay(_pathWaveIndex);
    }

    // Move
    public override void Move()
    {
        if(CurrentHp == 0) return;
        pathFinder.FollowPath(CurrentSpeed);
    }

    // Pos and Moving direction
    public override void SetMovingDirection()
    {
        if(CurrentPos == null || CurrentHp == 0) return;
        float x = transform.position.x - CurrentPos.x;
        if(x < 0) transform.localScale = new(-1,1);
        else if(x < 0) transform.localScale = new(1,1);
        CurrentPos = transform.position;
    }

    // take damage and Die
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(CurrentHp == 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        OnEnemyDeath?.Invoke(this);
        StartCoroutine(DestroyAfterPlayAnimation());
    }

    public IEnumerator DestroyAfterPlayAnimation()
    {
        yield return new WaitForSeconds(unitAnatation.GetCurrentAnimationLength());
        Destroy(this.gameObject);
    }
}
