using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{
    public static IEffect CreateEffect(string type, float value, float duration, int occursTime, float range)
    {
        switch (type)
        {
            case "slow":
                return new SlowEffect(type, value, duration, occursTime, range);
            case "dot":
                return new DamageOverTimeEffect(type, value, duration, occursTime, range);
            case "aoe20":
                return new AreaOfEffect(type, value, duration, occursTime, range);
            case "aoe30":
                return new AreaOfEffect(type, value, duration, occursTime, range);
            default :
                return null;
        }
    }
}
