using System.Collections;
using UnityEngine;

public class SlowEffect : EffectBase
{
    public SlowEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime,range);
    }

    public override IEnumerator ApplyEffect(UnitBase enemy)
    {
        enemy.CurrentSpeed = enemy.Speed*(1-value/100);
        yield return new WaitForSeconds(duration);
        enemy.ResetCurrentSpeed();
        enemy.underEffect.Remove(type);
        yield break;
    }
    
}
