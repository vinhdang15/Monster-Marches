using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyManager   enemyManager;
    private UnitPool                        unitPool;
    [Header("Pathway to Spawn Enemy")]
    [SerializeField] private PathConfigSO   pathConfigSO;
    [Header("Enemy-Wave information")]
    public List<EnemyEntry>                 enemyEntries = new List<EnemyEntry>();
    private float                           timeBetweenEnemy;
    public BtnCaution                       cautionBtn;
    private EnemySpawnerManager             spawnEnemyManager;
    private bool                            isStartNextWave = false;

    public event Action                     OnFinishCurrentWave;
    
    // private void Awake()
    // {
    //     LoadComponents();
    // }

    // private void Start()
    // {
    //     GetTimeBetweenEnemy();
    //     CheckShowFristWaveCaution();
    // }

    // private void OnEnable()
    // {
    //     RegisterStartNextWaveEvent();
    // }

    public void PrepareGame()
    {
        LoadComponents();
        GetTimeBetweenEnemy();
        CheckShowFristWaveCaution();
        RegisterStartNextWaveEvent();
    }

    private void OnDisable()
    {
        UnregisterStartNextWaveEvent();
    }

    public void LoadComponents()
    {
        enemyManager = GameObject.Find(InitNameObject.EnemyManager.ToString()).GetComponent<EnemyManager>();
        spawnEnemyManager = GameObject.Find(InitNameObject.EnemySpawnerManager.ToString()).GetComponent<EnemySpawnerManager>();
        unitPool = enemyManager.unitPool;
    }

    private void GetTimeBetweenEnemy()
    {
        timeBetweenEnemy = spawnEnemyManager.GetTimeBetweenEnemy();
    }
    
    private IEnumerator SpawnEnemyCoroutine()
    {
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

    public int GetNumberEnemyInWave(int waveIndex)
    {
        return waveIndex < enemyEntries.Count ? enemyEntries[waveIndex].numberEnemyInWave : 0;
    }

    private void CheckShowFristWaveCaution()
    {
        if(GetNumberEnemyInWave(0) != 0)
        {
            cautionBtn.isFirstWave = true;
            cautionBtn.StartActiveCautionFill();
        }
        else cautionBtn.HideCautionFill();
    }

    private void GetUnitBase(Enemy _enemy, int lineInPathIndex)
    {
        string unitPrefabName = _enemy.name.Trim().ToLower();
        Enemy enemy = unitPool.GetEnemy(unitPrefabName);
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
        // spawnEnemyManager = GetComponentInParent<SpawnEnemyManager>();
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
