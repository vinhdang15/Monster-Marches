using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    // chịu trách nhiệm xử lý các dữ kiện đầu vào của người dùng
    [SerializeField] private Button archerTowerBtn;
    [SerializeField] private Button mageTowerBtn;
    [SerializeField] private Button barrackTowerBtn;
    [SerializeField] private Button cannonTowerBtn;
    [SerializeField] private Button guardPointBtn;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button sellBtn;

    public event Action<Button> OnArcherTowerInit;
    public event Action<Button> OnMageTowerInit;
    public event Action<Button> OnBarrackTowerInit;
    public event Action<Button> OnCannonTowerInit;
    public event Action<Button> OnGuardPointBtn;
    public event Action<Button> OnUpgradeTower;
    public event Action<Button> OnSellTower;

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
        archerTowerBtn.onClick.AddListener(() => OnArcherTowerInit?.Invoke(archerTowerBtn));
        mageTowerBtn.onClick.AddListener(() => OnMageTowerInit?.Invoke(mageTowerBtn));
        barrackTowerBtn.onClick.AddListener(() => OnBarrackTowerInit?.Invoke(barrackTowerBtn));
        cannonTowerBtn.onClick.AddListener(() => OnCannonTowerInit?.Invoke(cannonTowerBtn));
        guardPointBtn.onClick.AddListener(() => OnGuardPointBtn?.Invoke(guardPointBtn));
        upgradeBtn.onClick.AddListener(() => OnUpgradeTower?.Invoke(upgradeBtn));
        sellBtn.onClick.AddListener(() => OnSellTower?.Invoke(sellBtn));
    }

    public void InputCOntrollerPrepareGame()
    {
        GetButton();
        AddButtonListener();
    }
}
