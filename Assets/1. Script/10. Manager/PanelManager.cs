using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;
    [SerializeField] InputController inputController;
    [SerializeField] GamePlayManager gamePlayManager;

    [Header("GameMenu")]
    [SerializeField] PanelUI pauseMenu;
    [SerializeField] PanelUI victoryMenu;
    [SerializeField] PanelUI gameOverMenu;

    [Header("TowerMenu")]
    [SerializeField] InitMenu initMenu;
    [SerializeField] UpgradeMenu upgradeMenu;
    [SerializeField] CheckSymbol checkSymbol;

    [Header("TowerStatus")]
    [SerializeField] CurrentSttPanel currentSttPanel;
    [SerializeField] UpgradeSttPanel upgradeSttPanel;

    [Header("GameStatus")]
    [SerializeField] GameSttPanel gameSttPanel;

    private TowerPresenter CurrentSelectedPresenter;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadComponents();
    }

    private void Start()
    {
        GetTotalWave();
        ResetCurrentWave();
        UpdateCurrentGold();
        RegisterInputControllerEvent();
        RegisterGamePlayManagerEvent();
    }

    private void OnDisable()
    {
        UnregisterInputControllerEvent();
        UnregisterGamePlayManagerEvent();
    }

    private void LoadComponents()
    {
        inputController     = GameObject.Find("InputController").GetComponent<InputController>();
        gamePlayManager     = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        initMenu            = GameObject.Find("InitMenu").GetComponent<InitMenu>();
        upgradeMenu         = GameObject.Find("UpgradeMenu").GetComponent<UpgradeMenu>();
        checkSymbol         = GameObject.Find("CheckSymbol").GetComponent<CheckSymbol>();

        pauseMenu           = GameObject.Find("PauseMenu").GetComponent<PanelUI>();
        victoryMenu         = GameObject.Find("VictoryMenu").GetComponent<PanelUI>();
        gameOverMenu        = GameObject.Find("GameOverMenu").GetComponent<PanelUI>();

        currentSttPanel     = GameObject.Find("CurrentSttPanel").GetComponent<CurrentSttPanel>();
        upgradeSttPanel     = GameObject.Find("UpgradeSttPanel").GetComponent<UpgradeSttPanel>();
        gameSttPanel        = GameObject.Find("GameSttPanel").GetComponent<GameSttPanel>();
    }

    #region REGISTER EVENT
    private void RegisterInputControllerEvent()
    {
        inputController.OnTryToInitTower                += HandleOnTryToInitTower;
        inputController.OnFirstButtonClick              += HandleShowCheckSymbol;
        inputController.OnButtonDoubleClick             += HandleRaycastHitNull;
        inputController.OnRaycastHitNull                += HandleRaycastHitNull;
        inputController.OnSelectedEmptyPlot             += HandleOnSelectedEmptyPlot;
        inputController.OnSelectedBulletTower           += HandleOnSelectedBulletTower;
        inputController.OnSelectedBarrackTower          += HandleOnSelectedBarrackTower;
        inputController.OnSelectedGuardPointBtnClick    += HandleOnSelectedGuardPointBtnClick;
        inputController.OnTryToUpgradeTower             += HandleOnTryToUpgradeTower;
    }

    private void UnregisterInputControllerEvent()
    {
        inputController.OnTryToInitTower                -= HandleOnTryToInitTower;
        inputController.OnFirstButtonClick              -= HandleShowCheckSymbol;
        inputController.OnButtonDoubleClick             -= HandleRaycastHitNull;
        inputController.OnRaycastHitNull                -= HandleRaycastHitNull;
        inputController.OnSelectedEmptyPlot             -= HandleOnSelectedEmptyPlot;
        inputController.OnSelectedBulletTower           -= HandleOnSelectedBulletTower;
        inputController.OnSelectedBarrackTower          -= HandleOnSelectedBarrackTower;
        inputController.OnSelectedGuardPointBtnClick    -= HandleOnSelectedGuardPointBtnClick;
        inputController.OnTryToUpgradeTower             -= HandleOnTryToUpgradeTower;
    }

    private void RegisterGamePlayManagerEvent()
    {
        gamePlayManager.OnGoldChangeForUI                       += HandleGoldChange;
        gamePlayManager.OnLiveChangeForUI                       += HandleLiveChange;
        gamePlayManager.spawnEnemyManager.OnUpdateCurrentWave   += HandleUpdateCurrentWave;
    }

    private void UnregisterGamePlayManagerEvent()
    {
        gamePlayManager.OnGoldChangeForUI                       -= HandleGoldChange;
        gamePlayManager.OnLiveChangeForUI                       -= HandleLiveChange;
        gamePlayManager.spawnEnemyManager.OnUpdateCurrentWave   -= HandleUpdateCurrentWave;
    }
    #endregion

    #region PANEL, MENU VISIBLE
    private void HandleOnTryToInitTower(TowerType type, EmptyPlot plot)
    {
        upgradeSttPanel.SetInitSttText(type);
        upgradeSttPanel.ShowInPos(plot.transform.position);
    }

    private void HandleOnTryToUpgradeTower(TowerPresenter towerPresenter)
    {
        upgradeSttPanel.SetUpgradeSttText(towerPresenter);
        upgradeSttPanel.ShowInPos(towerPresenter.transform.position);
    }

    private void HandleShowCheckSymbol(Button clickedButton)
    {
        checkSymbol.transform.position = clickedButton.transform.transform.position;
        checkSymbol.GreyOutCheckSymbol(clickedButton);
        checkSymbol.Show();
    }

    private void HandleRaycastHitNull()
    {
        initMenu.Hide();
        upgradeMenu.Hide();
        checkSymbol.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();
        currentSttPanel.Hide();
        upgradeSttPanel.Hide();
    }

    private void HandleOnSelectedEmptyPlot(EmptyPlot plot)
    {   
        initMenu.Hide();
        upgradeMenu.Hide();
        checkSymbol.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();
        initMenu.ButtonCheckInitGoldRequire(GetCurrentGold());
        initMenu.ShowInPos(plot.transform.position);
    }

    private void HandleOnSelectedBulletTower(TowerPresenter presenter)
    {
        CurrentSelectedPresenter = presenter;
        initMenu.Hide();
        checkSymbol.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();

        currentSttPanel.SetCurrentSttText(presenter);
        currentSttPanel.Show();
        upgradeMenu.HideGuardPointBtn();
        upgradeMenu.UpdateText(presenter);
        upgradeMenu.UpdateButtonColor(presenter,GetCurrentGold());
        upgradeMenu.ShowInPos(presenter.transform.position);
    }

    private void HandleOnSelectedBarrackTower(TowerPresenter presenter)
    {
        CurrentSelectedPresenter = presenter;
        initMenu.Hide();
        checkSymbol.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();

        currentSttPanel.SetCurrentSttText(presenter);
        currentSttPanel.Show();
        upgradeMenu.ShowGuardPointBtn();
        upgradeMenu.UpdateText(presenter);
        upgradeMenu.UpdateButtonColor(presenter,GetCurrentGold());
        upgradeMenu.ShowInPos(presenter.transform.position);
    }

    private void HandleOnSelectedGuardPointBtnClick()
    {
        checkSymbol.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();
        upgradeMenu.Hide();
    }
    #endregion

    #region GAME STT PANEL
    private void ResetCurrentWave()
    {
        gameSttPanel.ResetCurrentWave();
    }
    
    private void GetTotalWave()
    {
        int totalWave = gamePlayManager.spawnEnemyManager.TotalWave;
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

        if(CurrentSelectedPresenter == null) return;
        upgradeMenu.UpdateButtonColor(CurrentSelectedPresenter,GetCurrentGold());
    }
    
    private void HandleUpdateCurrentWave(int currentWave)
    {
        gameSttPanel.HandleUpdateCurrentWave(currentWave);
    }

    private void HandleLiveChange()
    {
        int live = gamePlayManager.live;
        gameSttPanel.UpdateLive(live);
        if(live != 0) return;
        gameOverMenu.Show();
    }

    private int GetCurrentGold()
    {
        return gamePlayManager.gold;
    }
    #endregion

    #region GAME MENU
    public void ShowPauseMenu()
    {
        pauseMenu.Show();
    }

    public void HidePauseMenu()
    {
        pauseMenu.Hide();
    }

    public void ShowVictoryMenu()
    {
        victoryMenu.Show();
    }
    #endregion
}
