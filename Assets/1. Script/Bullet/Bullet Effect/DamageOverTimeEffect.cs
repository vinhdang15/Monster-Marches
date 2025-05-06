using System.Collections;
using UnityEngine;
public class DamageOverTimeEffect : EffectBase
{
    public DamageOverTimeEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime, range);
    }

    public override IEnumerator ApplyEffect(UnitBase enemy)
    {
        float timeAmong = duration / occursTime;
        int efectOccursTime = occursTime;
        while(efectOccursTime > 0)
        {   
            enemy.ApplyEffectFlashColor(Color.red);
            yield return new WaitForSeconds(timeAmong);
            if(enemy.CurrentHp > 0)
            {
                enemy.TakeDamage(value);
                enemy.ApplyEffectFlashColor(Color.red);
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
