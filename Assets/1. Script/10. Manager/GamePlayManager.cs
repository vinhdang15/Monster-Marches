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
    [SerializeField] BulletTowerManager         bulletTowerManager;
    [SerializeField] BarrackTowerManager        barrackTowerManager;
    [SerializeField] EnemyManager               enemyManager;
    [SerializeField] InputController            inputManager;
    private Vector2                             initPanelPos;
    private TowerPresenter                      selectedBuilding;
    public event Action OnSelectedTowerForUI;
    public event Action OnGoldChangeForUI;
    public event Action OnLiveChangeForUI;
    public bool IsDataLoaded { get; private set; }

    public SpawnEnemyManager spawnEnemyManager;
    public EmptyPlot currentEmptyPlot;

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;
    
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
        archerTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.ArcherTower.ToString().Trim().ToLower());
        mageTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.MageTower.ToString().Trim().ToLower());
        barrackTowerInitGold = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(TowerType.Barrack.ToString().Trim().ToLower());
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
        inputManager.OnTryToInitTower               += HandleOnTryToInitTower;
        inputManager.OnTowerInit                    += HandleTowerInit;
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
        inputManager.OnTryToInitTower               -= HandleOnTryToInitTower;
        inputManager.OnTowerInit                    -= HandleTowerInit;
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
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);
        if(gold < goldRequired) return;
        action?.Invoke();
        gold -= goldRequired;
        OnGoldChangeForUI?.Invoke();
    }
    
    // Init tower
    private void HandleOnTryToInitTower(TowerType  towerType, EmptyPlot emptyPlot)
    {

    }
    private void HandleTowerInit(TowerType  towerType)
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
        OnInitTower(archerTowerInitGold, () => bulletTowerManager.InitArcherTower(initPanelPos, currentEmptyPlot));
    }

    private void InitMageTower()
    {
        OnInitTower(mageTowerInitGold, () => bulletTowerManager.InitMageTower(initPanelPos, currentEmptyPlot));
    }

    private void InitBarrackTower()
    {
        OnInitTower(barrackTowerInitGold, () => barrackTowerManager.InitBarack(initPanelPos, currentEmptyPlot));
    }

    private void InitCannonTower()
    {
        OnInitTower(cannonTowerInitGold, () => bulletTowerManager.InitCannonTower(initPanelPos, currentEmptyPlot));
    }

    // Upgrade selected tower
    private void HandleTryToUpgradeSelectedTower(TowerPresenter towerPresenter)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        bulletTowerManager.UpdateRangeDetectionUpgrade(selectedBuilding);
        selectedBuilding.towerView.ShowRangeDetectionUpgrade(true);
    }

    private void HandleUpgradeSelectedTower()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);
        if(gold < selectedBuilding.GoldUpdrade) return;
        // process gold
        int goldUpdrade = selectedBuilding.GoldUpdrade;
        gold -= goldUpdrade;
        OnGoldChangeForUI?.Invoke();
        bulletTowerManager.UpgradeBuilding(selectedBuilding);
        selectedBuilding.GoldRefund += goldUpdrade;
        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
    }

    // Sell selected tower
    private void HandleSellSelectedTower()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.AddGoldSound);
        gold += selectedBuilding.GoldRefund;
        selectedBuilding.emptyPlot.ShowEmptyPlot();
        Destroy(selectedBuilding.gameObject);
        OnGoldChangeForUI?.Invoke();
    }
    #endregion

    private void HandleSelectedEmptyPlot(EmptyPlot emptyPlot)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        currentEmptyPlot = emptyPlot;
        initPanelPos = emptyPlot.GetPos();
    }

    private void HandleOnSelectedBulletTower(TowerPresenter selectedTowerPresenter)
    {  
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        selectedBuilding = selectedTowerPresenter;
        OnSelectedTowerForUI?.Invoke();
        selectedBuilding.towerView.ShowRangeDetection(true);;
    }
    private void HandleOnSelectedBarrackTower(TowerPresenter selectedTowerPresenter)
    {  
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        selectedBuilding = selectedTowerPresenter;
        OnSelectedTowerForUI?.Invoke();
    }

    private void HandleOnSelectedGuardPointBtnClick()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        if(selectedBuilding != null)
        {
            selectedBuilding.towerView.ShowRangeDetection(true);
        }
    }

    // selected new guard point
    private void HandleOnSelectedNewGuardPoint(Vector2 newGuardPointPos)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        selectedBuilding.towerView.ShowRangeDetection(false);
        barrackTowerManager.SetNewGuardPointPos(selectedBuilding, newGuardPointPos);
    }

    // raycast hit null
    private void HandleRaycatHitNull()
    {
        if(selectedBuilding == null) return;
        selectedBuilding.towerView.ShowRangeDetection(false);
        selectedBuilding.towerView.ShowRangeDetectionUpgrade(false);
        selectedBuilding = null;
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
