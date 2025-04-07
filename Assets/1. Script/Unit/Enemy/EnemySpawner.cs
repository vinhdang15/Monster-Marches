using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyManager   enemyManager;
 
    [Header("Pathway to Spawn Enemy")]
    private List<PathWaySegment>            pathWaySegmentList;
    
    [Header("Enemy-Wave information")]
    public List<EnemyEntry>                 enemyEntries = new()
    {
        new EnemyEntry
        {
            enemyID = "Monster_1",
            numberEnemyInWave = 1,
        }
    };
    private float                           timeBetweenEnemy;
    public BtnCaution                       cautionBtn;
    private Vector2                         cautionBtnPos;
    private EnemySpawnerManager             enemySpawnerManager;
    private bool                            isStartNextWave = false;
    public event Action                     OnFinishCurrentWave;
    private Coroutine SpawnEnemyCoroutine;

    public void PrepareGame(EnemySpawnerManager enemySpawnerManager, Vector2 cautionBtnPos, List<PathWaySegment> pathWaySegmentList)
    {
        LoadComponents();
        SetEnemySpawnerManager(enemySpawnerManager);
        SetCautionBtnPos(cautionBtnPos);
        SetPathWaySegmentList(pathWaySegmentList);
        GetTimeBetweenEnemy();
        RegisterStartNextWaveEvent();
    }

    public void LoadComponents()
    {
        enemyManager = GameObject.Find(InitNameObject.EnemyManager.ToString()).GetComponent<EnemyManager>();
    }

    private void SetEnemySpawnerManager(EnemySpawnerManager enemySpawnerManager)
    {
        this.enemySpawnerManager = enemySpawnerManager;
    }

    private void SetCautionBtnPos(Vector2 pos)
    {
        cautionBtnPos = pos;
    }

    public Vector2 GetCautionBtnPos()
    {
        return cautionBtnPos;
    }

    private void SetPathWaySegmentList(List<PathWaySegment> pathWaySegmentList)
    {
        this.pathWaySegmentList = pathWaySegmentList;
    }

    private void GetTimeBetweenEnemy()
    {
        timeBetweenEnemy = enemySpawnerManager.GetTimeBetweenEnemy();
    }

    public void RegisterStartNextWaveEvent()
    {
        enemySpawnerManager.OnCallNextWave += StartNextWave;
    }

    public void UnregisterStartNextWaveEvent()
    {
        enemySpawnerManager.OnCallNextWave -= StartNextWave;
    }
    
    private IEnumerator SpawnEnemy()
    {
        // for loop to spwan enemies in all wave
        for(int y = 0; y < enemyEntries.Count; y++)
        {
            // for loop to spawn enemies in one wave
            for(int i = 0; i < enemyEntries[y].numberEnemyInWave; i++)
            {
                GetUnitBase(enemyEntries[y].enemyID, i);
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
        if(SpawnEnemyCoroutine !=null)
        {
            StopCoroutine(SpawnEnemyCoroutine);
        }
        SpawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    private int GetNumberEnemyInWave(int waveIndex)
    {
        return waveIndex < enemyEntries.Count ? enemyEntries[waveIndex].numberEnemyInWave : 0;
    }

    public bool HasEnemyInWave(int waveNumber)
    {
        return GetNumberEnemyInWave(waveNumber) > 0;
    }

    private void GetUnitBase(string enemyID, int pathWaySegmentIndex)
    {
        string unitPrefabName = enemyID;
        Enemy enemy = UnitPool.Instance.GetEnemy(unitPrefabName);
        enemy.PrepareGame(pathWaySegmentList, pathWaySegmentIndex % 3);
        enemyManager.AddEnemy(enemy);
    }
    
    private float SetTimeBetweenEnemy()
    {
        return Random.Range(timeBetweenEnemy * 0.3f, timeBetweenEnemy * 2.5f);
    }

    private void StartNextWave()
    {
        isStartNextWave = true;
    }

    public int GetTotalWave()
    {
        return enemyEntries.Count;
    }

    public void SetCautionBtm(BtnCaution cautionBtn)
    {
        this.cautionBtn = cautionBtn;
    }
}

[System.Serializable]
public class EnemyEntry
{
    public string enemyID;
    public int numberEnemyInWave;
}
