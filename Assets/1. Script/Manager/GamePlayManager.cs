using System;
using System.Collections;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int gold = 200;
    public int lives = 20;
    public int currentLives;
    [HideInInspector] public int    archerTowerInitGold;
    [HideInInspector] public int    mageTowerInitGold;
    [HideInInspector] public int    barrackTowerInitGold;
    [HideInInspector] public int    cannonTowerInitGold;
    [HideInInspector] public int    towerUpgradeGold;
    [HideInInspector] public int    towerSellGold;
    private TowerDataReader         towerDataReader;
    private EnemyManager            enemyManager;
    private GamePlayUIManager       gamePlayUIManager;

    private RaycastHandler          raycastHandler;
    private TowerActionHandler      towerActionHandler;

    private BulletTowerManager      bulletTowerManager;
    private BarrackTowerManager     barrackTowerManager;
    public EnemySpawnerManager      enemySpawnerManager;
    private Vector2                 initMenuPanelPos;
    private EmptyPlot               selectedEmptyPlot;
    private TowerPresenter          selectedTower;
    private TowerPresenter          selectedBulletTower;
    private TowerPresenter          selectedBarackTower;
    private DustFX                  dustFX;
    public event Action             OnGoldChangeForUI;
    public event Action<int>        OnLiveChangeForUI;
    public event Action<float>      OnFinishedMatch;
    
    

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;

    public void PrepareGame(TowerDataReader tDR, GamePlayUIManager gPUIE, EnemyManager eM, RaycastHandler rH,
                            TowerActionHandler tAH, EnemySpawnerManager eSM,
                            BulletTowerManager bulletTM, BarrackTowerManager barrackTM,
                            DustFX dFX)
    {
        LoadComponents(tDR, gPUIE,eM,rH,tAH,eSM,bulletTM,barrackTM,dFX);
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

    private void LoadComponents(TowerDataReader tDR, GamePlayUIManager gPUIE, EnemyManager eM, RaycastHandler rH,
                                TowerActionHandler tAH, EnemySpawnerManager eSM,
                                BulletTowerManager bulletTM, BarrackTowerManager barrackTM,
                                DustFX dFX)
    {
        towerDataReader     = tDR; 
        gamePlayUIManager   = gPUIE;
        enemyManager        = eM;
        raycastHandler      = rH;
        towerActionHandler  = tAH;
        enemySpawnerManager = eSM;
        bulletTowerManager  = bulletTM;
        barrackTowerManager = barrackTM;
        dustFX              = dFX;
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
        archerTowerInitGold     = towerDataReader.towerDataListSO.GetGoldInit(TowerType.ArcherTower.ToString());
        mageTowerInitGold       = towerDataReader.towerDataListSO.GetGoldInit(TowerType.MageTower.ToString());
        barrackTowerInitGold    = towerDataReader.towerDataListSO.GetGoldInit(TowerType.Barrack.ToString());
        cannonTowerInitGold     = towerDataReader.towerDataListSO.GetGoldInit(TowerType.CannonTower.ToString());
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
        raycastHandler.blockRaycast = true;
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);
        selectedEmptyPlot.DisableCollider();
        Vector2 emptyPlotPos = selectedEmptyPlot.transform.position;

        selectedEmptyPlot.PlayBuildingFX(() => PlayDustFX(emptyPlotPos), () => InitTower(towerType));
        gamePlayUIManager.HandleRaycastHitNull();
    }

    private void PlayDustFX(Vector2 pos)
    {
        StartCoroutine(dustFX.Play(pos));
    }

    private void InitTower(TowerType towerType)
    {
        switch (towerType)
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
        raycastHandler.blockRaycast = false;
    }

    private void InitArcherTower()
    {
        OnInitTower(archerTowerInitGold, () => bulletTowerManager.InitArcherTower(initMenuPanelPos, selectedEmptyPlot));
    }

    private void InitMageTower()
    {
        OnInitTower(mageTowerInitGold, () => bulletTowerManager.InitMageTower(initMenuPanelPos, selectedEmptyPlot));
    }

    private void InitBarrackTower()
    {
        OnInitTower(barrackTowerInitGold, () => barrackTowerManager.InitBarack(initMenuPanelPos, selectedEmptyPlot));
    }

    private void InitCannonTower()
    {
        OnInitTower(cannonTowerInitGold, () => bulletTowerManager.InitCannonTower(initMenuPanelPos, selectedEmptyPlot));
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
        // Hide range detection and upgrade panel
        HandleRaycatHitNull();
        gamePlayUIManager.HandleRaycastHitNull();
    }

    private void UpgradeSelectedTower()
    {
        if(gold < selectedTower.GoldUpgrade) return;
        AudioManager.Instance.PlaySound(soundEffectSO.BuildSound);

        // check if there is barrack tower upgrade, then replace new soldier
        if(selectedTower.towerModel.TowerType == TowerType.Barrack.ToString())
        {
            SelectedBarrackUpgradeSoldier();
        }

        // process gold
        int goldUpdrade = selectedTower.GoldUpgrade;
        gold -= goldUpdrade;
        selectedTower.GoldRefund += goldUpdrade;
        OnGoldChangeForUI?.Invoke();

        PlayDustFX(selectedTower.transform.position);


        if (selectedTower.towerModel.TowerType == TowerType.Barrack.ToString())
        {
            barrackTowerManager.UpgradeTower(selectedTower);
        }
        else
        {
            bulletTowerManager.UpgradeTower(selectedTower);
        }
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
        gamePlayUIManager.HandleRaycastHitNull();
    }

    private void RemoveTowerInTowerList(TowerPresenter selectedTower)
    {
        if(selectedTower.towerModel.TowerType == TowerType.Barrack.ToString())
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
        selectedEmptyPlot = emptyPlot;
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
