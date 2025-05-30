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
            damage          = 50,
            speed           = 8.5f,
            effectTyes      = BulletEffectType.none.ToString(),
        },
        new BulletData
        {
            bulletID        = BulletID.Arrow_2.ToString(),
            damage          = 50,
            speed           = 9,
            effectTyes      = BulletEffectType.Fire_DoT.ToString(),
        },

        // MagicBall bullet
        new BulletData
        {
            bulletID        = BulletID.MagicBall_1.ToString(),
            damage          = 70,
            speed           = 3,
            effectTyes      = BulletEffectType.Slow.ToString(),
        },
        new BulletData
        {
            bulletID        = BulletID.MagicBall_2.ToString(),
            damage          = 70,
            speed           = 4,
            effectTyes      = $"{BulletEffectType.Magic_DoT};{BulletEffectType.Slow}"
        },

        // Bomb bullet
        new BulletData
        {
            bulletID        = BulletID.Bomb_1.ToString(),
            damage          = 60,
            speed           = 5,
            effectTyes      = BulletEffectType.Bomb_1_AoE.ToString(),
        },
        new BulletData
        {
            bulletID        = BulletID.Bomb_2.ToString(),
            damage          = 90,
            speed           = 5,
            effectTyes      = BulletEffectType.Bomb_2_AoE.ToString(),
        },
    };

     public List<BulletData> GetBulletDataForJson()
    {
        return bulletDatas;
    }
}
