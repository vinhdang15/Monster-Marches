using System.Collections.Generic;
using UnityEngine;

public class TowerBaseManager : MonoBehaviour
{
    protected TowerDataReader towerDataReader;

    #region INIT BUILDING
    protected TowerPresenter InitBuildingPresenter(TowerViewBase towerPrefab, TowerData towerData, Vector3 pos,
                                                    TowerDataReader towerDataReader, BulletDataReader bulletDataReader = null,
                                                    UnitDataReader unitDataReader = null)
    {
        TowerViewBase buildingView = Instantiate(towerPrefab, pos, Quaternion.identity, transform);
        TowerModel buildingModel = TowerModel.Craete(buildingView, towerData);
        TowerPresenter towerPresenter = TowerPresenter.CreateTowerPresenterComponent(buildingModel, buildingView,
                                                                                            towerDataReader, bulletDataReader,
                                                                                            unitDataReader);
        return towerPresenter;
    }

    protected void AddTowerPersenterEmptyPlot(TowerPresenter towerPresenter, EmptyPlot emptyPlot)
    {
        towerPresenter.emptyPlot = emptyPlot;
        // towerPresenter.emptyPlot.HideEmptyPlot();
    }

    #endregion

    #region UPGRADE BUILDING
    public virtual void UpgradeTower(TowerPresenter towerPresenter)
    {
        string towerType = towerPresenter.towerModel.TowerType;
        int nextLevel = towerPresenter.towerModel.Level + 1;

        //Update tower sprite
        towerPresenter.UpdateTowerSprite(nextLevel);

        // upgrade tower model
        TowerData towerData = towerDataReader.towerDataListSO.GetTowerData(towerType, nextLevel);
        towerPresenter.towerModel.UpgradeTowerModel(towerData);

        // upgrade range, range upgrade
        UpdateRangeDetection(towerPresenter);
        UpdateRangeDetectionUpgrade(towerPresenter);

        // Upgrade TowerPresenter data after upgrade tower
        towerPresenter.SetTowerPresenterData();
    }

    private void UpdateRangeDetection(TowerPresenter buildingPresenter)
    {
        float rangeDetect = buildingPresenter.towerModel.RangeDetect;
        buildingPresenter.towerViewBase.SetRangeDetect(rangeDetect);
    }

    public void UpdateRangeDetectionUpgrade(TowerPresenter buildingPresenter)
    {
        float rangeDetectUpgrade = buildingPresenter.RangeDetectUpgrade;
        buildingPresenter.towerViewBase.SetRangeDetectUpgrade(rangeDetectUpgrade);
    }
    #endregion
}