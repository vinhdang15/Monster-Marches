using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTowerView : TowerViewBase
{
    [SerializeField] Transform          spawnBulletLeftTrans;
    [SerializeField] Transform          spawnBulletRightTrans;
    [SerializeField] float              spawnBulletAngle;
    private Transform                   spawnBulletTrans;
    private float                       spawnBulletDirection;
    
    public void FireBulletAnimation(Transform target)
    {
        if(transform.position.x < target.position.x)
        {
            towerAnimation.PlayShootingInRight();
            spawnBulletTrans = spawnBulletRightTrans;
            spawnBulletDirection = spawnBulletAngle;
        }
        else
        {
            towerAnimation.PlayShootingInLeft();
            spawnBulletTrans = spawnBulletLeftTrans;
            spawnBulletDirection = 180 - spawnBulletAngle;
        }
    }

    public Vector2 GetSpawnBulletPos()
    {
        return spawnBulletTrans.position;
    }

    public float GetSpawnBulletDirection()
    {
        return spawnBulletDirection;
    }
}
