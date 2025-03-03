using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{
    public static IEffect CreateEffect(string type, float value, float duration, int occursTime, float range)
    {
        switch (type)
        {
            case string t when t.Contains("slow"):
                return new SlowEffect(type, value, duration, occursTime, range);
            case string t when t.Contains ("dot"):
                return new DamageOverTimeEffect(type, value, duration, occursTime, range);
            case string t when t.Contains ("aoe"):
                return new AreaOfEffect(type, value, duration, occursTime, range);
            default :
                return null;
        }
    }
}
