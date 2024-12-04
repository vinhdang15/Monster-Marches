using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] BulletPool bulletPool;
    public List<BulletBase> activeBullets = new List<BulletBase>();

    private void Update()
    {
        if(activeBullets.Count == 0) return;
        foreach(var bullet in activeBullets)
        {
            bullet.MoveToTarget();
        }
    }

    public void SpawnBullet(string bulletType, Vector2 initPos, UnitBase _enemy)
    {
        BulletBase bullet = bulletPool.GetBullet(bulletType, initPos);
        bullet.InitBulletTarget(_enemy);
        bullet.OnFinishBulletAnimation += HandleFinishBulletAnimation;
        activeBullets.Add(bullet);
    }
    
    private void HandleFinishBulletAnimation(BulletBase bullet)
    {
        bullet.OnFinishBulletAnimation -= HandleFinishBulletAnimation;
        activeBullets.Remove(bullet);
        bulletPool.ReturnBullet(bullet);
    }
}
