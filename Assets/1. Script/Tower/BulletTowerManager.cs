using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTowerManager : TowerBaseManager
{
    [SerializeField] BulletManager          bulletManager;
    [SerializeField] List<TowerViewBase>    towerPrefabList = new List<TowerViewBase>();
    [SerializeField] List<TowerPresenter>   bulletTOwerList;
    private Dictionary<TowerPresenter, BulletTowerInfor> bulletTowerInfor = new Dictionary<TowerPresenter, BulletTowerInfor>();

    public void PrepareGame()
    {
        bulletManager = FindObjectOfType<BulletManager>();
    }
    
    private void OnDisable()
    {
        foreach(var towerPresenter in bulletTowerInfor.Keys)
        {
            UnRegisterTowerEvent(towerPresenter);
        }
    }

    public void InitTower(Vector3 pos, TowerType towerType, EmptyPlot emptyPlot)
    {
        TowerData towerData = TowerDataReader.Instance.towerDataListSO.GetTowerData(towerType.ToString(), 1);
        TowerViewBase towerPrefab = towerPrefabList[(int)towerType];
        TowerPresenter towerPresenter = base.InitBuildingPresenter(towerPrefab, towerData, pos);

        bulletTowerInfor[towerPresenter]       = new BulletTowerInfor();
        base.AddTowerPersenterEmptyPlot(towerPresenter, emptyPlot);
        RegisterTowerEvent(towerPresenter);

        bulletTOwerList.Add(towerPresenter);
    }

    private void RegisterTowerEvent(TowerPresenter towerPresenter)
    {
        towerPresenter.towerViewBase.OnEnemyEnter   += (enmey, view) => HanldeEnemyEnter(enmey, towerPresenter);
        towerPresenter.towerViewBase.OnEnemyExit    += (enmey, view) => HanldeEnemyExit(enmey, towerPresenter);
    }

    private void UnRegisterTowerEvent(TowerPresenter towerPresenter)
    {
        towerPresenter.towerViewBase.OnEnemyEnter   -= (enmey, view) => HanldeEnemyEnter(enmey, towerPresenter);
        towerPresenter.towerViewBase.OnEnemyExit    -= (enmey, view) => HanldeEnemyExit(enmey, towerPresenter);
    }

    public void InitArcherTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        InitTower(pos, TowerType.ArcherTower, emptyPlot);
    }

    public void InitMageTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        InitTower(pos, TowerType.MageTower, emptyPlot);
    }

    public void InitCannonTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        InitTower(pos, TowerType.CannonTower, emptyPlot);
    }

    #region PROCESS DETECT ENEMY AND SPAWN BULLET
    private void HanldeEnemyEnter(Enemy enemy, TowerPresenter towerPresenter)
    {
        bulletTowerInfor[towerPresenter].enemies.Add(enemy);

        if(bulletTowerInfor[towerPresenter].enemies.Count == 1)
        {
            bulletTowerInfor[towerPresenter].spawnBulletCoroutine = StartCoroutine(SpawnBulletCoroutine(towerPresenter));
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

    private IEnumerator SpawnBulletCoroutine(TowerPresenter towerPresenter)
    {
        List<Enemy> towerPresentEnemiesList = bulletTowerInfor[towerPresenter].enemies;
        TowerModel towerModel = towerPresenter.towerModel;
        BulletTowerView bulletTowerView = towerPresenter.towerViewBase as BulletTowerView;

        while(towerPresentEnemiesList.Count > 0)
        {
            bulletTowerView.FireBulletAnimation(towerPresentEnemiesList[0].transform);
            yield return new WaitForSeconds(towerModel.FireAnimDelay );

            string bulletType = towerModel.SpawnObject;
            Vector2 spawnPos = bulletTowerView.GetSpawnBulletPos();
            float spawnBulletDirection = bulletTowerView.GetSpawnBulletDirection();

            if(towerPresentEnemiesList.Count > 0 && !towerPresentEnemiesList[0].isDead)
            {
                bulletManager.SpawnBullet(bulletType, spawnPos, spawnBulletDirection, towerPresentEnemiesList[0], towerPresenter);
            }
            yield return new WaitForSeconds(towerPresenter.towerModel.SpawnRate);
        }
    }
    #endregion

    public void CleanupSelectedTower(TowerPresenter selectedTower)
    {
        bulletTOwerList.Remove(selectedTower);
        Destroy(selectedTower.gameObject);
    }

    public void ClearBulletTowers()
    {
        foreach(var bulletTower in bulletTOwerList)
        {
            Destroy(bulletTower.gameObject);
        }
        bulletTOwerList.Clear();
    }
}

[System.Serializable]
public class BulletTowerInfor 
{
    public List<Enemy>      enemies = new List<Enemy>();
    public Coroutine        spawnBulletCoroutine;
}
