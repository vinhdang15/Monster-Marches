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
    private List<EnemyWave>                 enemyWaveList = new();

    public BtnCaution                       cautionBtn;
    private Vector2                         cautionBtnPos;
    private EnemySpawnerManager             enemySpawnerManager;
    private bool                            isStartNextWave = false;
    public event Action<float>              OnFinishCurrentWave;
    private Coroutine SpawnEnemyCoroutine;

    public void PrepareGame(EnemyManager enemyManager, EnemySpawnerManager enemySpawnerManager,
                            Vector2 cautionBtnPos, int pathID, MapData mapData,
                            EnemyWaveDataReader enemyWaveDataReader,
                            List<PathWaySegment> pathWaySegmentList)
    {
        LoadComponents(enemyManager);
        SetEnemySpawnerManager(enemySpawnerManager);
        SetCautionBtnPos(cautionBtnPos);
        GetEnemyWaveDataList(pathID, mapData, enemyWaveDataReader);
        SetPathWaySegmentList(pathWaySegmentList);
        RegisterStartNextWaveEvent();
    }

    public void LoadComponents(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
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

    private void GetEnemyWaveDataList(int pathID, MapData mapData, EnemyWaveDataReader enemyWaveDataReader)
    {

        enemyWaveList = enemyWaveDataReader.GetSelectedMapEnemyWaveList(pathID, mapData);
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
        for(int y = 0; y < enemyWaveList.Count; y++)
        {
            // for loop to spawn primary enemies in one wave
            for(int i = 0; i < enemyWaveList[y].primaryEnemyCount; i++)
            {
                GetUnitBase(enemyWaveList[y].primaryEnemyID);
                // the waiting time between each enemy instantiate
                yield return new WaitForSeconds(SetTimeBetweenEnemy(enemyWaveList[y].timeBetweenEachSpawn));
            }

            // for loop to spawn secondary enemies in one wave
            for(int i = 0; i < enemyWaveList[y].secondaryEnemyCount; i++)
            {
                GetUnitBase(enemyWaveList[y].secondaryEnemyID);
                // the waiting time between each enemy instantiate
                yield return new WaitForSeconds(SetTimeBetweenEnemy(enemyWaveList[y].timeBetweenEachSpawn));
            }
            isStartNextWave = false;
            OnFinishCurrentWave?.Invoke(enemyWaveList[y].timeWaitForNextWave);
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
        return waveIndex < enemyWaveList.Count ? enemyWaveList[waveIndex].primaryEnemyCount : 0;
    }

    public bool HasEnemyInWave(int waveNumber)
    {
        return GetNumberEnemyInWave(waveNumber) > 0;
    }

    private void GetUnitBase(string enemyID)
    {
        Enemy enemy = UnitPool.Instance.GetUnitBase(enemyID) as Enemy;
        if (enemy == null) return;
        enemy.PrepareGame(pathWaySegmentList, SetRandomIndex());
        enemyManager.AddEnemy(enemy);
    }

    private int SetRandomIndex()
    {
        return Random.Range(0, 3);
     }
    
    private float SetTimeBetweenEnemy(float timeBetweenEachSpawn)
    {
        return Random.Range(timeBetweenEachSpawn + 0.1f, timeBetweenEachSpawn - 0.1f);
    }

    private void StartNextWave()
    {
        isStartNextWave = true;
    }

    public int GetTotalWave()
    {
        return enemyWaveList.Count;
    }

    public void SetCautionBtm(BtnCaution cautionBtn)
    {
        this.cautionBtn = cautionBtn;
    }
}
