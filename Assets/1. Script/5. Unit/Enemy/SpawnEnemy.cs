using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private CSVUnitDataReader unitDataReader;
    [SerializeField] private EnemyManager enemyManager;
    [Header("Pathway to Spawn Enemy")]
    [SerializeField] private PathConfigSO pathConfigSO;
    [Header("Enemy-Wave information")]
    public List<EnemyEntry> enemyEntries = new List<EnemyEntry>();
     private float timeBetweenEnemy;
    public BtnCautionSlider btnCautionSlider;
    private SpawnEnemyManager spawnEnemyManager;
    private bool isStartNextWave = false;

    public event Action OnFinishCurrentWave;
    // public delegate void numberEnemyInWaveHandler(BtnCautionSlider btnCautionSlider);
    // public numberEnemyInWaveHandler OnNumberEnemyInWaveIsNull;
    // public numberEnemyInWaveHandler OnNumberEnemyInWave;
    

    private void Start()
    {
        RegisterStartNextWaveEvent();
        GetTimeBetweenEnemy();
        CheckShowFristWaveCaution();
    }

    private void GetTimeBetweenEnemy()
    {
        timeBetweenEnemy = spawnEnemyManager.GetTimeBetweenEnemy();
    }

    
    private IEnumerator SpawnEnemyCoroutine()
    {
        yield return new WaitUntil(() => unitDataReader.IsDataLoaded);
        // Check number enemy in the current way, if none hide the caution button else show the caution button

        // for loop to spwan enemies in all wave
        for(int y = 0; y < enemyEntries.Count; y++)
        {
            // for loop to spawn enemies in one wave
            for(int i = 0; i < enemyEntries[y].numberEnemyInWave; i++)
            {
                InstantiateEnemy(enemyEntries[y].enemy, i);
                
                // wait time among instantiate each enemy
                yield return new WaitForSeconds(SetTimeBetweenEnemy());
            }
            isStartNextWave = false;
            OnFinishCurrentWave?.Invoke();
            yield return new WaitUntil(() => isStartNextWave); 
        }
    }

    public void StartSpawnEnemyCoroutine()
    {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public int GetNumberEnemyInNWave(int waveIndex)
    {
        // if(waveIndex < enemyEntries.Count)
        // {
        //     return enemyEntries[waveIndex].numberEnemyInWave;
        // }
        // else
        // {
        //     return 0;
        // }
        return waveIndex < enemyEntries.Count ? enemyEntries[waveIndex].numberEnemyInWave : 0;
    }

    private void CheckShowFristWaveCaution()
    {
        if(GetNumberEnemyInNWave(0) != 0)
        {
            btnCautionSlider.gameObject.SetActive(true);
        }
        else btnCautionSlider.gameObject.SetActive(false);
    }

    private void InstantiateEnemy(Enemy enemy, int lineInPathIndex)
    {
        UnitData enemyData = unitDataReader.unitDataList.GetEnemyData(enemy.enemyName);
        Enemy enemyIns = Instantiate(enemy, enemyManager.transform);
        enemyIns.InitUnit(enemyData);
        enemyIns.InitState();
        // add path to enemy pathway
        enemyIns.GetPathConfigSO(pathConfigSO);
        enemyIns.SetPosInPathWave(lineInPathIndex % 3);
        enemyManager.AddEnemy(enemyIns);

    }
    private float SetTimeBetweenEnemy()
    {
        return Random.Range(timeBetweenEnemy * 0.5f, timeBetweenEnemy * 2f);
    }

    public void RegisterStartNextWaveEvent()
    {
        spawnEnemyManager = GetComponentInParent<SpawnEnemyManager>();
        spawnEnemyManager.OnCallNextWave += StartNextWave;

    }

    private void StartNextWave()
    {
        isStartNextWave = true;
    }

    public int GetTotalWave()
    {
        return enemyEntries.Count;
    }

    public Vector2 GetCautionPos()
    {
        return pathConfigSO.GetCautionPos();
    }
}

[System.Serializable]
public class EnemyEntry
{
    public Enemy enemy;
    public int numberEnemyInWave;
}
