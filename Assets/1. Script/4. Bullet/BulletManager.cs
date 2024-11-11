using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public List<BulletBase> Bullets = new();
    public BulletBase GetBullet(string bulleType)
    {
        return Bullets.Find(bullet => bullet.bulletType == bulleType);
    }
}
