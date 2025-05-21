using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] List<BulletBase> activeBullets = new List<BulletBase>();

    public void ClearBulletManager()
    {
        foreach (var bullet in activeBullets)
        {
            bullet.StopAllCoroutines();
            BulletPool.Instance.ReturnBullet(bullet);
        }
        activeBullets.Clear();
    }

    private void Update()
    {
        if (activeBullets.Count == 0) return;
        foreach (var bullet in activeBullets)
        {
            bullet.MoveToTarget();
        }
    }

    public void SpawnBullet(string bulletType, Vector2 initPos, float spawnBulletDirection, UnitBase _enemy, TowerPresenter towerPresenter)
    {
        BulletBase bullet = BulletPool.Instance.GetBullet(bulletType, initPos);
        bullet.InitBulletParent(towerPresenter);
        bullet.InitBulletTarget(_enemy);
        bullet.SetBulletInitAngle(spawnBulletDirection);
        bullet.OnFinishBulletAnimation += HandleFinishBulletAnimation;
        activeBullets.Add(bullet);
    }
    
    private void HandleFinishBulletAnimation(BulletBase bullet)
    {
        activeBullets.Remove(bullet);
        BulletPool.Instance.ReturnBullet(bullet);
    }
}
