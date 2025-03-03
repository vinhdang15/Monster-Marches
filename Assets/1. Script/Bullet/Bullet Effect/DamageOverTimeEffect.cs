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
        float timeAmong = duration / occursTime;
        int efectOccursTime = occursTime;
        while(efectOccursTime - 1 > 0)
        {   
            yield return new WaitForSeconds(timeAmong);
            if(enemy.CurrentHp > 0)
            {
                enemy.TakeDamage(value);
                efectOccursTime --;
            }
            else
            {
                efectOccursTime = 0;
            }
        }
        enemy.underEffect.Remove(type);
        yield break;
    }
}
