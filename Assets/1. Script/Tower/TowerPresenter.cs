using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TowerPresenter : MonoBehaviour
{
    public TowerModel               towerModel;
    public TowerViewBase            towerViewBase;
    public SelectTargetEnemyHandler    getTargetEnemyHandler;
    private SpriteLibraryHandler    spriteLibraryHandler;
    public EmptyPlot                emptyPlot;
    public int               TowerMaxLevel { set ; get ; }
    public int               CurrentTowerDamage { set ; get ; }
    public string            DescriptionUpgrade { set ; get ; }
    public int               TowerDamageUpgrade { set ; get ; }
    public float             TowerSpawnRateUpgrade { set ; get ; }
    public float             RangeDetectUpgrade { set ; get ; }
    public int               GoldUpgrade { set ; get ; }
    public int               GoldRefund { set ; get ; }
    private TowerDataReader     towerDataReader;
    private BulletDataReader    bulletDataReader;
    private UnitDataReader      unitDataReader;

    public static TowerPresenter CreateTowerPresenterComponent(TowerModel towerModel, TowerViewBase towerView,
                                                                TowerDataReader towerDataReader, BulletDataReader bulletDataReader,
                                                                UnitDataReader unitDataReader)
    {
        TowerPresenter towerPresenter = towerView.gameObject.AddComponent<TowerPresenter>();
        towerPresenter.TowerPresenterInit(towerModel, towerView, towerDataReader, bulletDataReader, unitDataReader);
        return towerPresenter;
    }

    public void TowerPresenterInit(TowerModel towerModel, TowerViewBase towerView,
                                    TowerDataReader towerDataReader, BulletDataReader bulletDataReader,
                                    UnitDataReader unitDataReader)
    {
        this.towerModel = towerModel;
        towerViewBase = towerView;
        this.towerDataReader = towerDataReader;
        this.bulletDataReader = bulletDataReader;
        this.unitDataReader = unitDataReader;
        InitTowerRange();
        GetTowerMaxLevel();
        AddGoldInitRefund();
        SetTowerPresenterData();
        GetSpriteLibraryHandler();
        GetGetTargetEnemyHandler();
    }

    private void GetSpriteLibraryHandler()
    {
        spriteLibraryHandler = GetComponent<SpriteLibraryHandler>();
    }

    private void GetGetTargetEnemyHandler()
    {
        getTargetEnemyHandler = gameObject.AddComponent<SelectTargetEnemyHandler>();
        
    }

    public void UpdateTowerSprite(int level)
    {
        spriteLibraryHandler.UpdateSprite(level);
    }

    private void GetTowerMaxLevel()
    {
        TowerMaxLevel = towerDataReader.towerDataListSO.GetTowerMaxLevel(towerModel.TowerType);
    }

    public bool IsTowerMaxLevel()
    {
        return towerModel.Level == TowerMaxLevel;
    }

    private void InitTowerRange()
    {
        towerViewBase.SetRangeRaycat(towerModel.RangeRaycast);
        towerViewBase.SetRangeDetect(towerModel.RangeDetect);
    }

    public void SetTowerPresenterData()
    {
        string towerType    = towerModel.TowerType;
        int towerLevel      = towerModel.Level;

        DescriptionUpgrade      = towerDataReader.towerDataListSO.GetDescription(towerType, towerLevel + 1);
        GoldUpgrade             = towerDataReader.towerDataListSO.GetGoldRequired(towerType, towerLevel + 1);
        RangeDetectUpgrade      = towerDataReader.towerDataListSO.GetRangeDetect(towerType, towerLevel + 1);
        TowerSpawnRateUpgrade   = towerDataReader.towerDataListSO.GetSpawnRate(towerType, towerLevel + 1);

        switch(towerType)
        {
            case string t when  t == TowerType.ArcherTower.ToString() ||
                                t == TowerType.MageTower.ToString() ||
                                t == TowerType.CannonTower.ToString():
                string bullet = towerDataReader.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel);
                CurrentTowerDamage = bulletDataReader.bulletDataListSO.GetBulletDamage(bullet);
                if(towerLevel >= TowerMaxLevel) return;
                string bulletUpgrade = towerDataReader.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel + 1);
                TowerDamageUpgrade = bulletDataReader.bulletDataListSO.GetBulletDamage(bulletUpgrade);
                break;

            case string t when t == TowerType.Barrack.ToString():
                string soldier = towerDataReader.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel);
                CurrentTowerDamage = unitDataReader.unitDataListSO.GetUnitDamage(soldier);
                if(towerLevel >= TowerMaxLevel) return;
                string soldierUpgrade = towerDataReader.towerDataListSO.GetTowerSpawnObject(towerType, towerLevel + 1);
                TowerDamageUpgrade = unitDataReader.unitDataListSO.GetUnitDamage(soldierUpgrade);
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
