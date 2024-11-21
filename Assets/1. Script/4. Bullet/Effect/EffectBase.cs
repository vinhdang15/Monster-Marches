using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectBase : IEffect
{
    protected string  type;
    protected float   value;
    protected float   duration;
    protected int     occursTime;
    protected float   range;

    public void Init(string type, float value, float duration, int occursTime, float range)
    {
        this.type           = type;
        this.value          = value;
        this.duration       = duration;
        this.occursTime     = occursTime;
        this.range          = range;
    }

    public virtual IEnumerator ApplyEffect(UnitBase enemy)
    {
        yield return null;
    }
}
