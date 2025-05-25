using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTowerManager : TowerBaseManager
{
    private BulletManager       bulletManager;
    private BulletDataReader    bulletDataReader;
    [SerializeField] List<TowerViewBase>    towerPrefabList = new List<TowerViewBase>();
    [SerializeField] List<TowerPresenter>   bulletTowerList;
    private Dictionary<TowerPresenter, BulletTowerInfor> bulletTowerInfor = new Dictionary<TowerPresenter, BulletTowerInfor>();
    

    public void PrepareGame(TowerDataReader towerDataReader, BulletDataReader bulletDataReader)                     
    {
        this.towerDataReader = towerDataReader;
        this.bulletDataReader = bulletDataReader;
        bulletManager = FindObjectOfType<BulletManager>();
    }
    
    private void OnDisable()
    {
        foreach(var towerPresenter in bulletTowerInfor.Keys)
        {
            UnRegisterTowerEvent(towerPresenter);
        }
    }

    #region INIT BUILDING
    public void Init(Vector3 pos, TowerType towerType, EmptyPlot emptyPlot,
                        TowerDataReader towerDataReader, BulletDataReader bulletDataReader)

    {
        TowerData towerData = towerDataReader.towerDataListSO.GetTowerData(towerType.ToString(), 1);
        TowerViewBase towerPrefab = towerPrefabList[(int)towerType];
        TowerPresenter towerPresenter = base.InitBuildingPresenter(towerPrefab, towerData, pos,
                                                                    towerDataReader, bulletDataReader);

        towerPresenter.getTargetEnemyHandler.LoadComponents(towerPresenter, bulletDataReader);
        towerPresenter.getTargetEnemyHandler.CheckBulletEffectInfo();

        bulletTowerInfor[towerPresenter] = new BulletTowerInfor();
        base.AddTowerPersenterEmptyPlot(towerPresenter, emptyPlot);
        RegisterTowerEvent(towerPresenter);

        bulletTowerList.Add(towerPresenter);
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
        Init(pos, TowerType.ArcherTower, emptyPlot, towerDataReader, bulletDataReader);
                                                                    
    }

    public void InitMageTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        Init(pos, TowerType.MageTower, emptyPlot, towerDataReader, bulletDataReader);
    }

    public void InitCannonTower(Vector3 pos, EmptyPlot emptyPlot)
    {
        Init(pos, TowerType.CannonTower, emptyPlot,towerDataReader, bulletDataReader);
    }
    #endregion

    #region UPGRADE
    public override void UpgradeTower(TowerPresenter towerPresenter)
    {
        base.UpgradeTower(towerPresenter);
        towerPresenter.getTargetEnemyHandler.CheckBulletEffectInfo();
    }
    #endregion

    #region PROCESS DETECT ENEMY AND SPAWN BULLET
    private void HanldeEnemyEnter(Enemy enemy, TowerPresenter towerPresenter)
    {
        bulletTowerInfor[towerPresenter].enemies.Add(enemy);

        if (bulletTowerInfor[towerPresenter].enemies.Count == 1)
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
        SelectTargetEnemyHandler getTargetEnemyHandler = towerPresenter.getTargetEnemyHandler;

        while (towerPresentEnemiesList.Count > 0)
        {
            // bulletTowerView.FireBulletAnimation(towerPresentEnemiesList[0].transform);
            // yield return new WaitForSeconds(towerModel.FireAnimDelay);

            // string bulletType = towerModel.SpawnObject;
            // Vector2 spawnPos = bulletTowerView.GetSpawnBulletPos();
            // float spawnBulletDirection = bulletTowerView.GetSpawnBulletDirection();

            // if (towerPresentEnemiesList.Count > 0 && !towerPresentEnemiesList[0].isDead)
            // {
            //     bulletManager.SpawnBullet(bulletType, spawnPos, spawnBulletDirection, towerPresentEnemiesList[0], towerPresenter);
            // }
            // yield return new WaitForSeconds(towerPresenter.towerModel.SpawnRate);

            bulletTowerView.FireBulletAnimation(towerPresentEnemiesList[0].transform);
            yield return new WaitForSeconds(towerModel.FireAnimDelay);

            string bulletType = towerModel.SpawnObject;
            Vector2 spawnPos = bulletTowerView.GetSpawnBulletPos();
            float spawnBulletDirection = bulletTowerView.GetSpawnBulletDirection();

            Enemy enemy = getTargetEnemyHandler.GetTargetEnemy(towerPresentEnemiesList);
            if (enemy != null)
            {
                bulletManager.SpawnBullet(bulletType, spawnPos, spawnBulletDirection, enemy, towerPresenter);
            }
            yield return new WaitForSeconds(towerPresenter.towerModel.SpawnRate);
        }
    }
    #endregion

    public void CleanupSelectedTower(TowerPresenter selectedTower)
    {
        bulletTowerList.Remove(selectedTower);
        Destroy(selectedTower.gameObject);
    }

    public void ClearBulletTowers()
    {
        foreach(var bulletTower in bulletTowerList)
        {
            Destroy(bulletTower.gameObject);
        }
        bulletTowerList.Clear();
    }
}

[System.Serializable]
public class BulletTowerInfor 
{
    public List<Enemy>      enemies = new List<Enemy>();
    public Coroutine        spawnBulletCoroutine;
}
