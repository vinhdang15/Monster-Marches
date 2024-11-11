using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int Gold { get; private set; } = 200;
    [HideInInspector] public int archerTOwerInitGold;
    [HideInInspector] public int mageTowerInitGold;
    [HideInInspector] public int barrackTowerInitGold;
    [HideInInspector] public int cannonTowerInitGold;
    [HideInInspector] public int towerUpgradeGold;
    [HideInInspector] public int towerSellGold;
    [SerializeField] CSVTowerDataReader TowerDataReader;
    [SerializeField] EmptyPlotManager   emptyPlotManager;
    [SerializeField] TowerManager       towerManager;
    [SerializeField] InputManager       inputManager;
    private Vector2                     SelectedEmptyPlotPos;
    private TowerPresenter selectedTower;
    public delegate void TowerManagerHandler();
    public delegate void GoldChangeHandler();
    public delegate void SelectedTowerHandler();
    public event SelectedTowerHandler OnSelectedTower;
    public event GoldChangeHandler OnGoldChange;
    public bool IsDataLoaded { get; private set; }

    private void Start()
    {
        RegisterButtonEvent();
        RegisterTowerSelectionEvent();
        StartCoroutine(WaitForDataLoadAnhProcess());
    }

    private void GetInitGold()
    {
        archerTOwerInitGold = TowerDataReader.towerDataList.GetGoldInit(TowerType.ArcherTower.ToString());
        mageTowerInitGold = TowerDataReader.towerDataList.GetGoldInit(TowerType.MageTower.ToString());
        barrackTowerInitGold = TowerDataReader.towerDataList.GetGoldInit(TowerType.BarrackTower.ToString());
        cannonTowerInitGold = TowerDataReader.towerDataList.GetGoldInit(TowerType.CannonTower.ToString());
    }

    #region REGISTER EMPTYPLOT CLICK EVENT, TOWER BUTTON CLICK EVENT, SELECTED TOWER EVENT
    // EMPTYPLOT CLICK EVENT
    private IEnumerator WaitForDataLoadAnhProcess()
    {
        yield return new WaitUntil(() => TowerDataReader.IsDataLoaded && emptyPlotManager.isInitEmptyPlot);
        IsDataLoaded = true;
        RegisterSelectedEmptyPlotEvent();
        GetInitGold();
    }

    private void RegisterSelectedEmptyPlotEvent()
    {
        foreach( var emptyPlot in emptyPlotManager.emptyPlotList)
        {
            emptyPlot.OnSelectedEmptyPlot += HandleSelectedEmptyPlot;
        }
    }

    // BUTTON CLICK EVENT
    private void RegisterButtonEvent()
    {
        inputManager.OnInitArcherTower     += HandleInitArcherTower;
        inputManager.OnInitMageTower       += HandleInitMageTower;
        inputManager.OnInitBarrackTower    += HandleInitBarrackTower;
        inputManager.OnInitCannonTower     += HandleInitCannonTower;
        inputManager.OnTryToUpgradeTower   += HandleTryToUpgradeSelectedTower;
        inputManager.OnUpgradeTower        += HandleUpgradeSelectedTower;
        inputManager.OnSellTower           += HandleSellSelectedTower;
        inputManager.OnRaycastHitNull      += HandleRaycatHitNull;
    }

    // Tower Selection Event
    private void RegisterTowerSelectionEvent()
    {
        towerManager.OnSelectedTowerPersenter += HandleSelectedTowerPersenter;
    }
    #endregion

    #region INIT TOWER
    private void OnInitTower(int goldRequired, TowerManagerHandler towerManagerAction)
    {
        if(Gold < goldRequired) return;
        towerManagerAction();
        Gold -= goldRequired;
        OnGoldChange?.Invoke();
        inputManager.HideInitPanel();
    }
    
    // Init tower
    private void HandleInitArcherTower()
    {
        OnInitTower(archerTOwerInitGold, () => towerManager.InitArcherTower(SelectedEmptyPlotPos));
    }

    private void HandleInitMageTower()
    {
        OnInitTower(mageTowerInitGold, () => towerManager.InitMageTower(SelectedEmptyPlotPos));
    }

    private void HandleInitBarrackTower()
    {
        OnInitTower(barrackTowerInitGold, () => towerManager.InitBarackTower(SelectedEmptyPlotPos));
    }

    private void HandleInitCannonTower()
    {
        OnInitTower(cannonTowerInitGold, () => towerManager.InitCannonTower(SelectedEmptyPlotPos));
    }

    // Upgrade selected tower
    private void HandleTryToUpgradeSelectedTower()
    {
        towerManager.UpdateRangeDetectionUpgrade(selectedTower);
        selectedTower.towerView.ShowRangeDetectionUpgrade(true);
    }  
    #endregion

    private void HandleSelectedEmptyPlot(EmptyPlot emptyPlot)
    {
        HideCurrentTowerRangeDetect();
        towerManager.selectedEmptyPlot = emptyPlot;
        SelectedEmptyPlotPos = emptyPlot.GetPos();
        inputManager.ShowInitPanel(SelectedEmptyPlotPos);
    }

    private void HandleSelectedTowerPersenter(TowerPresenter selectedTowerPresenter)
    {   
        HideCurrentTowerRangeDetect();
        selectedTower = selectedTowerPresenter;
        OnSelectedTower?.Invoke();
        Vector2 SelectedTowerPos = selectedTowerPresenter.towerView.GetPos();
        selectedTower.towerView.ShowRangeDetection(true);
        inputManager.ShowUpgradePanel(SelectedTowerPos);
    }

    private void HandleRaycatHitNull()
    {
        if(selectedTower == null) return;
        selectedTower.towerView.ShowRangeDetection(false);
        selectedTower.towerView.ShowRangeDetectionUpgrade(false);
        selectedTower = null;
    }

    private void HandleUpgradeSelectedTower()
    {
        if(Gold < towerManager.towerExtraData[selectedTower].GoldUpdrade) return;

        int goldUpdrade = towerManager.towerExtraData[selectedTower].GoldUpdrade;
        // process gold
        Gold -= goldUpdrade;
        towerManager.towerExtraData[selectedTower].GoldRefund += goldUpdrade;
        OnGoldChange?.Invoke();
        
        // Update tower model
        string towerType = selectedTower.towerModel.TowerType;
        int towerLevel = selectedTower.towerModel.Level;
        TowerData towerData = TowerDataReader.towerDataList.GetTowerData(towerType, towerLevel + 1);
        selectedTower.towerModel.UpgradeTowerModel(towerData);

        // Update next upgrade gold, next range detection data
        towerManager.UpdateTowerExtraData(selectedTower);

        // Update range detection, range detection upgrade
        towerManager.UpdateRangeDetection(selectedTower);
        towerManager.UpdateRangeDetectionUpgrade(selectedTower);

        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
        inputManager.HideUpgradePanel();
    }

    // Sell selected tower
    private void HandleSellSelectedTower()
    {
        Gold += towerManager.towerExtraData[selectedTower].GoldRefund;
        towerManager.towerExtraData[selectedTower].emptyPlot.ShowEmptyPlot();
        Destroy(selectedTower.gameObject);
        OnGoldChange?.Invoke();
        inputManager.HideUpgradePanel();
    }
    
    public int GetTowerGoldUpgrade()
    {
        return towerManager.towerExtraData[selectedTower].GoldUpdrade;
    }

    public int GetTowerGoldRefund()
    {
        return towerManager.towerExtraData[selectedTower].GoldRefund;
    }

    private void HideCurrentTowerRangeDetect()
    {
        if(selectedTower != null)
        {
            Debug.Log(selectedTower.name);
            selectedTower.towerView.ShowRangeDetection(false);
            selectedTower.towerView.ShowRangeDetectionUpgrade(false);
        }
    }
}
