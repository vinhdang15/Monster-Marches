using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int gold = 200;
    public int live = 20;
    [HideInInspector] public int archerTowerInitGold;
    [HideInInspector] public int mageTowerInitGold;
    [HideInInspector] public int barrackTowerInitGold;
    [HideInInspector] public int cannonTowerInitGold;
    [HideInInspector] public int towerUpgradeGold;
    [HideInInspector] public int towerSellGold;
    [SerializeField] EnemyManager               enemyManager;
    [SerializeField] PanelManager               panelManager;

    [SerializeField] RaycastHandler             raycastHandler;
    [SerializeField] TowerActionHandler         towerActionHandler;

    [SerializeField] BulletTowerManager         bulletTowerManager;
    [SerializeField] BarrackTowerManager        barrackTowerManager;
    public SpawnEnemyManager spawnEnemyManager;
    private Vector2                             initMenuPanelPos;
    private TowerPresenter                      selectedTower;
    public event Action OnSelectedTowerForUI;
    public event Action OnGoldChangeForUI;
    public event Action OnLiveChangeForUI;
    
    public EmptyPlot currentEmptyPlot;

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;

    public void GamePlayManagerPrepareGame()
    {
        LoadComponents();
        RegisterEnemyEvent();
        RegisterButtonEvent();
        RegisterCautionClickEvent();
        GetInitGold();
    }

    private void OnDisable()
    {
        UnregisterEnemyEvent();
        UnregisterButtonEvent();
        UnregisterCautionClickEvent();
    }

    private void LoadComponents()
    {
        panelManager        = FindObjectOfType<PanelManager>();
        enemyManager        = FindObjectOfType<EnemyManager>();

        // inputController     = FindObjectOfType<InputControllerxxx>();
        raycastHandler      = FindObjectOfType<RaycastHandler>();
        towerActionHandler  = FindObjectOfType<TowerActionHandler>();

        spawnEnemyManager   = FindObjectOfType<SpawnEnemyManager>();
        bulletTowerManager  = FindObjectOfType<BulletTowerManager>();
        barrackTowerManager = FindObjectOfType<BarrackTowerManager>();
    }

    private void GetInitGold()
    {
        archerTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.ArcherTower.ToString().Trim().ToLower());
        mageTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.MageTower.ToString().Trim().ToLower());
        barrackTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.Barrack.ToString().Trim().ToLower());
        cannonTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.CannonTower.ToString().Trim().ToLower());
    }

    #region REGISTER EMPTYPLOT CLICK EVENT, TOWER BUTTON CLICK EVENT, SELECTED TOWER EVENT
    // BUTTON CLICK EVENT
    private void RegisterButtonEvent()
    {
        raycastHandler.OnRaycastHitNull             += HandleRaycatHitNull;
        raycastHandler.OnSelectedEmptyPlot          += HandleSelectedEmptyPlot;
        raycastHandler.OnSelectedBulletTower        += HandleOnSelectedBulletTower;
        raycastHandler.OnSelectedBarrackTower       += HandleOnSelectedBarrackTower;
        raycastHandler.OnSelectedNewGuardPointPos   += HandleOnSelectedNewGuardPoint;

        towerActionHandler.OnInitTower              += HandleInitTower;
        towerActionHandler.OnTryToUpgradeTower      += HandleTryToUpgradeSelectedTower;
        towerActionHandler.OnUpgradeTower           += HandleUpgradeSelectedTower;
        towerActionHandler.OnGuardPointBtnClick     += HandleGuardPointBtnClick;
        towerActionHandler.OnSellTower              += HandleSellSelectedTower;
    }

    private void UnregisterButtonEvent()
    {
        raycastHandler.OnRaycastHitNull             -= HandleRaycatHitNull;
        raycastHandler.OnSelectedEmptyPlot          -= HandleSelectedEmptyPlot;
        raycastHandler.OnSelectedBulletTower        -= HandleOnSelectedBulletTower;
        raycastHandler.OnSelectedBarrackTower       -= HandleOnSelectedBarrackTower;
        raycastHandler.OnSelectedNewGuardPointPos   -= HandleOnSelectedNewGuardPoint;

        towerActionHandler.OnInitTower              -= HandleInitTower;
        towerActionHandler.OnTryToUpgradeTower      -= HandleTryToUpgradeSelectedTower;
        towerActionHandler.OnUpgradeTower           -= HandleUpgradeSelectedTower;
        towerActionHandler.OnGuardPointBtnClick     -= HandleGuardPointBtnClick;
        towerActionHandler.OnSellTower              -= HandleSellSelectedTower;
    }

    private void RegisterEnemyEvent()
    {
        enemyManager.OnEnemyDeath += HandleEnemyDeath;
        enemyManager.OnEnemyReachEndPoint += HandleEnemyReachEndPoint;
    }

    private void UnregisterEnemyEvent()
    {
        enemyManager.OnEnemyDeath -= HandleEnemyDeath;
        enemyManager.OnEnemyReachEndPoint -= HandleEnemyReachEndPoint;
    }

    private void HandleEnemyDeath(UnitBase enemy)
    {
        gold += enemy.Gold;
        OnGoldChangeForUI?.Invoke();
        CheckAllEnemiesDie();
        // Debug.Log(spawnEnemyManager.totalEnemies + "    " + enemyManager.totalEnemiesDie);
    }

    private void HandleEnemyReachEndPoint()
    {
        live --;
        OnLiveChangeForUI?.Invoke();
        CheckAllEnemiesDie();
        // Debug.Log(spawnEnemyManager.totalEnemies + "    " + enemyManager.totalEnemiesDie);
    }

    private void CheckAllEnemiesDie()
    {
        if(spawnEnemyManager.totalEnemies == enemyManager.totalEnemiesDie)
        {
            // Debug.Log(spawnEnemyManager.totalEnemies + "    " + enemyManager.totalEnemiesDie);
            Invoke(nameof(ShowVictoryMenu), 2.5f);
        }
    }

    private void ShowVictoryMenu()
    {
        PanelManager.Instance.ShowVictoryMenu();
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
    private void OnInitTower(int goldRequired, Action action)
    {
        if(gold < goldRequired) return;
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);
        action?.Invoke();
        gold -= goldRequired;
        OnGoldChangeForUI?.Invoke();
        panelManager.HandleRaycastHitNull();
    }
    
    // Init tower
    private void HandleInitTower(TowerType  towerType)
    {
        switch(towerType)
        {
            case TowerType.ArcherTower:
                InitArcherTower();
                break;
            case TowerType.MageTower:
                InitMageTower();
                break;
            case TowerType.Barrack:
                InitBarrackTower();
                break;
            case TowerType.CannonTower:
                InitCannonTower();
                break;
        }
    }
    private void InitArcherTower()
    {
        OnInitTower(archerTowerInitGold, () => bulletTowerManager.InitArcherTower(initMenuPanelPos, currentEmptyPlot));
    }

    private void InitMageTower()
    {
        OnInitTower(mageTowerInitGold, () => bulletTowerManager.InitMageTower(initMenuPanelPos, currentEmptyPlot));
    }

    private void InitBarrackTower()
    {
        OnInitTower(barrackTowerInitGold, () => barrackTowerManager.InitBarack(initMenuPanelPos, currentEmptyPlot));
    }

    private void InitCannonTower()
    {
        OnInitTower(cannonTowerInitGold, () => bulletTowerManager.InitCannonTower(initMenuPanelPos, currentEmptyPlot));
    }

    // Upgrade selected tower
    private void HandleTryToUpgradeSelectedTower(TowerPresenter towerPresenter)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        bulletTowerManager.UpdateRangeDetectionUpgrade(selectedTower);
        selectedTower.towerViewBase.ShowRangeDetectionUpgrade(true);
    }

    private void HandleUpgradeSelectedTower()
    {
        if(gold < selectedTower.GoldUpdrade) return;
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);
        // process gold
        int goldUpdrade = selectedTower.GoldUpdrade;
        gold -= goldUpdrade;
        OnGoldChangeForUI?.Invoke();

        TowerBaseManager towerBaseManager = bulletTowerManager;
        towerBaseManager.UpgradeBuilding(selectedTower);

        selectedTower.GoldRefund += goldUpdrade;
        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
        // inputController.ButtonDoubleClickAction();
        panelManager.HandleRaycastHitNull();
    }

    // Sell selected tower
    private void HandleSellSelectedTower()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.AddGoldSound);
        gold += selectedTower.GoldRefund;
        selectedTower.emptyPlot.ShowEmptyPlot();
        Destroy(selectedTower.gameObject);
        OnGoldChangeForUI?.Invoke();
        panelManager.HandleRaycastHitNull();
    }
    #endregion

    private void HandleSelectedEmptyPlot(EmptyPlot emptyPlot)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        currentEmptyPlot = emptyPlot;
        initMenuPanelPos = emptyPlot.GetPos();
    }

    private void HandleOnSelectedBulletTower(TowerPresenter selectedTowerPresenter)
    {  
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        selectedTower = selectedTowerPresenter;
        OnSelectedTowerForUI?.Invoke();
        selectedTower.towerViewBase.ShowRangeDetection(true);
    }

    private void HandleOnSelectedBarrackTower(TowerPresenter selectedTowerPresenter)
    {  
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        selectedTower = selectedTowerPresenter;
        OnSelectedTowerForUI?.Invoke();
    }

    private void HandleGuardPointBtnClick()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        if(selectedTower != null)
        {
            selectedTower.towerViewBase.ShowRangeDetection(true);
        }
    }

    // selected new guard point
    private void HandleOnSelectedNewGuardPoint(Vector2 newGuardPointPos)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        selectedTower.towerViewBase.ShowRangeDetection(false);
        barrackTowerManager.SetNewGuardPointPos(selectedTower, newGuardPointPos);
    }

    // raycast hit null
    private void HandleRaycatHitNull()
    {
        if(selectedTower == null) return;
        selectedTower.towerViewBase.ShowRangeDetection(false);
        selectedTower.towerViewBase.ShowRangeDetectionUpgrade(false);
        selectedTower = null;
    }
    
    public int GetTowerGoldUpgrade()
    {
        return selectedTower.GoldUpdrade;
    }

    public int GetTowerGoldRefund()
    {
        return selectedTower.GoldRefund;
    }

    private void HideCurrentTowerRangeDetect()
    {
        if(selectedTower != null)
        {
            selectedTower.towerViewBase.ShowRangeDetection(false);
            selectedTower.towerViewBase.ShowRangeDetectionUpgrade(false);
        }
    }
}
