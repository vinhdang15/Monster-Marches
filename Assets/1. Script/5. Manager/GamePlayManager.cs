using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int gold = 200;
    [HideInInspector] public int archerTOwerInitGold;
    [HideInInspector] public int mageTowerInitGold;
    [HideInInspector] public int barrackTowerInitGold;
    [HideInInspector] public int cannonTowerInitGold;
    [HideInInspector] public int towerUpgradeGold;
    [HideInInspector] public int towerSellGold;
    [SerializeField] CSVTowerDataReader towerDataReader;
    [SerializeField] EmptyPlotManager   emptyPlotManager;
    [SerializeField] TowerManager       towerManager;
    [SerializeField] InputManager       inputManager;
    private Vector2                     SelectedEmptyPlotPos;
    private TowerPresenter selectedTower;
    public delegate void TowerManagerHandler();
    public event Action OnSelectedTower;
    public event Action OnGoldChange;
    public bool IsDataLoaded { get; private set; }

    public SpawnEnemyManager spawnEnemyManager;

    private void Start()
    {
        RegisterButtonEvent();
        RegisterTowerSelectionEvent();
        RegisterCautionClickEvent();
        StartCoroutine(WaitForDataLoadAndProcess());
    }

    private void GetInitGold()
    {
        archerTOwerInitGold = towerDataReader.towerDataList.GetGoldInit(TowerType.ArcherTower.ToString());
        mageTowerInitGold = towerDataReader.towerDataList.GetGoldInit(TowerType.MageTower.ToString());
        barrackTowerInitGold = towerDataReader.towerDataList.GetGoldInit(TowerType.BarrackTower.ToString());
        cannonTowerInitGold = towerDataReader.towerDataList.GetGoldInit(TowerType.CannonTower.ToString());
    }

    #region REGISTER EMPTYPLOT CLICK EVENT, TOWER BUTTON CLICK EVENT, SELECTED TOWER EVENT
    // EMPTYPLOT CLICK EVENT
    private IEnumerator WaitForDataLoadAndProcess()
    {
        yield return new WaitUntil(() => towerDataReader.IsDataLoaded && emptyPlotManager.isInitEmptyPlot);
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

    // caution click event
    private void RegisterCautionClickEvent()
    {
        spawnEnemyManager.OnCautionClick += HandleCautionClick;
    }

    private void HandleCautionClick(float time)
    {
        gold += (int)time;
        OnGoldChange?.Invoke();
        Debug.Log(time);
    }

    #region INIT TOWER
    private void OnInitTower(int goldRequired, TowerManagerHandler towerManagerAction)
    {
        if(gold < goldRequired) return;
        towerManagerAction();
        gold -= goldRequired;
        OnGoldChange?.Invoke();
        //towerManager.towerExtraData[selectedTower].GoldRefund += goldRequired;
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
        if(gold < towerManager.towerExtraData[selectedTower].GoldUpdrade) return;
        // process gold
        int goldUpdrade = towerManager.towerExtraData[selectedTower].GoldUpdrade;
        gold -= goldUpdrade;
        OnGoldChange?.Invoke();
        towerManager.UpgradeTower(selectedTower);
        towerManager.towerExtraData[selectedTower].GoldRefund += goldUpdrade;
        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
        inputManager.HideUpgradePanel();
    }

    // Sell selected tower
    private void HandleSellSelectedTower()
    {
        gold += towerManager.towerExtraData[selectedTower].GoldRefund;
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
