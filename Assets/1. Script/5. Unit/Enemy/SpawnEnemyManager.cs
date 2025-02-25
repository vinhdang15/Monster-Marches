using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : MonoBehaviour
{
    // pathway infor
    public List<SpawnEnemy> SpawnEnemies;
    private int NumberPathWay => SpawnEnemies.Count;

    // Wave infor
    public int TotalWave = 0;
    private int CurrentWaveIndex { get; set; } = 0;
    public int CurrentWave => CurrentWaveIndex + 1;
    public int totalEnemies = 0;

    // spawn enemy infor
    // add 1 if any pathway finish spawn enemy in current wave
    private int finishedSpawnEnemiesCount = 0;
    [SerializeField] private float timeBetweenEnemy;
    [SerializeField] private float timeWaitForNextWave = 3f;

    // Init BtnCautionSlider
    [SerializeField] CautionManager cautionManager;

    // Time show BtnCaution, time call next wave
    private float waitCautionStartTime;
    private Coroutine WaitToCallNextWave;
    public event Action OnCallNextWave;
    public event Action      OnCautionClick;
    public event Action<int> OnUpdateCurrentWave;
    public event Action<int> OnAddGoldWhenCautionClick;

    // trigger to call the first wave when BtnCautionSlider click
    private bool isBeginFristWave = false;

    public void PrepareGame()
    {
        LoadComponents();
        FindAllSpawnEnemy();
        RegisterFinishCurrentWaveEvent();
        GetTotalWave();
        GetTotalEnemies();
    }

    private void OnDisable()
    {
        UnregisterFinishCurrentWaveEvent();
    }

    private void LoadComponents()
    {
        cautionManager = FindObjectOfType<CautionManager>();
    }

    #region SPAWN ENEMY
    private void FindAllSpawnEnemy()
    {
        SpawnEnemy[] allSpawnEnemy = FindObjectsOfType<SpawnEnemy>();
        SpawnEnemies = new List<SpawnEnemy>(allSpawnEnemy);
    }

    private void RegisterFinishCurrentWaveEvent()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            spawnEnemy.OnFinishCurrentWave += HandleFinishCurrentWave;
        }
    } 

    private void UnregisterFinishCurrentWaveEvent()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            spawnEnemy.OnFinishCurrentWave -= HandleFinishCurrentWave;
        }
    }

    // after all enemies in current wave had spawn, start WaitToCallNextWaveCoroutine
    // to call next enemies
    private void HandleFinishCurrentWave()
    {
        if(CurrentWave == TotalWave) return;
        finishedSpawnEnemiesCount++;
        if(finishedSpawnEnemiesCount == NumberPathWay)
        {
            finishedSpawnEnemiesCount = 0;
            WaitToCallNextWave = StartCoroutine(WaitToCallNextWaveCoroutine());
        }
    }
    
    private IEnumerator WaitToCallNextWaveCoroutine()
    {
        yield return new WaitForSeconds(2f);
        CheckToShowCautionBtnInWhichSpawnEnemy();
        waitCautionStartTime = Time.time;
        yield return new WaitForSeconds(timeWaitForNextWave);
        waitCautionStartTime = 0;
        HideAllCautionFill();
        OnCallNextWave?.Invoke();
        UpdateCurrentWaveIndex();

    }
    #endregion

    #region CAUTION SLIDER
    private void CheckToShowCautionBtnInWhichSpawnEnemy()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            if(spawnEnemy.GetNumberEnemyInWave(CurrentWaveIndex + 1) != 0)
            {
                // spawnEnemy.cautionBtn.gameObject.SetActive(true);
                spawnEnemy.cautionBtn.StartActiveCautionFill();
            }
            else
            {
                // spawnEnemy.cautionBtn.gameObject.SetActive(false);
                spawnEnemy.cautionBtn.HideCautionFill();
            }
        }
    }

    private void HideAllCautionFill()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            // spawnEnemy.cautionBtn.gameObject.SetActive(false);
            spawnEnemy.cautionBtn.HideCautionFill();
        }
    }

    public void CautionClick()
    {
        if(!isBeginFristWave)
        {
            foreach(var spawnEnemy in SpawnEnemies)
            {
                spawnEnemy.StartSpawnEnemyCoroutine();
            }
            isBeginFristWave = true;
            OnUpdateCurrentWave?.Invoke(CurrentWave);
        }

        if(WaitToCallNextWave != null)
        {
            StopCoroutine(WaitToCallNextWave);
            WaitToCallNextWave = null;
            // call next wave

            OnCallNextWave?.Invoke();

            // update gold when call next wave early
            HandleCautionClick();
            UpdateCurrentWaveIndex();
        }
        OnCautionClick?.Invoke();
    }

    private void HandleCautionClick()
    {
        float elapsedTime = Time.time - waitCautionStartTime;
        float timeCallEarly = timeWaitForNextWave - elapsedTime;
        int goldCallEarly = (int)timeCallEarly*2;

        foreach(var spawnEnemy in SpawnEnemies)
        {
            if(spawnEnemy.cautionBtn.gameObject.activeSelf)
            {
                OnAddGoldWhenCautionClick?.Invoke(goldCallEarly);
            }
        }
    }
    #endregion

    private void UpdateCurrentWaveIndex()
    {
        if(CurrentWaveIndex >= TotalWave) return;
        CurrentWaveIndex++;
        // update UI current wave
        OnUpdateCurrentWave?.Invoke(CurrentWave);
    }

    public float GetTimeBetweenEnemy()
    {
        return timeBetweenEnemy;
    }

    public float GetTimeWaitForNextWave()
    {
        return timeWaitForNextWave;
    }

    private void GetTotalWave()
    {
        TotalWave = SpawnEnemies[0].GetTotalWave();
    }

    private void GetTotalEnemies()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            foreach(var enemyEntry in spawnEnemy.enemyEntries)
            {
                totalEnemies += enemyEntry.numberEnemyInWave;
            }
        }   
    }
}