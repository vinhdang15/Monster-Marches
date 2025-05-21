using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButtonHandler : MonoBehaviour
{
    // Handles communication triggered by player interactions with UI buttons
    [SerializeField] private Button archerTowerBtn;
    [SerializeField] private Button mageTowerBtn;
    [SerializeField] private Button barrackTowerBtn;
    [SerializeField] private Button cannonTowerBtn;
    [SerializeField] private Button guardPointBtn;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button sellBtn;

    public event Action<Button> OnArcherTowerInitBtnClick;
    public event Action<Button> OnMageTowerInitBtnClick;
    public event Action<Button> OnBarrackTowerInitBtnClick;
    public event Action<Button> OnCannonTowerInitBtnClick;
    public event Action<Button> OnGuardPointBtnBtnClick;
    public event Action<Button> OnUpgradeTowerBtnClick;
    public event Action<Button> OnSellTowerBtnClick;

    public void PrepareGame()
    {
        GetButton();
        AddButtonListener();
    }

    private void GetButton()
    {
        archerTowerBtn      = GameObject.Find("InitArcherTowerBtn").GetComponent<Button>();
        mageTowerBtn        = GameObject.Find("InitMageTowerBtn").GetComponent<Button>();
        barrackTowerBtn     = GameObject.Find("InitBarrackTowerBtn").GetComponent<Button>();
        cannonTowerBtn      = GameObject.Find("InitCannonTowerBtn").GetComponent<Button>();
        guardPointBtn       = GameObject.Find("GuardPointBtn").GetComponent<Button>();
        upgradeBtn          = GameObject.Find("UpgradeTowerBtn").GetComponent<Button>();
        sellBtn             = GameObject.Find("SellTowerBtn").GetComponent<Button>();
    }

    private void AddButtonListener()
    {
        archerTowerBtn.onClick.AddListener(()   => OnArcherTowerInitBtnClick?.Invoke(archerTowerBtn));
        mageTowerBtn.onClick.AddListener(()     => OnMageTowerInitBtnClick?.Invoke(mageTowerBtn));
        barrackTowerBtn.onClick.AddListener(()  => OnBarrackTowerInitBtnClick?.Invoke(barrackTowerBtn));
        cannonTowerBtn.onClick.AddListener(()   => OnCannonTowerInitBtnClick?.Invoke(cannonTowerBtn));
        guardPointBtn.onClick.AddListener(()    => OnGuardPointBtnBtnClick?.Invoke(guardPointBtn));
        upgradeBtn.onClick.AddListener(()       => OnUpgradeTowerBtnClick?.Invoke(upgradeBtn));
        sellBtn.onClick.AddListener(()          => OnSellTowerBtnClick?.Invoke(sellBtn));
    }
}
