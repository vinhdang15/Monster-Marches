using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPresenter : MonoBehaviour
{
    public TowerModel        towerModel;
    public TowerView         towerView;
    public CSVTowerDataReader   towerDataReader;
    public EmptyPlot            emptyPlot;
    public float                RangeDetectUpgrade { set ; get ; }
    public int                  GoldUpdrade { set ; get ; }
    public int                  GoldRefund { set ; get ; }

    public static TowerPresenter Create(TowerModel towerModel, TowerView towerView)
    {
        TowerPresenter towerPresenter = towerView.gameObject.AddComponent<TowerPresenter>();
        towerPresenter.TowerPresenterInit(towerModel, towerView);
        return towerPresenter;
    } 
    
    public void TowerPresenterInit(TowerModel towerModel, TowerView towerView)
    {
        this.towerModel = towerModel;
        this.towerView  = towerView;
        AddGoldInitRefund();
        InitTowerRange();
    }

    public void InitTowerRange()
    {
        towerView.SetRangeRaycat(towerModel.RangeRaycast);
        towerView.SetRangeDetect(towerModel.RangeDetect);
    }

    // call thí in BuildingManager after buildingPresenter get buildingData
    public void UpdateGoldUpgrade()
    {
        string towerType = towerModel.TowerType;
        int towerLevel = towerModel.Level;
        GoldUpdrade = towerDataReader.towerDataList.GetGoldRequired(towerType, towerLevel + 1);
    }

    public void UpdateRangeDetectUpgradeData()
    {
        string towerType = towerModel.TowerType;
        int towerLevel = towerModel.Level;
        RangeDetectUpgrade = towerDataReader.towerDataList.GetRangeDetect(towerType, towerLevel + 1);
    }

    public void UpdateBuildingData()
    {
        UpdateGoldUpgrade();
        UpdateRangeDetectUpgradeData();
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
