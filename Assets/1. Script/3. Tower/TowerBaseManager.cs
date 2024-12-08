using System.Collections.Generic;
using UnityEngine;

public class TowerBaseManager : MonoBehaviour
{
    [SerializeField] protected CSVTowerDataReader     towerDataReader;

    #region INIT BUILDING
    protected TowerPresenter InitBuildingPresenter(TowerView towerPrefab, TowerData towerData, Vector3 pos)
    {
        TowerView buildingView             = Instantiate(towerPrefab, pos, Quaternion.identity, transform);
        TowerModel buildingModel           = TowerModel.Craete(buildingView,towerData);
        TowerPresenter buildingPresenter   = TowerPresenter.Create(buildingModel, buildingView);
        buildingPresenter.towerDataReader = towerDataReader;
        buildingPresenter.UpdateRangeDetectUpgradeData();
        return buildingPresenter;
    }

    protected void AddTowerPersenterEmptyPlot(TowerPresenter towerPresenter, EmptyPlot emptyPlot)
    {
        towerPresenter.emptyPlot = emptyPlot;
        towerPresenter.emptyPlot.HideEmptyPlot();
    }

    #endregion

    #region UPGRADE BUILDING
    public void UpgradeBuilding(TowerPresenter buildingPresenter)
    {
        // upgrade tower mode
        string buildingType = buildingPresenter.towerModel.TowerType;
        int buildingLevel = buildingPresenter.towerModel.Level;
        TowerData towerData = towerDataReader.towerDataList.GetTowerData(buildingType, buildingLevel + 1);
        buildingPresenter.towerModel.UpgradeTowerModel(towerData);

        // upgrade range, range upgrade
        UpdateRangeDetection(buildingPresenter);
        UpdateRangeDetectionUpgrade(buildingPresenter);

        // Upgrade TowerPresenter Extra data
        buildingPresenter.UpdateBuildingData();
    }

    private void UpdateRangeDetection(TowerPresenter buildingPresenter)
    {
        float rangeDetect = buildingPresenter.towerModel.RangeDetect;
        buildingPresenter.towerView.SetRangeDetect(rangeDetect);
    }

    public void UpdateRangeDetectionUpgrade(TowerPresenter buildingPresenter)
    {
        float rangeDetectUpgrade = buildingPresenter.RangeDetectUpgrade;
        buildingPresenter.towerView.SetRangeDetectUpgrade(rangeDetectUpgrade);
    }
    #endregion
}