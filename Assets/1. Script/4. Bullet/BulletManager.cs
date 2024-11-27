using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] BulletPool bulletPool;
    public List<BulletBase> activeBullets = new List<BulletBase>();

    public void AddBullet(string bulletType, Vector2 initPos, UnitBase _enemy)
    {
        BulletBase bullet = bulletPool.GetBullet(bulletType, initPos);
        bullet.InitBulletTarget(_enemy);
        bullet.OnReachEnemyPos += HandleReachingEnemyPos;
        StartCoroutine(bullet.MoveToTargetCoroutine());
    }
    
    private void HandleReachingEnemyPos(BulletBase bullet)
    {
        bullet.ReachingEnemyPos();
        bullet.OnReachEnemyPos -= HandleReachingEnemyPos;
        activeBullets.Remove(bullet);
        bulletPool.ReturnBullet(bullet);
    }
}
