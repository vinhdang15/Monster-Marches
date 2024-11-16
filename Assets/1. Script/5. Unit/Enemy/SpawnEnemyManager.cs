using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : MonoBehaviour
{
    public List<SpawnEnemy> SpawnEnemies { get; set; }
    private int NumberPathWay => SpawnEnemies.Count;
    public int TotalWave => SpawnEnemies[0].GetTotalWave();
    private int CurrentWaveIndex { get; set; } = 0;
    public int CurrentWave => CurrentWaveIndex + 1;
    private int finishedSpawnEnemiesCount = 0;
    [SerializeField] private float timeBetweenEnemy;
    [SerializeField] private float timeWaitForNextWave = 3f;
    private float waitCautionStartTime;
    private Coroutine WaitToCallNextWave;
    public event Action OnCallNextWave;
    public event Action<int> OnUpdateCurrentWave;
    public event Action<float> OnCautionClick;
    private bool isBeginFristWave = false;

    private void Awake()
    {
        GetSpawnEnemies();
    }
    private void Start()
    {
        RegisterFinishCurrentWaveEvent();
    }
    private void GetSpawnEnemies()
    {
        SpawnEnemy[] children =  GetComponentsInChildren<SpawnEnemy>();
        SpawnEnemies = new List<SpawnEnemy>(children);
    }

    private void RegisterFinishCurrentWaveEvent()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            spawnEnemy.OnFinishCurrentWave += HandleFinishCurrentWave;
        }
    }

    private void HandleFinishCurrentWave()
    {
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
        CheckToShowCautionSliderInWhichSpawnEnemy();
        waitCautionStartTime = Time.time;
        yield return new WaitForSeconds(timeWaitForNextWave);
        waitCautionStartTime = 0;
        HideAllCautionSlider();
        
    }

    private void CheckToShowCautionSliderInWhichSpawnEnemy()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            if(spawnEnemy.GetNumberEnemyInNWave(CurrentWaveIndex + 1) != 0)
            {
                spawnEnemy.btnCautionSlider.gameObject.SetActive(true);
            }
            else
            {
                spawnEnemy.btnCautionSlider.gameObject.SetActive(false);
            }
            //Debug.Log($"Next Wave {CurrentWave + 1} => {spawnEnemy.name}: {spawnEnemy.enemyEntries[CurrentWave + 1].numberEnemyInWave}");
        }
    }

    private void HideAllCautionSlider()
    {
        foreach(var spawnEnemy in SpawnEnemies)
        {
            spawnEnemy.btnCautionSlider.gameObject.SetActive(false);
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

            HankdleCautionClick();
            // update UI curent wave
            CurrentWaveIndex++;
            OnUpdateCurrentWave?.Invoke(CurrentWave);
        }
    }

    private void HankdleCautionClick()
    {
        float elapsedTime = Time.time - waitCautionStartTime;
        float timeCallEarly = timeWaitForNextWave - elapsedTime;

        foreach(var spawnEnemy in SpawnEnemies)
        {
            if(spawnEnemy.btnCautionSlider.gameObject.activeSelf)
            {
                OnCautionClick?.Invoke(timeCallEarly);
            }
        }
    }

    public float GetTimeBetweenEnemy()
    {
        return timeBetweenEnemy;
    }

}
