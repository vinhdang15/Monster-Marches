using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAction : MonoBehaviour
{
    private InputController inputController;
    private Button currentButton = null;

    public event Action<TowerType, EmptyPlot> OnTryToInitTower;
    public event Action<TowerType> OnInitTower;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] SoundEffectSO soundEffectSO;

    private void GetInputController()
    {
        inputController = FindObjectOfType<InputController>();
    }

    private void RegisterInputControllerEvent()
    {
        inputController.OnArcherTowerInit += OnArcherTowerInit;
        inputController.OnMageTowerInit += OnMageTowerInit;
        inputController.OnBarrackTowerInit += OnBarrackTowerInit;
        inputController.OnCannonTowerInit += OnCannonTowerInit;
        inputController.OnGuardPointBtn += OnGuardPointBtn;
        inputController.OnUpgradeTower += OnUpgradeTower;
        inputController.OnSellTower += OnSellTower;
    }

    private void HandleInitBtnClick(Button clickedButton, TowerType towerType)
    {
        if(currentButton != clickedButton)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            // OnTryToInitTower?.Invoke(towerType, emptyPlot);
            // OnFirstButtonClick?.Invoke(clickedButton);
        }
        else
        {
            OnInitTower?.Invoke(towerType);
        }
        currentButton = clickedButton;
    }

    public void TowerActionPrepareGame()
    {
        GetInputController();
        RegisterInputControllerEvent();
    }

    private void OnSellTower(Button button)
    {
        throw new NotImplementedException();
    }

    private void OnUpgradeTower(Button button)
    {
        throw new NotImplementedException();
    }

    private void OnGuardPointBtn(Button button)
    {
        throw new NotImplementedException();
    }

    private void OnCannonTowerInit(Button button)
    {
        throw new NotImplementedException();
    }

    private void OnBarrackTowerInit(Button button)
    {
        throw new NotImplementedException();
    }

    private void OnMageTowerInit(Button button)
    {
        throw new NotImplementedException();
    }

    private void OnArcherTowerInit(Button button)
    {
        throw new NotImplementedException();
    }
}
