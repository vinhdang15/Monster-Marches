using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDataForJson : MonoBehaviour
{
    private List<BulletData> bulletDatas = new()
    {
        // Arrow bullet
        new BulletData
        {
            bulletID        = BulletID.Arrow_1.ToString(),
            damage          = 25,
            speed           = 7,
            effectTyes      = BulletEffectType.none.ToString(),
        },
        new BulletData
        {
            bulletID        = BulletID.Arrow_2.ToString(),
            damage          = 30,
            speed           = 8,
            effectTyes      = BulletEffectType.Fire_DoT.ToString(),
        },

        // MagicBall bullet
        new BulletData
        {
            bulletID        = BulletID.MagicBall_1.ToString(),
            damage          = 35,
            speed           = 4,
            effectTyes      = BulletEffectType.Slow.ToString(),
        },
        new BulletData
        {
            bulletID        = BulletID.MagicBall_2.ToString(),
            damage          = 40,
            speed           = 4,
            effectTyes      = $"{BulletEffectType.Magic_DoT};{BulletEffectType.Slow}"
        },

        // Bomb bullet
        new BulletData
        {
            bulletID        = BulletID.Bomb_1.ToString(),
            damage          = 30,
            speed           = 6,
            effectTyes      = BulletEffectType.Bomb_1_AoE20.ToString(),
        },
        new BulletData
        {
            bulletID        = BulletID.Bomb_2.ToString(),
            damage          = 50,
            speed           = 6,
            effectTyes      = BulletEffectType.Bomb_2_AoE30.ToString(),
        },
    };

     public List<BulletData> GetBulletDataForJson()
    {
        return bulletDatas;
    }
}
