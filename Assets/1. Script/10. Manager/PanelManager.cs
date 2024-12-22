using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] InputController inputController;
    [SerializeField] GameObject checkSmybol;
    [Header("GameStatus")]
    [SerializeField] GameSttPanel gameSttPanel;
    [Header("TowerBuildingStatus")]
    [SerializeField] InitMenu initMenu;
    [SerializeField] UpgradeMenu upgradeMenu;

    [Header("TowerStatus")]
    [SerializeField] CurrentSttPanel currentSttPanel;
    [SerializeField] UpgradeSttPanel upgradeSttPanel;

    private void Start()
    {
        RegisterInputControllerEvent();
    }

    #region MENU, PANEL VISIBLE

    private void HideCheckSymbol()
    {
        checkSmybol.SetActive(false);
    }
    #endregion

    #region PANEL, MENU VISIBLE
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
        initMenu.ShowInPos(plot.transform.position);
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
        upgradeMenu.ShowInPos(presenter.transform.position);
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
        upgradeMenu.ShowInPos(presenter.transform.position);
    }

    private void HandleOnSelectedGuardPointBtnClick()
    {
        HideCheckSymbol();
        upgradeSttPanel.Hide();
        currentSttPanel.Hide();
        upgradeMenu.Hide();
    }
    #endregion
}
