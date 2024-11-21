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
        //enemy.ApplySlow(this, type, value, duration, occursTime, range);
        if(enemy.activeEffect.ContainsKey(type)) yield break;
        enemy.activeEffect.Add(type, this);
        enemy.CurrentSpeed = enemy.Speed*(1-value/100);
        yield return new WaitForSeconds(duration);
        enemy.activeEffect.Remove(type);
    }
    
}
