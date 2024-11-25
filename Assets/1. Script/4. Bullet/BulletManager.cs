using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<BulletBase> Bullets = new List<BulletBase>();

    public void AddBullet(BulletBase bulletBase)
    {
        Bullets.Add(bulletBase);
        bulletBase.OnReachEnemyPos += HandleReachingEnemyPos;
        StartCoroutine(bulletBase.MoveToTargetCoroutine());
    }
    
    private void HandleReachingEnemyPos(BulletBase bullet)
    {
        bullet.ReachingEnemyPos();
    }
}
