using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TowerPresenter : MonoBehaviour
{
    public TowerModel        towerModel;
    public TowerViewBase     towerViewBase;
    public EmptyPlot         emptyPlot;
    public int               CurentTowerDamage { set ; get ; }
    public string            DescriptionUpgrade { set ; get ; }
    public int               TowerDamageUpgrade { set ; get ; }
    public float             TowerSpawnRateUpgrade { set ; get ; }
    public float             RangeDetectUpgrade { set ; get ; }
    public int               GoldUpgrade { set ; get ; }
    public int               GoldRefund { set ; get ; }

    public static TowerPresenter Create(TowerModel towerModel, TowerViewBase towerView)
    {
        TowerPresenter towerPresenter = towerView.gameObject.AddComponent<TowerPresenter>();
        towerPresenter.TowerPresenterInit(towerModel, towerView);
        return towerPresenter;
    } 
    
    public void TowerPresenterInit(TowerModel towerModel, TowerViewBase towerView)
    {
        this.towerModel = towerModel;
        this.towerViewBase  = towerView;
        InitTowerRange();
        AddGoldInitRefund();
        SetTowerPresenterData();
    }

    public void InitTowerRange()
    {
        towerViewBase.SetRangeRaycat(towerModel.RangeRaycast);
        towerViewBase.SetRangeDetect(towerModel.RangeDetect);
    }

    public void SetTowerPresenterData()
    {
        string towerType    = towerModel.TowerType;
        int towerLevel      = towerModel.Level;

        DescriptionUpgrade      = TowerDataReader.Instance.towerDataListSO.GetDescription(towerType, towerLevel + 1);
        GoldUpgrade             = TowerDataReader.Instance.towerDataListSO.GetGoldRequired(towerType, towerLevel + 1);
        RangeDetectUpgrade      = TowerDataReader.Instance.towerDataListSO.GetRangeDetect(towerType, towerLevel + 1);
        TowerSpawnRateUpgrade   = TowerDataReader.Instance.towerDataListSO.GetSpawnRate(towerType, towerLevel + 1);

        switch(towerType)
        {
            case string t when  t == TowerType.ArcherTower.ToString() ||
                                t == TowerType.MageTower.ToString() ||
                                t == TowerType.CannonTower.ToString():
                string bullet = TowerDataReader.Instance.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel);
                CurentTowerDamage = BulletDataReader.Instance.bulletDataListSO.GetBulletDamage(bullet);
                if(towerLevel + 1 > 2) return;
                string bulletUpgrade = TowerDataReader.Instance.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel + 1);
                TowerDamageUpgrade = BulletDataReader.Instance.bulletDataListSO.GetBulletDamage(bulletUpgrade);
                break;

            case string t when t == TowerType.Barrack.ToString():
                string soldier = TowerDataReader.Instance.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel);
                CurentTowerDamage = UnitDataReader.Instance.unitDataListSO.GetUnitDamage(soldier);
                if(towerLevel + 1 > 2) return;
                string soldierUpgrade = TowerDataReader.Instance.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel + 1);
                TowerDamageUpgrade = UnitDataReader.Instance.unitDataListSO.GetUnitDamage(soldierUpgrade);
                break;
        }
    }

    public void AddGoldRefund(int gold)
    {
        GoldRefund += gold;
    }

    private void AddGoldInitRefund()
    {
        GoldRefund += towerModel.GoldRequired;
    }
}
