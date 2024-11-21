using System.Collections;
using UnityEngine;
public class DamageOverTimeEffect : EffectBase
{
    public DamageOverTimeEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime,range);
    }

    public override IEnumerator ApplyEffect(UnitBase enemy)
    {
        //enemy.ApplyDamageOverTimeCoroutine(this, type, value, duration, occursTime, range);
        if(enemy.activeEffect.ContainsKey(type)) yield break;
        enemy.activeEffect.Add(type, this);

        float timeAmong = duration / occursTime;
        while(occursTime - 1 > 0)
        {   
            yield return new WaitForSeconds(timeAmong);
            if(enemy.CurrentHp > 0)
            {
                enemy.TakeDamage(value);
                occursTime --;
            }
            else
            {
                occursTime = 0;
            }
        }
        enemy.activeEffect.Remove(type);
    }
}
