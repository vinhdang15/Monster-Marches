using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerActionHandler : MonoBehaviour
{
    private RaycastHandler      raycastHandler;
    private InputButtonHandler  inputButtonHandler;
    private EmptyPlot           selectedEmptyPlot;
    private TowerPresenter      selectedTower;
    private Button              currentButton = null;
    

    public event Action<Button>                 OnFirstButtonClick;
    public event Action<TowerType, EmptyPlot>   OnTryToInitTower;
    public event Action<TowerType>              OnInitTower;
    public event Action                         OnGuardPointBtnClick;
    public event Action<TowerPresenter>         OnTryToUpgradeTower;
    public event Action                         OnUpgradeTower;
    public event Action                         OnSellTower;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] SoundEffectSO soundEffectSO;

    

    public void PrepareGame()
    {
        LoadComponents();
        RegisterInputControllerEvent();
        RegisterRaycastHandlerEvent();
        audioSource = GetComponent<AudioSource>();
    }

    private void LoadComponents()
    {
        inputButtonHandler = FindObjectOfType<InputButtonHandler>();
        raycastHandler = FindObjectOfType<RaycastHandler>();
    }

    private void OnDisable()
    {
        UnregisterInputControllerEvent();
        UnregisterRaycastHandlerEvent();
    }

    private void RegisterRaycastHandlerEvent()
    {
        raycastHandler.OnRaycastHitNull         += HandleRaycastHitNull;
        raycastHandler.OnSelectedEmptyPlot      += HandleSelectedEmptyPlot;
        raycastHandler.OnSelectedBulletTower    += HandleOnSelectedTower;
        raycastHandler.OnSelectedBarrackTower   += HandleOnSelectedTower;
    }

    private void UnregisterRaycastHandlerEvent()
    {
        raycastHandler.OnRaycastHitNull         -= HandleRaycastHitNull;
        raycastHandler.OnSelectedEmptyPlot      -= HandleSelectedEmptyPlot;
        raycastHandler.OnSelectedBulletTower    -= HandleOnSelectedTower;
        raycastHandler.OnSelectedBarrackTower   -= HandleOnSelectedTower;
    }

    private void UnregisterInputControllerEvent()
    {
        inputButtonHandler.OnArcherTowerInitBtnClick    -= HandleArcherTowerInitBtnClick;
        inputButtonHandler.OnMageTowerInitBtnClick      -= HandleMageTowerInitBtnClick;
        inputButtonHandler.OnBarrackTowerInitBtnClick   -= HandleBarrackTowerInitBtnClick;
        inputButtonHandler.OnCannonTowerInitBtnClick    -= HandleCannonTowerInitBtnClick;
        inputButtonHandler.OnGuardPointBtnBtnClick      -= HandleGuardPointBtnClick;
        inputButtonHandler.OnUpgradeTowerBtnClick       -= HandleUpgradeTowerBtnClick;
        inputButtonHandler.OnSellTowerBtnClick          -= HandleSellTowerBtnClick;
    }

    private void RegisterInputControllerEvent()
    {
        inputButtonHandler.OnArcherTowerInitBtnClick    += HandleArcherTowerInitBtnClick;
        inputButtonHandler.OnMageTowerInitBtnClick      += HandleMageTowerInitBtnClick;
        inputButtonHandler.OnBarrackTowerInitBtnClick   += HandleBarrackTowerInitBtnClick;
        inputButtonHandler.OnCannonTowerInitBtnClick    += HandleCannonTowerInitBtnClick;
        inputButtonHandler.OnGuardPointBtnBtnClick      += HandleGuardPointBtnClick;
        inputButtonHandler.OnUpgradeTowerBtnClick       += HandleUpgradeTowerBtnClick;
        inputButtonHandler.OnSellTowerBtnClick          += HandleSellTowerBtnClick;
    }
    
    private void HandleOnSelectedTower(TowerPresenter presenter)
    {
        selectedTower = presenter;
    }

    private void HandleSelectedEmptyPlot(EmptyPlot plot)
    {
        selectedEmptyPlot = plot;
    }

    private void HandleRaycastHitNull()
    {
        currentButton = null;
    }

    #region INIT TOWER
    private void HandleInitBtnClick(Button clickedButton, TowerType towerType)
    {
        if(currentButton != clickedButton)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            OnTryToInitTower?.Invoke(towerType, selectedEmptyPlot);
            OnFirstButtonClick?.Invoke(clickedButton);
        }
        else
        {
            OnInitTower?.Invoke(towerType);
        }
        currentButton = clickedButton;
    }

    private void HandleArcherTowerInitBtnClick(Button button)
    {
        HandleInitBtnClick(button, TowerType.ArcherTower);
    }

    private void HandleMageTowerInitBtnClick(Button button)
    {
        HandleInitBtnClick(button, TowerType.MageTower);
    }

    private void HandleBarrackTowerInitBtnClick(Button button)
    {
        HandleInitBtnClick(button, TowerType.Barrack);
    }

    private void HandleCannonTowerInitBtnClick(Button button)
    {
        HandleInitBtnClick(button, TowerType.CannonTower);
    }
    #endregion

    #region UPGRADE TOWER
    private void HandleUpgradeTowerBtnClick(Button button)
    {
        if(currentButton != button)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            OnFirstButtonClick?.Invoke(button);
            OnTryToUpgradeTower?.Invoke(selectedTower);
        }
        else
        {
            OnUpgradeTower?.Invoke();
        }
        currentButton = button;
    }
    #endregion

    #region BARRACK GUARD POINT CLICK
    private void HandleGuardPointBtnClick(Button button)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
        OnGuardPointBtnClick?.Invoke();
    }
    #endregion
    
    #region SELL
    private void HandleSellTowerBtnClick(Button button)
    {
        if(currentButton != button)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            OnFirstButtonClick?.Invoke(button);
        }
        else
        {
            OnSellTower?.Invoke();
            return;
        }
        currentButton = button;
    }
    #endregion
}
