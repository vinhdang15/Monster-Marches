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
        enemy.CurrentSpeed = enemy.MoveSpeed*(1-value/100);
        enemy.ApplyEffectColor(Color.cyan);
        yield return new WaitForSeconds(duration);
        enemy.ResetColor();
        enemy.ResetCurrentSpeed();
        enemy.underEffect.Remove(type);
        yield break;
    }
    
}
