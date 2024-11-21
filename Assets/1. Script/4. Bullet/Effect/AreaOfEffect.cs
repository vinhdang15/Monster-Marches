using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : EffectBase
{
    public AreaOfEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime,range);
    }

    public override void Apply(UnitBase enemy)
    {
        enemy.ApplyAreaOfEffect(type, value, duration, occursTime, range);
    }
}
