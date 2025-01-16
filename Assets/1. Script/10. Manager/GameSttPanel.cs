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

    public void ResetCurrentWave()
    {
        currentWaveText.text = "0";
        currentWaveText.text = "WAVE 0";
    }
    public void GetTotalWave(int i)
    {
        // totalWaveText.text = "OF " + gamePlayManager.spawnEnemyManager.TotalWave.ToString();
        totalWaveText.text = "/" + i.ToString();
    }
    public void UpdateLive(int i )
    {
        // livesText.text = gamePlayManager.live.ToString();
        livesText.text = i.ToString();
    }

    public void UpdateGold(int i)
    {
        // goldText.text = gamePlayManager.gold.ToString();
        goldText.text = i.ToString();
    }

    public void HandleUpdateCurrentWave(int currentWave)
    {
        currentWaveText.text = "WAVE " + currentWave.ToString();
    }
}
