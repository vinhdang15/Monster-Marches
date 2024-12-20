using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSttPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI totalWaveText;
    [SerializeField] TextMeshProUGUI currentWaveText;
    [HideInInspector] public GamePlayManager gamePlayManager;

    public void GetTotalWave()
    {
        totalWaveText.text = "OF " + gamePlayManager.spawnEnemyManager.TotalWave.ToString();
    }
    public void UpdateLive()
    {
        livesText.text = gamePlayManager.live.ToString();
    }

    public void UpdateGold()
    {
        goldText.text = gamePlayManager.gold.ToString();
    }

     public void GetCurrentWaveBeign()
    {
        currentWaveText.text = "0";
    }

    public void HandleUpdateCurrentWave(int currentWave)
    {
        currentWaveText.text = currentWave.ToString();
    }
}
