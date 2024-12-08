using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GamePlayManager gamePlayManager;
    [Header("For UI")]
    [SerializeField] TextMeshProUGUI goldText;
    [Header("Panel Gold Text")]
    [SerializeField] TextMeshProUGUI ArcherTowerInitGoldText;
    [SerializeField] TextMeshProUGUI mageTowerInitGoldText;
    [SerializeField] TextMeshProUGUI barrackTowerInitGoldText;
    [SerializeField] TextMeshProUGUI cannonTowerInitGoldText;
    [SerializeField] TextMeshProUGUI upgradeTowerGoldText;
    [SerializeField] TextMeshProUGUI sellTowerGoldText;
    [Header("Wave")]
    [SerializeField] TextMeshProUGUI totalWaveText;
    [SerializeField] TextMeshProUGUI currentWaveText;

    private void Start()
    {
        gamePlayManager = GetComponent<GamePlayManager>();
        RegistereGamePlayManagerEvent();
        StartCoroutine(WaitForDataLoadAnhProcess());
        GetTotalWave();
        GetCurrentWaveBeign();
        HandleGoldChange();
    }

    private void  RegistereGamePlayManagerEvent()
    {
        gamePlayManager.OnSelectedTowerForUI += HandeSelectedTower;
        gamePlayManager.OnGoldChangeForUI += HandleGoldChange;
        gamePlayManager.spawnEnemyManager.OnUpdateCurrentWave += HandleUpdateCurrentWave;
    }
    private void GetTotalWave()
    {
        totalWaveText.text = "OF " + gamePlayManager.spawnEnemyManager.TotalWave.ToString();
    }

    private void GetCurrentWaveBeign()
    {
        currentWaveText.text = "0";
    }

    private void HandleUpdateCurrentWave(int currentWave)
    {
        currentWaveText.text = currentWave.ToString();
    }

    private void HandleGoldChange()
    {
        goldText.text = gamePlayManager.gold.ToString();
    }
    private IEnumerator WaitForDataLoadAnhProcess()
    {
        yield return new WaitUntil(() => gamePlayManager.IsDataLoaded);
        SetupTowerInitGoldText();
    }
    private void SetupTowerInitGoldText()
    {
        ArcherTowerInitGoldText.text = gamePlayManager.archerTOwerInitGold.ToString();
        mageTowerInitGoldText.text = gamePlayManager.mageTowerInitGold.ToString();
        barrackTowerInitGoldText.text = gamePlayManager.barrackTowerInitGold.ToString();
        cannonTowerInitGoldText.text = gamePlayManager.cannonTowerInitGold.ToString();
    }

    private void HandeSelectedTower()
    {
        upgradeTowerGoldText.text = gamePlayManager.GetTowerGoldUpgrade().ToString();
        sellTowerGoldText.text = gamePlayManager.GetTowerGoldRefund().ToString();
    }
}
