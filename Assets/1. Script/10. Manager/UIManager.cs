using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GamePlayManager gamePlayManager;
    [Header("Game Status")]
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI totalWaveText;
    [SerializeField] TextMeshProUGUI currentWaveText;
    
    [Header("Panel Gold Text")]
    [SerializeField] TextMeshProUGUI ArcherTowerInitGoldText;
    [SerializeField] TextMeshProUGUI mageTowerInitGoldText;
    [SerializeField] TextMeshProUGUI barrackTowerInitGoldText;
    [SerializeField] TextMeshProUGUI cannonTowerInitGoldText;
    [SerializeField] TextMeshProUGUI upgradeTowerGoldText;
    [SerializeField] TextMeshProUGUI sellTowerGoldText;

    [Header("Game menu")]
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject victoryMenu;
    [SerializeField] GameObject pauseMenu;
    
    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        StartCoroutine(WaitForDataLoadAnhProcess());
        GetTotalWave();
        GetCurrentWaveBeign();
        HandleGoldChange();
        HandleLiveChange();
    }
    private void OnEnable()
    {
        RegisterGamePlayManagerEvent();
        Instance = this;
    }
    private void OnDisable()
    {
        UnregisterGamePlayManagerEvent();
        Instance = null;
    }
    private void RegisterGamePlayManagerEvent()
    {
        gamePlayManager.OnGoldChangeForUI += HandleGoldChange;
        gamePlayManager.OnLiveChangeForUI += HandleLiveChange;
        gamePlayManager.OnSelectedTowerForUI += HandleSelectedTower;
        gamePlayManager.spawnEnemyManager.OnUpdateCurrentWave += HandleUpdateCurrentWave;
    }

    private void UnregisterGamePlayManagerEvent()
    {
        gamePlayManager.OnGoldChangeForUI -= HandleGoldChange;
        gamePlayManager.OnLiveChangeForUI -= HandleLiveChange;
        gamePlayManager.OnSelectedTowerForUI -= HandleSelectedTower;
        gamePlayManager.spawnEnemyManager.OnUpdateCurrentWave -= HandleUpdateCurrentWave;
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

    private void HandleLiveChange()
    {
        livesText.text = gamePlayManager.live.ToString();
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

    private void HandleSelectedTower()
    {
        upgradeTowerGoldText.text = gamePlayManager.GetTowerGoldUpgrade().ToString();
        sellTowerGoldText.text = gamePlayManager.GetTowerGoldRefund().ToString();
    }

    #region GAME MENU
    void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void ShowVictoryMenu()
    {
        victoryMenu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    void HideAllPanel()
    {
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }
    #endregion

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumingGame()
    {
        Time.timeScale = 1;
    }
}
