using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIManager : MonoBehaviour
{
    public static GamePlayUIManager         Instance;
    [SerializeField] RaycastHandler         raycastHandler;
    [SerializeField] TowerActionHandler     towerActionHandler;
    [SerializeField] GamePlayManager        gamePlayManager;
    private MapManager                      mapManager;

    [Header("GameMenu")]
    [SerializeField] PauseMenu              pauseMenu;
    [SerializeField] VictoryMenu            victoryMenu;
    [SerializeField] GameOverMenu           gameOverMenu;
    [SerializeField] SelectedMapMenu        mapMenu;
    [SerializeField] InstructionMenu        instructionMenu;

    [Header("TowerMenu")]
    [SerializeField] InitMenu               initMenu;
    [SerializeField] UpgradeMenu            upgradeMenu;
    [SerializeField] CheckSymbol            checkSymbol;

    [Header("TowerStatus")]
    [SerializeField] CurrentTowerSttPanel   currentTowerSttPanel;
    [SerializeField] UpgradeTowerSttPanel   upgradeTowerSttPanel;

    [Header("GameStatus")]
    [SerializeField] GameSttPanel           gameSttPanel;

    private TowerPresenter                  selectedTower;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PrepareGame(TowerDataReader towerDataReader,
                            BulletDataReader bulletDataReader,
                            UnitDataReader unitDataReader)
                            
    {
        LoadComponents(towerDataReader, bulletDataReader, unitDataReader);
    }

    public void GetInfor()
    {
        GetTotalWave();
        ResetCurrentWave();
        UpdateCurrentGold();
        RegisterInputControllerEvent();
        RegisterGamePlayManagerEvent();
        GetTotalLive();
    }

    private void OnDisable()
    {
        UnregisterInputControllerEvent();
        UnregisterGamePlayManagerEvent();
    }

    private void LoadComponents(TowerDataReader towerDataReader,
                                BulletDataReader bulletDataReader,
                                UnitDataReader unitDataReader)
    {
        raycastHandler          = FindObjectOfType<RaycastHandler>();
        towerActionHandler      = FindObjectOfType<TowerActionHandler>();
        gamePlayManager         = FindObjectOfType<GamePlayManager>();
        mapManager              = FindAnyObjectByType<MapManager>();

        initMenu                = FindObjectOfType<InitMenu>();

        upgradeMenu             = FindObjectOfType<UpgradeMenu>();
        checkSymbol             = FindObjectOfType<CheckSymbol>();

        pauseMenu               = FindObjectOfType<PauseMenu>();
        victoryMenu             = FindObjectOfType<VictoryMenu>();
        gameOverMenu            = FindObjectOfType<GameOverMenu>();
        mapMenu                 = FindObjectOfType<SelectedMapMenu>();
        instructionMenu         = FindObjectOfType<InstructionMenu>();

        currentTowerSttPanel    = FindObjectOfType<CurrentTowerSttPanel>();
        upgradeTowerSttPanel    = FindObjectOfType<UpgradeTowerSttPanel>();
        gameSttPanel            = FindObjectOfType<GameSttPanel>();

        initMenu.PrepareGame(towerDataReader);
        upgradeTowerSttPanel.PrepareGame(towerDataReader, bulletDataReader, unitDataReader);
    }

    #region REGISTER EVENT
    private void RegisterInputControllerEvent()
    {
        raycastHandler.OnRaycastHitNull         += HandleRaycastHitNull;
        raycastHandler.OnSelectedEmptyPlot      += HandleOnSelectedEmptyPlot;
        raycastHandler.OnSelectedBulletTower    += HandleOnSelectedBulletTower;
        raycastHandler.OnSelectedBarrackTower   += HandleOnSelectedBarrackTower;

        towerActionHandler.OnFirstButtonClick   += HandleShowCheckSymbol;
        towerActionHandler.OnTryToInitTower     += HandleOnTryToInitTower;
        towerActionHandler.OnGuardPointBtnClick += HandleGuardPointBtnClick;
        towerActionHandler.OnTryToUpgradeTower  += HandleOnTryToUpgradeTower;
    }

    private void UnregisterInputControllerEvent()
    {
        raycastHandler.OnRaycastHitNull         -= HandleRaycastHitNull;
        raycastHandler.OnSelectedEmptyPlot      -= HandleOnSelectedEmptyPlot;
        raycastHandler.OnSelectedBulletTower    -= HandleOnSelectedBulletTower;
        raycastHandler.OnSelectedBarrackTower   -= HandleOnSelectedBarrackTower;

        towerActionHandler.OnFirstButtonClick   -= HandleShowCheckSymbol;
        towerActionHandler.OnTryToInitTower     -= HandleOnTryToInitTower;
        towerActionHandler.OnGuardPointBtnClick -= HandleGuardPointBtnClick;
        towerActionHandler.OnTryToUpgradeTower  -= HandleOnTryToUpgradeTower;
    }

    private void RegisterGamePlayManagerEvent()
    {
        gamePlayManager.OnGoldChangeForUI       += HandleGoldChange;
        gamePlayManager.OnLiveChangeForUI       += HandleLiveChange;
        gamePlayManager.OnFinishedMatch         += HandleFinishedMatch;
        gamePlayManager.enemySpawnerManager.OnUpdateCurrentWave += HandleUpdateCurrentWave;
    }

    private void UnregisterGamePlayManagerEvent()
    {
        gamePlayManager.OnGoldChangeForUI       -= HandleGoldChange;
        gamePlayManager.OnLiveChangeForUI       -= HandleLiveChange;
        gamePlayManager.OnFinishedMatch         -= HandleFinishedMatch;
        gamePlayManager.enemySpawnerManager.OnUpdateCurrentWave -= HandleUpdateCurrentWave;
    }
    #endregion

    #region PANEL, MENU VISIBLE
    private void HandleOnTryToInitTower(TowerType type, EmptyPlot plot)
    {
        upgradeTowerSttPanel.SetInitSttText(type);
        upgradeTowerSttPanel.ShowInPos(plot.transform.position);
    }

    private void HandleOnTryToUpgradeTower(TowerPresenter towerPresenter)
    {
        upgradeTowerSttPanel.SetUpgradeSttText(towerPresenter);
        upgradeTowerSttPanel.ShowInPos(towerPresenter.transform.position);
    }

    private void HandleShowCheckSymbol(Button clickedButton)
    {
        checkSymbol.transform.position = clickedButton.transform.transform.position;
        checkSymbol.GreyOutCheckSymbol(clickedButton);
        checkSymbol.Show();
    }

    public void HandleRaycastHitNull()
    {
        initMenu.Hide();
        upgradeMenu.Hide();
        checkSymbol.Hide();
        upgradeTowerSttPanel.Hide();
        currentTowerSttPanel.Hide();
        currentTowerSttPanel.Hide();
        upgradeTowerSttPanel.Hide();
    }

    private void HandleOnSelectedEmptyPlot(EmptyPlot emptyPlotPos)
    {
        initMenu.Hide();
        upgradeMenu.Hide();
        checkSymbol.Hide();
        upgradeTowerSttPanel.Hide();
        currentTowerSttPanel.Hide();
        initMenu.ButtonCheckInitGoldRequire(GetCurrentGold());
        initMenu.ShowInPos(emptyPlotPos.transform.position);
    }

    private void HandleOnSelectedBulletTower(TowerPresenter selectedPresenter)
    {
        selectedTower = selectedPresenter;
        initMenu.Hide();
        checkSymbol.Hide();
        upgradeTowerSttPanel.Hide();
        currentTowerSttPanel.Hide();
        UpgradeButtonVisible(selectedPresenter);

        currentTowerSttPanel.SetCurrentSttText(selectedPresenter);
        currentTowerSttPanel.Show();
        upgradeMenu.HideGuardPointBtn();
        upgradeMenu.UpdateText(selectedPresenter);
        upgradeMenu.UpdateButtonColor(selectedPresenter, GetCurrentGold());
        upgradeMenu.ShowInPos(selectedPresenter.transform.position);
    }

    private void HandleOnSelectedBarrackTower(TowerPresenter selectedPresenter)
    {
        selectedTower = selectedPresenter;
        initMenu.Hide();
        checkSymbol.Hide();
        upgradeTowerSttPanel.Hide();
        currentTowerSttPanel.Hide();
        UpgradeButtonVisible(selectedPresenter);

        currentTowerSttPanel.SetCurrentSttText(selectedPresenter);
        currentTowerSttPanel.Show();
        upgradeMenu.ShowGuardPointBtn();
        upgradeMenu.UpdateText(selectedPresenter);
        upgradeMenu.UpdateButtonColor(selectedPresenter, GetCurrentGold());
        upgradeMenu.ShowInPos(selectedPresenter.transform.position);
    }

    private void UpgradeButtonVisible(TowerPresenter presenter)
    {
        if (presenter.IsTowerMaxLevel()) upgradeMenu.HideUpgradeButton();
        else upgradeMenu.ShowUpgradeButton();
    }

    private void HandleGuardPointBtnClick()
    {
        checkSymbol.Hide();
        upgradeTowerSttPanel.Hide();
        currentTowerSttPanel.Hide();
        upgradeMenu.Hide();
    }
    #endregion

    #region GAME STT PANEL
    private void ResetCurrentWave()
    {
        gameSttPanel.ResetCurrentWave();
    }

    private void GetTotalLive()
    {
        int live = gamePlayManager.lives;
        gameSttPanel.UpdateLive(live);
    }

    private void GetTotalWave()
    {
        int totalWave = gamePlayManager.enemySpawnerManager.TotalWave;
        gameSttPanel.GetTotalWave(totalWave);
    }

    private void UpdateCurrentGold()
    {
        gameSttPanel.UpdateGold(GetCurrentGold());
    }

    private void HandleGoldChange()
    {
        UpdateCurrentGold();
        initMenu.ButtonCheckInitGoldRequire(GetCurrentGold());

        if (selectedTower == null) return;
        upgradeMenu.UpdateButtonColor(selectedTower, GetCurrentGold());
    }

    private void HandleUpdateCurrentWave(int currentWave)
    {
        gameSttPanel.HandleUpdateCurrentWave(currentWave);
    }

    private void HandleLiveChange(int currentLives)
    {
        gameSttPanel.UpdateLive(currentLives);
        if (currentLives != 0) return;
        gameOverMenu.Show(gameOverMenu.PauseGame);
    }

    private void HandleFinishedMatch(float lifePercentage)
    {
        victoryMenu.SetStarScore(lifePercentage);
        victoryMenu.StartVictoryMenu();
        mapManager.UpdateMapDataJson();
        mapManager.SetCurrentMapStarPoint(victoryMenu.StarScore());
    }

    private int GetCurrentGold()
    {
        return gamePlayManager.gold;
    }
    #endregion

    #region GAME MENU
    public void ShowMapMenu(MapModel mapModel)
    {
        mapMenu.ShowSelectedMapMenu(mapModel);
    }

    public void HideMapMenu()
    {
        mapMenu.Hide();
    }

    public void ShowInstructionMenu(bool hasActive)
    {
        if(!hasActive) instructionMenu.ShowInstruction();
    }

    public void InstructionNextBtnClick()
    {
        instructionMenu.NextButtonClick();
    }

    public void InstructionSkipBtnClick()
    {
        instructionMenu.HideInstruction();
    }

    public void ShowPauseMenu()
    {
        pauseMenu.Show(pauseMenu.PauseGame);
    }

    public void HidePauseMenu()
    {
        pauseMenu.Hide();
    }

    public void ResetVictoryMenu()
    {
        victoryMenu.ResetState();
    }

    public void HideGameOverMenu()
    {
        gameOverMenu.Hide();
    }
    #endregion
}
