using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class TowerManager : MonoBehaviour
{
    public BulletManager bulletManager;
    [SerializeField] CSVTowerDataReader towerDataReader;
    [SerializeField] List<TowerView> towerPrefabList = new List<TowerView>();
    public List<TowerPresenter> towerList = new List<TowerPresenter>();
    public Dictionary<TowerPresenter, PresenterData> towerExtraData = new Dictionary<TowerPresenter, PresenterData>();
    public EmptyPlot        selectedEmptyPlot;

    #region INIT TOWER
    public void InitTower(Vector3 pos, TowerType towerType)
    {
        // init tower presenter
        TowerData towerData             = towerDataReader.towerDataList.GetTowerData(towerType.ToString(), 1);
        TowerView towerView             = Instantiate(towerPrefabList[(int)towerType], pos, Quaternion.identity, transform);
        TowerModel towerModel           = TowerModel.Craete(towerView,towerData);
        TowerPresenter towerPresenter   = TowerPresenter.Create(towerModel, towerView);

        // Register selected TowerView, or selected TowerView null enemy in range, enemy out range
        towerPresenter.towerView.OnEnemyEnter           += (enmey, view) => HanldeEnemyEnter(enmey, towerPresenter);
        towerPresenter.towerView.OnEnemyExit            += (enmey, view) => HanldeEnemyExit(enmey, towerPresenter);

        // init tower towerExtraData
        towerExtraData[towerPresenter]                      = new PresenterData();
        towerExtraData[towerPresenter].emptyPlot            = selectedEmptyPlot;
        towerExtraData[towerPresenter].emptyPlot.HideEmptyPlot();
        towerExtraData[towerPresenter].RangeDetectUpgrade   = towerDataReader.towerDataList.GetRangeDetect(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldUpdrade          = towerDataReader.towerDataList.GetGoldRequired(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldRefund            += towerData.goldRequired;

        // towerList is no use at the moment
        towerList.Add(towerPresenter);    
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
        InitTower(pos,TowerType.BarrackTower);
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
    private void HanldeEnemyEnter(UnitBase enemy, TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter].enemies.Add(enemy);

        if(towerExtraData[towerPresenter].enemies.Count == 1)
        {
            towerExtraData[towerPresenter].spawnCoroutine = StartCoroutine(SpawnBulletCorountine(towerPresenter));
        }
    }

    private void HanldeEnemyExit(UnitBase enemy, TowerPresenter towerPresenter)
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
        List<UnitBase> towerPresentEnemiesList = towerExtraData[towerPresenter].enemies;
        TowerModel towerModel = towerPresenter.towerModel;
        TowerView towerView = towerPresenter.towerView;

        while(towerPresentEnemiesList.Count > 0)
        {
            towerView.FireBulletAnimation();
            yield return new WaitForSeconds(0.5f);

            string bulletType = towerModel.BulletType;
            Vector2 spawnPos = towerView.GetSpawnBulletPos();

            if(towerPresentEnemiesList[0] != null)
            {
                bulletManager.AddBullet(bulletType,spawnPos,towerPresentEnemiesList[0]);
            }
            
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(towerPresenter.towerModel.FireRate - 1);
        }
    }
    #endregion

}

public enum TowerType
{
    ArcherTower = 0,
    MageTower = 1,
    BarrackTower = 2,
    CannonTower = 3
}

[System.Serializable]
public class PresenterData 
{
    public List<UnitBase>  enemies = new List<UnitBase>();
    public Coroutine    spawnCoroutine;
    public EmptyPlot    emptyPlot;
    public float        RangeDetectUpgrade { set ; get ; }
    public int          GoldUpdrade { set ; get ; }
    public int          GoldRefund { set ; get ; } = 0;
}
