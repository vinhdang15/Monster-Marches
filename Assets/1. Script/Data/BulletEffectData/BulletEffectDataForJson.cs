using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectDataForJson : MonoBehaviour
{
    private List<BulletEffectData> bulletEffectDatas = new()
    {
        // Slow effect
        new BulletEffectData
        {
            effectType          = BulletEffectType.Slow.ToString(),
            effectValue         = 50,
            effectDuration      = 2,
            effectOccursTime    = 0,
            effectRange         = 0,
        },

        // Magic_DoT effect
        new BulletEffectData
        {
            effectType          = BulletEffectType.Magic_DoT.ToString(),
            effectValue         = 15,
            effectDuration      = 1.5f,
            effectOccursTime    = 3,
            effectRange         = 0,
        },

        // Fire_DoT effect
        new BulletEffectData
        {
            effectType          = BulletEffectType.Fire_DoT.ToString(),
            effectValue         = 5,
            effectDuration      = 2,
            effectOccursTime    = 8,
            effectRange         = 0,
        },

        // Bomb1_AoE20 effect
        new BulletEffectData
        {
            effectType          = BulletEffectType.Bomb_1_AoE.ToString(),
            effectValue         = 50,
            effectDuration      = 0,
            effectOccursTime    = 0,
            effectRange         = 1.25f,
        },

        // Bomb2_AoE30 effect
        new BulletEffectData
        {
            effectType          = BulletEffectType.Bomb_2_AoE.ToString(),
            effectValue         = 90,
            effectDuration      = 0,
            effectOccursTime    = 0,
            effectRange         = 1.25f,
        }
    };

    public List<BulletEffectData> GetBulletEffectDatas()
    {
        return bulletEffectDatas;
    }
}
