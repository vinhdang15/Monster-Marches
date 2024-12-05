using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class TowerManager : MonoBehaviour
{
    [SerializeField] CSVTowerDataReader     towerDataReader;
    [SerializeField] BulletManager          bulletManager;
    [SerializeField] SoldierManager         soldierManager;
    [SerializeField] List<TowerView>        towerPrefabList = new List<TowerView>();
    public List<TowerPresenter>             towerList = new List<TowerPresenter>();
    public Dictionary<TowerPresenter, PresenterData> towerExtraData = new Dictionary<TowerPresenter, PresenterData>();
    public EmptyPlot        selectedEmptyPlot;

    #region INIT TOWER
    public void InitTower(Vector3 pos, TowerType towerType)
    {
        // TowerData towerData = towerDataReader.towerDataList.GetTowerData(towerType.ToString(), 1);
        // TowerPresenter towerPresenter = InitTowerPresenter(towerData, pos, towerType);
        // RegisterTowerPresenterEvent(towerPresenter);
        // InitTowerExtraData(towerData, towerType, towerPresenter);

        // init tower presenter
        TowerData towerData             = towerDataReader.towerDataList.GetTowerData(towerType.ToString().Trim().ToLower(), 1);
        TowerView towerView             = Instantiate(towerPrefabList[(int)towerType], pos, Quaternion.identity, transform);
        TowerModel towerModel           = TowerModel.Craete(towerView,towerData);
        TowerPresenter towerPresenter   = TowerPresenter.Create(towerModel, towerView);

        // init tower towerExtraData
        towerExtraData[towerPresenter]                      = new PresenterData();
        towerExtraData[towerPresenter].emptyPlot            = selectedEmptyPlot;
        towerExtraData[towerPresenter].emptyPlot.HideEmptyPlot();
        towerExtraData[towerPresenter].RangeDetectUpgrade   = towerDataReader.towerDataList.GetRangeDetect(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldUpdrade          = towerDataReader.towerDataList.GetGoldRequired(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldRefund           += towerData.goldRequired;

        // Register Tower Presenter Event
        RegisterTowerPresenterEvent(towerPresenter);

        GetBarrackGuradPoint(towerPresenter);
        SpawnBarackSoldier(towerPresenter);
        
        // towerList is no use at the moment
        towerList.Add(towerPresenter);
    }

    private TowerPresenter InitTowerPresenter(TowerData towerData, Vector3 pos, TowerType towerType)
    {
        TowerView towerView             = Instantiate(towerPrefabList[(int)towerType], pos, Quaternion.identity, transform);
        TowerModel towerModel           = TowerModel.Craete(towerView,towerData);
        TowerPresenter towerPresenter   = TowerPresenter.Create(towerModel, towerView);
        return towerPresenter;
    }

    private void InitTowerExtraData(TowerData towerData, TowerType towerType, TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter]                      = new PresenterData();
        towerExtraData[towerPresenter].emptyPlot            = selectedEmptyPlot;
        towerExtraData[towerPresenter].emptyPlot.HideEmptyPlot();
        towerExtraData[towerPresenter].RangeDetectUpgrade   = towerDataReader.towerDataList.GetRangeDetect(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldUpdrade          = towerDataReader.towerDataList.GetGoldRequired(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldRefund           += towerData.goldRequired;
    }

    private void RegisterTowerPresenterEvent(TowerPresenter towerPresenter)
    {
        if(towerPresenter.towerModel.TowerType == TowerType.Barrack.ToString().Trim().ToLower()) return;
        towerPresenter.towerView.OnEnemyEnter   += (enmey, view) => HanldeEnemyEnter(enmey, towerPresenter);
        towerPresenter.towerView.OnEnemyExit    += (enmey, view) => HanldeEnemyExit(enmey, towerPresenter);
    }

    private void GetBarrackGuradPoint(TowerPresenter towerPresenter)
    {
        if(towerPresenter.towerModel.TowerType != TowerType.Barrack.ToString().Trim().ToLower()) return;
        towerExtraData[towerPresenter].guardPoint = towerPresenter.transform.GetChild(0).GetComponent<GuardPoint>();
    }

    public void InitArcherTower(Vector3 pos)
    {
        InitTower(pos,TowerType.ArcherTower);
    }

    public void InitMageTower(Vector3 pos)
    {
        InitTower(pos,TowerType.MageTower);
    }

    public void InitBarackTower(Vector3 pos)
    {
        InitTower(pos,TowerType.Barrack);
    }

    public void InitCannonTower(Vector3 pos)
    {
        InitTower(pos,TowerType.CannonTower);
    }
    #endregion

    #region UPGRADE TOWER
    public void UpgradeTower(TowerPresenter towerPresenter)
    {
        // upgrade tower mode
        string towerType = towerPresenter.towerModel.TowerType;
        int towerLevel = towerPresenter.towerModel.Level;
        TowerData towerData = towerDataReader.towerDataList.GetTowerData(towerType, towerLevel + 1);
        towerPresenter.towerModel.UpgradeTowerModel(towerData);

        // upgrade range, range upgrade
        UpdateRangeDetection(towerPresenter);
        UpdateRangeDetectionUpgrade(towerPresenter);

        // Upgrade TowerPresenter Extra data
        UpdateTowerExtraData(towerPresenter);
    }

    private void UpdateRangeDetection(TowerPresenter towerPresenter)
    {
        float rangeDetect = towerPresenter.towerModel.RangeDetect;
        towerPresenter.towerView.SetRangeDetect(rangeDetect);
    }

    public void UpdateRangeDetectionUpgrade(TowerPresenter towerPresenter)
    {
        float rangeDetectUpgrade = towerExtraData[towerPresenter].RangeDetectUpgrade;
        towerPresenter.towerView.SetRangeDetectUpgrade(rangeDetectUpgrade);
    }
    #endregion

    public void UpdateTowerExtraData(TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter].GoldUpdrade = UpdateGoldUpgrade(towerPresenter);
        towerExtraData[towerPresenter].RangeDetectUpgrade = UpdateRangeDetectUpgradeData(towerPresenter);
    }
    private int UpdateGoldUpgrade(TowerPresenter towerPresenter)
    {
        string towerType = towerPresenter.towerModel.TowerType;
        int TowerLevel = towerPresenter.towerModel.Level;
        return towerDataReader.towerDataList.GetGoldRequired(towerType, TowerLevel + 1);
    }

    private float UpdateRangeDetectUpgradeData(TowerPresenter towerPresenter)
    {
        string towerType = towerPresenter.towerModel.TowerType;
        int TowerLevel = towerPresenter.towerModel.Level;
        return towerDataReader.towerDataList.GetRangeDetect(towerType, TowerLevel + 1);
    }

    public void AddGoldRefund(TowerPresenter towerPresenter, int gold)
    {
        towerExtraData[towerPresenter].GoldRefund += gold;
    }

    #region PROCESS DETECT ENEMY AND SPAWN BULLET
    private void HanldeEnemyEnter(Enemy enemy, TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter].enemies.Add(enemy);

        if(towerExtraData[towerPresenter].enemies.Count == 1)
        {
            towerExtraData[towerPresenter].spawnCoroutine = StartCoroutine(SpawnBulletCorountine(towerPresenter));
        }
    }

    private void HanldeEnemyExit(Enemy enemy, TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter].enemies.Remove(enemy);

        if(towerExtraData[towerPresenter].enemies.Count == 0)
        {
            if(towerExtraData[towerPresenter].spawnCoroutine == null) return;
            StopCoroutine(towerExtraData[towerPresenter].spawnCoroutine);
        }
    }

    private IEnumerator SpawnBulletCorountine(TowerPresenter towerPresenter)
    {
        List<Enemy> towerPresentEnemiesList = towerExtraData[towerPresenter].enemies;
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

    #region PROCESS SPAWN SOLDIER
    private void SpawnBarackSoldier(TowerPresenter towerPresenter)
    {
        if(towerPresenter.towerModel.TowerType != TowerType.Barrack.ToString().Trim().ToLower()) return;
        string soldierName = towerPresenter.towerModel.BulletType;
        Vector2 initPos = towerPresenter.towerView.GetSpawnBulletPos();
        GuardPoint guardPoint =  towerExtraData[towerPresenter].guardPoint;
        soldierManager.BarrackSpawnSoldier(soldierName, initPos, guardPoint);
    }
    #endregion
}

public enum TowerType
{
    ArcherTower = 0,
    MageTower = 1,
    Barrack = 2,
    CannonTower = 3
}

[System.Serializable]
public class PresenterData 
{
    public List<Enemy>      enemies = new List<Enemy>();
    public GuardPoint       guardPoint;
    public Coroutine        spawnCoroutine;
    public EmptyPlot        emptyPlot;
    public float            RangeDetectUpgrade { set ; get ; }
    public int              GoldUpdrade { set ; get ; }
    public int              GoldRefund { set ; get ; } = 0;
}
