using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{
    private UnitPool unitPool;
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
    
    private void Awake()
    {
        GetUnitPool();
    }

    private void Start()
    {
        GetTimeBetweenEnemy();
        CheckShowFristWaveCaution();
    }

    private void OnEnable()
    {
        RegisterStartNextWaveEvent();
    }

    private void OnDisable()
    {
        UnregisterStartNextWaveEvent();
    }

    private void GetUnitPool()
    {
        unitPool = enemyManager.unitPool;
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
                GetUnitBase(enemyEntries[y].enemy, i);
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

    private void GetUnitBase(Enemy _enemy, int lineInPathIndex)
    {
        Enemy enemy = unitPool.GetEnemy(_enemy.UnitName);
        // add path to enemy pathway
        enemy.GetPathConfigSO(pathConfigSO);
        enemy.SetPosInPathWave(lineInPathIndex % 3);
        enemyManager.AddEnemy(enemy);
    }
    
    private float SetTimeBetweenEnemy()
    {
        return Random.Range(timeBetweenEnemy * 0.3f, timeBetweenEnemy * 2.5f);
    }

    public void RegisterStartNextWaveEvent()
    {
        spawnEnemyManager = GetComponentInParent<SpawnEnemyManager>();
        spawnEnemyManager.OnCallNextWave += StartNextWave;
    }

    public void UnregisterStartNextWaveEvent()
    {
        spawnEnemyManager.OnCallNextWave -= StartNextWave;
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
