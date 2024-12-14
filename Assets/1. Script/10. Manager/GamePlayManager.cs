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
    [SerializeField] BulletTowerManager       bulletTowerManager;
    [SerializeField] BarrackTowerManager     barrackTowerManager;
    [SerializeField] EnemyManager       enemyManager;
    [SerializeField] InputManager       inputManager;
    private Vector2                     initPanelPos;
    private TowerPresenter selectedBuilding;
    public delegate void TowerManagerHandler();
    public event Action OnSelectedTowerForUI;
    public event Action OnGoldChangeForUI;
    public bool IsDataLoaded { get; private set; }

    public SpawnEnemyManager spawnEnemyManager;
    public EmptyPlot currentEmptyPlot;

    private void Start()
    {
        RegisterEnemyEvent();
        RegisterButtonEvent();
        RegisterCautionClickEvent();
        StartCoroutine(WaitForDataLoadAndProcess());
    }

    private void OnDisable()
    {
        UnregisterEnemyEvent();
        UnregisterButtonEvent();
        UnregisterCautionClickEvent();
    }

    private void GetInitGold()
    {
        archerTOwerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.ArcherTower.ToString().Trim().ToLower());
        mageTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.MageTower.ToString().Trim().ToLower());
        barrackTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(BarrackType.Barrack.ToString().Trim().ToLower());
        cannonTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.CannonTower.ToString().Trim().ToLower());
    }

    #region REGISTER EMPTYPLOT CLICK EVENT, TOWER BUTTON CLICK EVENT, SELECTED TOWER EVENT
    // EMPTYPLOT CLICK EVENT
    private IEnumerator WaitForDataLoadAndProcess()
    {
        yield return new WaitUntil(() => CSVTowerDataReader.Instance.IsDataLoaded);
        IsDataLoaded = true;
        GetInitGold();
    }

    // BUTTON CLICK EVENT
    private void RegisterButtonEvent()
    {
        inputManager.OnInitArcherTower              += HandleInitArcherTower;
        inputManager.OnInitMageTower                += HandleInitMageTower;
        inputManager.OnInitBarrackTower             += HandleInitBarrackTower;
        inputManager.OnInitCannonTower              += HandleInitCannonTower;
        inputManager.OnTryToUpgradeTower            += HandleTryToUpgradeSelectedTower;
        inputManager.OnUpgradeTower                 += HandleUpgradeSelectedTower;
        inputManager.OnSellTower                    += HandleSellSelectedTower;
        inputManager.OnSelectedEmptyPlot            += HandleSelectedEmptyPlot;
        inputManager.OnSelectedBulletTower          += HandleOnSelectedBulletTower;
        inputManager.OnSelectedBarrackTower         += HandleOnSelectedBarrackTower;
        inputManager.OnSelectedGuardPointBtnClick   += HandleOnSelectedGuardPointBtnClick;
        inputManager.OnSelectedNewGuardPointPos     += HandleOnSelectedNewGuardPoint;
        inputManager.OnRaycastHitNull               += HandleRaycatHitNull;
    }

    private void UnregisterButtonEvent()
    {
        inputManager.OnInitArcherTower              -= HandleInitArcherTower;
        inputManager.OnInitMageTower                -= HandleInitMageTower;
        inputManager.OnInitBarrackTower             -= HandleInitBarrackTower;
        inputManager.OnInitCannonTower              -= HandleInitCannonTower;
        inputManager.OnTryToUpgradeTower            -= HandleTryToUpgradeSelectedTower;
        inputManager.OnUpgradeTower                 -= HandleUpgradeSelectedTower;
        inputManager.OnSellTower                    -= HandleSellSelectedTower;
        inputManager.OnSelectedEmptyPlot            -= HandleSelectedEmptyPlot;
        inputManager.OnSelectedBulletTower          -= HandleOnSelectedBulletTower;
        inputManager.OnSelectedBarrackTower         -= HandleOnSelectedBarrackTower;
        inputManager.OnSelectedGuardPointBtnClick   -= HandleOnSelectedGuardPointBtnClick;
        inputManager.OnSelectedNewGuardPointPos     -= HandleOnSelectedNewGuardPoint;
        inputManager.OnRaycastHitNull               -= HandleRaycatHitNull;
    }

    private void RegisterEnemyEvent()
    {
        enemyManager.EnemyDieHandler += HandleEnemyDie;
    }

    private void UnregisterEnemyEvent()
    {
        enemyManager.EnemyDieHandler -= HandleEnemyDie;
    }

    private void HandleEnemyDie(UnitBase enemy)
    {
        gold += enemy.Gold;
        OnGoldChangeForUI?.Invoke();
    }
    #endregion

    // caution click event
    private void RegisterCautionClickEvent()
    {
        spawnEnemyManager.OnCautionClick += HandleCautionClick;
    }

    private void UnregisterCautionClickEvent()
    {
        spawnEnemyManager.OnCautionClick -= HandleCautionClick;
    }

    private void HandleCautionClick(float time)
    {
        gold += (int)time;
        OnGoldChangeForUI?.Invoke();
    }

    #region INIT TOWER
    private void OnInitTower(int goldRequired, TowerManagerHandler towerManagerAction)
    {
        if(gold < goldRequired) return;
        towerManagerAction();
        gold -= goldRequired;
        OnGoldChangeForUI?.Invoke();
        inputManager.HideInitPanel();
    }
    
    // Init tower
    private void HandleInitArcherTower()
    {
        OnInitTower(archerTOwerInitGold, () => bulletTowerManager.InitArcherTower(initPanelPos, currentEmptyPlot));
    }

    private void HandleInitMageTower()
    {
        OnInitTower(mageTowerInitGold, () => bulletTowerManager.InitMageTower(initPanelPos, currentEmptyPlot));
    }

    private void HandleInitBarrackTower()
    {
        OnInitTower(barrackTowerInitGold, () => barrackTowerManager.InitBarack(initPanelPos, currentEmptyPlot));
    }

    private void HandleInitCannonTower()
    {
        OnInitTower(cannonTowerInitGold, () => bulletTowerManager.InitCannonTower(initPanelPos, currentEmptyPlot));
    }

    // Upgrade selected tower
    private void HandleTryToUpgradeSelectedTower()
    {
        bulletTowerManager.UpdateRangeDetectionUpgrade(selectedBuilding);
        selectedBuilding.towerView.ShowRangeDetectionUpgrade(true);
    }  
    #endregion

    private void HandleSelectedEmptyPlot(EmptyPlot emptyPlot)
    {
        HideCurrentTowerRangeDetect();
        currentEmptyPlot = emptyPlot;
        initPanelPos = emptyPlot.GetPos();
        // inputManager.ShowInitPanel(initPanelPos);
    }

    private void HandleOnSelectedBulletTower(TowerPresenter selectedTowerPresenter)
    {  
        HideCurrentTowerRangeDetect();
        selectedBuilding = selectedTowerPresenter;
        OnSelectedTowerForUI?.Invoke();
        selectedBuilding.towerView.ShowRangeDetection(true);
        // Vector2 SelectedTowerPos = selectedTowerPresenter.towerView.GetPos();
        // inputManager.ShowUpgradePanel(SelectedTowerPos);
    }
    private void HandleOnSelectedBarrackTower(TowerPresenter selectedTowerPresenter)
    {  
        HideCurrentTowerRangeDetect();
        selectedBuilding = selectedTowerPresenter;
        OnSelectedTowerForUI?.Invoke();
        // selectedBuilding.towerView.ShowRangeDetection(true);
        // Vector2 SelectedTowerPos = selectedTowerPresenter.towerView.GetPos();
        // inputManager.ShowUpgradePanel(SelectedTowerPos);
    }

    private void HandleOnSelectedGuardPointBtnClick()
    {
        if(selectedBuilding != null)
        {
            selectedBuilding.towerView.ShowRangeDetection(true);
        }
    }

    // selected new guard point
    private void HandleOnSelectedNewGuardPoint(Vector2 newGuardPointPos)
    {
        selectedBuilding.towerView.ShowRangeDetection(false);
        barrackTowerManager.SetNewGuardPointPos(selectedBuilding, newGuardPointPos);
    }

    private void HandleUpgradeSelectedTower()
    {
        if(gold < selectedBuilding.GoldUpdrade) return;
        // process gold
        int goldUpdrade = selectedBuilding.GoldUpdrade;
        gold -= goldUpdrade;
        OnGoldChangeForUI?.Invoke();
        bulletTowerManager.UpgradeBuilding(selectedBuilding);
        selectedBuilding.GoldRefund += goldUpdrade;
        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
        inputManager.HideUpgradePanel();
    }

    // raycast hit null
    private void HandleRaycatHitNull()
    {
        if(selectedBuilding == null) return;
        selectedBuilding.towerView.ShowRangeDetection(false);
        selectedBuilding.towerView.ShowRangeDetectionUpgrade(false);
        selectedBuilding = null;
        inputManager.HidePanel();
    }

    // Sell selected tower
    private void HandleSellSelectedTower()
    {
        gold += selectedBuilding.GoldRefund;
        selectedBuilding.emptyPlot.ShowEmptyPlot();
        Destroy(selectedBuilding.gameObject);
        OnGoldChangeForUI?.Invoke();
        inputManager.HideUpgradePanel();
    }
    
    public int GetTowerGoldUpgrade()
    {
        return selectedBuilding.GoldUpdrade;
    }

    public int GetTowerGoldRefund()
    {
        return selectedBuilding.GoldRefund;
    }

    private void HideCurrentTowerRangeDetect()
    {
        if(selectedBuilding != null)
        {
            selectedBuilding.towerView.ShowRangeDetection(false);
            selectedBuilding.towerView.ShowRangeDetectionUpgrade(false);
        }
    }
}
