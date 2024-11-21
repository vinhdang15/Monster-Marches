using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<BulletBase> Bullets = new List<BulletBase>();

    private void Update()
    {
        if(Bullets.Count == 0) return;
        foreach(var bullet in Bullets)
        {
            bullet.MoveToTarget();
            bullet.UpdateBulletDirection();
        }
    }

    public void AddBullet(BulletBase bulletBase)
    {
        Bullets.Add(bulletBase);
        bulletBase.OnReachEnemyPos += HandleReachingEnemyPos;
    }
    
    private void HandleReachingEnemyPos(BulletBase bullet)
    {
        bullet.ReachingEnemyPos();
    }
}
