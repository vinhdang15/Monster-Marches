using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTowerManager : TowerBaseManager
{
    [SerializeField] BulletManager          bulletManager;
    [SerializeField] List<TowerView>        towerPrefabList = new List<TowerView>();
    public Dictionary<TowerPresenter, BulletTowerInfor> bulletTowerInfor = new Dictionary<TowerPresenter, BulletTowerInfor>();
    public void InitTower(Vector3 pos, TowerType towerType, EmptyPlot emptyPlot)
    {
        TowerData towerData = towerDataReader.towerDataList.GetTowerData(towerType.ToString().Trim().ToLower(), 1);

        TowerView towerPrefab = towerPrefabList[(int)towerType];
        TowerPresenter towerPresenter = base.InitBuildingPresenter(towerPrefab, towerData, pos);

        bulletTowerInfor[towerPresenter]       = new BulletTowerInfor();
        base.AddTowerPersenterEmptyPlot(towerPresenter, emptyPlot);
        RegisterTowerEvent(towerPresenter);
    }

    private void RegisterTowerEvent(TowerPresenter towerPresenter)
    {
        towerPresenter.towerView.OnEnemyEnter   += (enmey, view) => HanldeEnemyEnter(enmey, towerPresenter);
        towerPresenter.towerView.OnEnemyExit    += (enmey, view) => HanldeEnemyExit(enmey, towerPresenter);
    }

    public void InitArcherTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        InitTower(pos,TowerType.ArcherTower, emptyPlot);
    }

    public void InitMageTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        InitTower(pos,TowerType.MageTower, emptyPlot);
    }

    public void InitCannonTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        InitTower(pos,TowerType.CannonTower, emptyPlot);
    }

    #region PROCESS DETECT ENEMY AND SPAWN BULLET
    private void HanldeEnemyEnter(Enemy enemy, TowerPresenter towerPresenter)
    {
        bulletTowerInfor[towerPresenter].enemies.Add(enemy);

        if(bulletTowerInfor[towerPresenter].enemies.Count == 1)
        {
            bulletTowerInfor[towerPresenter].spawnBulletCoroutine = StartCoroutine(SpawnBulletCorountine(towerPresenter));
        }
    }

    private void HanldeEnemyExit(Enemy enemy, TowerPresenter towerPresenter)
    {
        bulletTowerInfor[towerPresenter].enemies.Remove(enemy);

        if(bulletTowerInfor[towerPresenter].enemies.Count == 0)
        {
            if(bulletTowerInfor[towerPresenter].spawnBulletCoroutine == null) return;
            StopCoroutine(bulletTowerInfor[towerPresenter].spawnBulletCoroutine);
        }
    }

    private IEnumerator SpawnBulletCorountine(TowerPresenter towerPresenter)
    {
        List<Enemy> towerPresentEnemiesList = bulletTowerInfor[towerPresenter].enemies;
        TowerModel towerModel = towerPresenter.towerModel;
        TowerView towerView = towerPresenter.towerView;

        while(towerPresentEnemiesList.Count > 0)
        {
            towerView.FireBulletAnimation();
            yield return new WaitForSeconds(0.2f);

            string bulletType = towerModel.BulletType;
            Vector2 spawnPos = towerView.GetSpawnBulletPos();

            if(towerPresentEnemiesList.Count > 0 && towerPresentEnemiesList[0].CurrentHp > 0)
            {
                bulletManager.SpawnBullet(bulletType,spawnPos,towerPresentEnemiesList[0]);
            }
            yield return new WaitForSeconds(towerPresenter.towerModel.FireRate - 0.2f);
        }
    }
    #endregion
}

[System.Serializable]
public class BulletTowerInfor 
{
    public List<Enemy>      enemies = new List<Enemy>();
    public Coroutine        spawnBulletCoroutine;
}
