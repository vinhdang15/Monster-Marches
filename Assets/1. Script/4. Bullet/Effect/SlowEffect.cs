using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : EffectBase
{
    public SlowEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime,range);
    }

    public override void Apply(UnitBase enemy)
    {
        enemy.ApplySlow(this, type, value, duration, occursTime, range);
    }
}
