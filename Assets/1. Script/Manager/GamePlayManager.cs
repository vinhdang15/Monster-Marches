using System;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int gold = 200;
    public int lives = 20;
    public int currentLives;
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
    public EnemySpawnerManager                  enemySpawnerManager;
    private Vector2                             initMenuPanelPos;
    private EmptyPlot                           SelectedEmptyPlot;
    private TowerPresenter                      selectedTower;
    private TowerPresenter                      selectedBulletTower;
    private TowerPresenter                      selectedBarackTower;
    private DustFX dustFX;
    public event Action OnGoldChangeForUI;
    public event Action<int> OnLiveChangeForUI;
    public event Action<float> OnFinishedMatch;
    
    

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;

    public void PrepareGame(PanelManager PM, EnemyManager EM, RaycastHandler RH,
                            TowerActionHandler TAH, EnemySpawnerManager ESM,
                            BulletTowerManager BulletTM, BarrackTowerManager BarrackTM,
                            DustFX DFX)
    {
        LoadComponents(PM,EM,RH,TAH,ESM,BulletTM,BarrackTM,DFX);
        RegisterEnemyEvent();
        RegisterButtonEvent();
        RegisterCautionClickEvent();
    }

    public void GetInfor(MapData mapData)
    {
        GetTowerInitGold();
        GetCurentMapInitGold(mapData);
        GetCurrentMapLive(mapData);
        currentLives = lives;
    }

    private void OnDisable()
    {
        UnregisterEnemyEvent();
        UnregisterButtonEvent();
        UnregisterCautionClickEvent();
    }

    private void LoadComponents(PanelManager PM, EnemyManager EM, RaycastHandler RH,
                                TowerActionHandler TAH, EnemySpawnerManager ESM,
                                BulletTowerManager BulletTM, BarrackTowerManager BarrackTM,
                                DustFX DFX)
    {
        panelManager        = PM;
        enemyManager        = EM;
        raycastHandler      = RH;
        towerActionHandler  = TAH;
        enemySpawnerManager = ESM;
        bulletTowerManager  = BulletTM;
        barrackTowerManager = BarrackTM;
        dustFX              = DFX;
    }

    private void GetCurentMapInitGold(MapData mapData)
    {
        gold = mapData.goldInit;
    }

    private void GetCurrentMapLive(MapData mapData)
    {
        lives = mapData.lives;
    }

    private void GetTowerInitGold()
    {
        archerTowerInitGold = TowerDataReader.Instance.towerDataListSO.GetGoldInit(TowerType.ArcherTower.ToString());
        mageTowerInitGold = TowerDataReader.Instance.towerDataListSO.GetGoldInit(TowerType.MageTower.ToString());
        barrackTowerInitGold = TowerDataReader.Instance.towerDataListSO.GetGoldInit(TowerType.Barrack.ToString());
        cannonTowerInitGold = TowerDataReader.Instance.towerDataListSO.GetGoldInit(TowerType.CannonTower.ToString());
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
        HandleFinishedMatch();
        // Debug.Log(spawnEnemyManager.totalEnemies + "    " + enemyManager.totalEnemiesDie);
    }

    private void HandleEnemyReachEndPoint()
    {
        if(currentLives > 0) currentLives --;
        OnLiveChangeForUI?.Invoke(currentLives);
        HandleFinishedMatch();
        // Debug.Log(spawnEnemyManager.totalEnemies + "    " + enemyManager.totalEnemiesDie);
    }

    private void HandleFinishedMatch()
    {
        if(enemySpawnerManager.totalEnemies == enemyManager.totalEnemiesDie && currentLives != 0)
        {
            float lifePercentage = (float)currentLives / lives * 100;
            // Debug.Log(spawnEnemyManager.totalEnemies + "    " + enemyManager.totalEnemiesDie);
            OnFinishedMatch?.Invoke(lifePercentage);
        }
    }
    #endregion

    // Caution click event
    private void RegisterCautionClickEvent()
    {
        enemySpawnerManager.OnAddGoldWhenCautionClick += HandleAddGoldWhenCautionClick;
    }

    private void UnregisterCautionClickEvent()
    {
        enemySpawnerManager.OnAddGoldWhenCautionClick -= HandleAddGoldWhenCautionClick;
    }

    private void HandleAddGoldWhenCautionClick(int goldAdd)
    {
        gold += goldAdd;
        OnGoldChangeForUI?.Invoke();
    }

    #region INIT TOWER
    private void OnInitTower(int goldRequired, Action action)
    {
        if(gold < goldRequired) return;
        action?.Invoke();
        gold -= goldRequired;
        OnGoldChangeForUI?.Invoke();
    }
    
    // Init tower
    private void HandleInitTower(TowerType  towerType)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);
        SelectedEmptyPlot.DisableCollider();
        SelectedEmptyPlot.PlayBuildingFX(() => PlayDustFX(SelectedEmptyPlot.transform.position), () => InitTower(towerType));
        panelManager.HandleRaycastHitNull();
    }

    private void PlayDustFX(Vector2 pos)
    {
        dustFX.PlayBuildingFX(pos);
    }

    private void InitTower(TowerType  towerType)
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
        OnInitTower(archerTowerInitGold, () => bulletTowerManager.InitArcherTower(initMenuPanelPos, SelectedEmptyPlot));
    }

    private void InitMageTower()
    {
        OnInitTower(mageTowerInitGold, () => bulletTowerManager.InitMageTower(initMenuPanelPos, SelectedEmptyPlot));
    }

    private void InitBarrackTower()
    {
        OnInitTower(barrackTowerInitGold, () => barrackTowerManager.InitBarack(initMenuPanelPos, SelectedEmptyPlot));
    }

    private void InitCannonTower()
    {
        OnInitTower(cannonTowerInitGold, () => bulletTowerManager.InitCannonTower(initMenuPanelPos, SelectedEmptyPlot));
    }
    #endregion

    #region UPGRADE TOWER
    // Upgrade selected tower
    private void HandleTryToUpgradeSelectedTower(TowerPresenter towerPresenter)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        bulletTowerManager.UpdateRangeDetectionUpgrade(selectedTower);
        selectedTower.towerViewBase.ShowRangeDetectionUpgrade(true);
    }

    private void HandleUpgradeSelectedTower()
    {
        UpgradeSelectedTower();

        // check if there is barrack tower upgrade, then replace new soldier
        if(selectedTower.towerModel.TowerType == "Barrack")
        {
            SelectedBarrackUpgradeSoldier();
        }

        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
        panelManager.HandleRaycastHitNull();
    }

    private void UpgradeSelectedTower()
    {
        if(gold < selectedTower.GoldUpgrade) return;
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);

        // process gold
        int goldUpdrade = selectedTower.GoldUpgrade;
        gold -= goldUpdrade;
        OnGoldChangeForUI?.Invoke();

        PlayDustFX(selectedTower.transform.position);
        TowerBaseManager towerBaseManager = bulletTowerManager;
        towerBaseManager.UpgradeTower(selectedTower);
        selectedTower.GoldRefund += goldUpdrade;
    }

    private void SelectedBarrackUpgradeSoldier()
    {
        barrackTowerManager.ReplaceSoldier(selectedTower);
    }
    #endregion

    #region SELL TOWER
    // Sell selected tower
    private void HandleSellSelectedTower()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.AddGoldSound);
        gold += selectedTower.GoldRefund;
        selectedTower.emptyPlot.EnableCollider();
        RemoveTowerInTowerList(selectedTower);
        OnGoldChangeForUI?.Invoke();
        panelManager.HandleRaycastHitNull();
    }

    private void RemoveTowerInTowerList(TowerPresenter selectedTower)
    {
        if(selectedTower.towerModel.TowerType == "Barrack")
        {
            barrackTowerManager.CleanupSelectedTower(selectedTower);
        }
        else
        {
            bulletTowerManager.CleanupSelectedTower(selectedTower);
        }   
    }

    #endregion

    #region SELECT EMPTYPLOT, BULLET TOWER, BARRACK TOWER
    private void HandleSelectedEmptyPlot(EmptyPlot emptyPlot)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        SelectedEmptyPlot = emptyPlot;
        initMenuPanelPos = emptyPlot.GetPos();
    }

    private void HandleOnSelectedBulletTower(TowerPresenter selectedTowerPresenter)
    {  
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        selectedBulletTower = selectedTowerPresenter;
        selectedTower = selectedBulletTower;
        selectedTower.towerViewBase.ShowRangeDetection(true);
    }

    private void HandleOnSelectedBarrackTower(TowerPresenter selectedTowerPresenter)
    {  
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        HideCurrentTowerRangeDetect();
        selectedBarackTower = selectedTowerPresenter;
        selectedTower = selectedBarackTower;
    }
    #endregion

    #region PICK NEW GUARD POINT, MOVE SOLDIER 
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
    #endregion

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
        return selectedTower.GoldUpgrade;
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
