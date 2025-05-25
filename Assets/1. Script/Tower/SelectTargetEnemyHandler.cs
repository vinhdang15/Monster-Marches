using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectTargetEnemyHandler : MonoBehaviour
{
    private TowerPresenter towerPresenter;
    private BulletDataReader bulletDataReader;

    private bool hasBulletSlowEffect;

    public void LoadComponents(TowerPresenter towerPresenter, BulletDataReader bulletDataReader)
    {
        this.towerPresenter = towerPresenter;
        this.bulletDataReader = bulletDataReader;
    }

    public void CheckBulletEffectInfo()
    {
        string bulletType = towerPresenter.towerModel.SpawnObject.ToString();
        BulletData bulletDatas = bulletDataReader.bulletDataListSO.GetBulletData(bulletType);
        if (bulletDatas == null) return;
        if (bulletDatas.effectTyes.ToString().Contains(BulletEffectType.Slow.ToString()))
        {
            hasBulletSlowEffect = true;
        }
        else
        {
            hasBulletSlowEffect = false;
        }
    }

    public Enemy GetTargetEnemy(List<Enemy> enemies)
    {
        Enemy enemy = enemies[0];

        if (hasBulletSlowEffect)
        {
            foreach (var e in enemies)
            {
                if (!e.underEffect.ContainsKey(BulletEffectType.Slow.ToString()))
                {
                    enemy = e;
                }
            }
        }

        if ((enemies.Count > 0) && (enemy.CurrentHp > 0)) return enemy;
        else return null;
    }
}
