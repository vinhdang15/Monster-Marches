using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaOfEffect : EffectBase
{
    public AreaOfEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime,range);
    }

    public override IEnumerator ApplyEffect(UnitBase enemy)
    { 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.transform.position, range, LayerMask.GetMask("Enemy"));
        foreach(var enemyColl in hitColliders)
        {
            Enemy enemyNearBy = enemyColl.GetComponent<Enemy>();
            if(enemyNearBy != enemy && enemyNearBy.CurrentHp > 0) enemyNearBy.TakeDamage(value);
        }
        enemy.underEffect.Remove(type);
        yield break;
    }
}
