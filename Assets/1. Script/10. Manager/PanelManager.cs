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
    [SerializeField] GameObject checkSmybol;

    [Header("GameMenu")]
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] GameObject pauseMenu;

    [Header("GameStatus")]
    [SerializeField] GameSttPanel gameSttPanel;

    [Header("TowerMenu")]
    [SerializeField] InitMenu initMenu;
    [SerializeField] UpgradeMenu upgradeMenu;

    [Header("TowerStatus")]
    [SerializeField] CurrentSttPanel currentSttPanel;
    [SerializeField] UpgradeSttPanel upgradeSttPanel;

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
    }

    private void Start()
    {
        GetTotalWave();
        ResetCurrentWave();
        HandleGoldChange();
        RegisterInputControllerEvent();
        RegisterGamePlayManagerEvent();
    }

    private void OnDisable()
    {
        UnregisterInputControllerEvent();
        UnregisterGamePlayManagerEvent();
    }

    private void HideCheckSymbol()
    {
        checkSmybol.SetActive(false);
    }

    #region REGISTER EVENT
    private void RegisterInputControllerEvent()
    {
        inputController.OnTryToInitTower                += HandleOnTryToInitTower;
        inputController.OnButtonClick                   += HandleShowCheckSymbol;
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
        inputController.OnButtonClick                   -= HandleShowCheckSymbol;
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
        checkSmybol.transform.position = clickedButton.transform.transform.position;
        checkSmybol.SetActive(true);
    }

    private void HandleRaycastHitNull()
    {
        HideCheckSymbol();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();
        initMenu.Hide();
        upgradeMenu.Hide();
        currentSttPanel.Hide();
        upgradeSttPanel.Hide();
    }

    private void HandleOnSelectedEmptyPlot(EmptyPlot plot)
    {
        HideCheckSymbol();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();
        upgradeMenu.Hide();
        initMenu.CheckAndShowInPos(plot.transform.position, gamePlayManager.gold);
    }

    private void HandleOnSelectedBulletTower(TowerPresenter presenter)
    {
        HideCheckSymbol();
        initMenu.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();

        currentSttPanel.SetCurrentSttText(presenter);
        currentSttPanel.Show();
        upgradeMenu.HideGuardPointBtn();
        upgradeMenu.UpdateText(presenter);
        upgradeMenu.CheckAndShowInPos(presenter.transform.position, gamePlayManager.gold);
    }

    private void HandleOnSelectedBarrackTower(TowerPresenter presenter)
    {
        HideCheckSymbol();
        initMenu.Hide();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();

        currentSttPanel.SetCurrentSttText(presenter);
        currentSttPanel.Show();
        upgradeMenu.ShowGuardPointBtn();
        upgradeMenu.UpdateText(presenter);
        upgradeMenu.CheckAndShowInPos(presenter.transform.position, gamePlayManager.gold);
    }

    private void HandleOnSelectedGuardPointBtnClick()
    {
        HideCheckSymbol();
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
    private void HandleGoldChange()
    {
        gameSttPanel.UpdateGold(gamePlayManager.gold);
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
        gameOverMenu.SetActive(true);
    }
    #endregion

    #region GAME MENU
    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ShowVictoryMenu()
    {
        victoryMenu.SetActive(true);
    }
    #endregion
}
