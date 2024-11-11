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
    // public BulletBase GetBullet(string bulleType)
    // {
    //     return Bullets.Find(bullet => bullet.BulletType == bulleType);
    // }
}
