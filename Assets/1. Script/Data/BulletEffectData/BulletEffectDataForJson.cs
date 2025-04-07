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
            effectType          = "Slow",
            effectValue         = 50,
            effectDuration      = 1,
            effectOccursTime    = 0,
            effectRange         = 0,
        },

        // Magic_DoT effect
        new BulletEffectData
        {
            effectType          = "Magic_DoT",
            effectValue         = 15,
            effectDuration      = 1.5f,
            effectOccursTime    = 3,
            effectRange         = 0,
        },

        // Fire_DoT effect
        new BulletEffectData
        {
            effectType          = "Fire_DoT",
            effectValue         = 5,
            effectDuration      = 2,
            effectOccursTime    = 8,
            effectRange         = 0,
        },

        // Bomb1_AoE20 effect
        new BulletEffectData
        {
            effectType          = "Bomb1_AoE20",
            effectValue         = 20,
            effectDuration      = 0,
            effectOccursTime    = 0,
            effectRange         = 1.25f,
        },

        // Bomb2_AoE30 effect
        new BulletEffectData
        {
            effectType          = "Bomb2_AoE30",
            effectValue         = 30,
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
