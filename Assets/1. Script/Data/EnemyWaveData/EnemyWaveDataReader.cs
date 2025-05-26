using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveDataReader : MonoBehaviour
{
    public EnemyWaveDataSO                  enemyWaveDataSO;

    public void PrepareGame()
    {
        enemyWaveDataSO.EnemyWaveDataList = JSONDataLoader.enemyWaveDataList;
    }

    public EnemyWaveData GetSelectedMapEnemyWaveData(MapData mapData)
    {
        foreach(EnemyWaveData enemyWaveData in enemyWaveDataSO.EnemyWaveDataList)
        {
            if(enemyWaveData.mapID == mapData.mapID) return enemyWaveData;
        }
        return null;
    }

    public int GetTotalPathWayWave(EnemyWaveData enemyWaveData)
    {
        return enemyWaveData.pathWayWaveList.Count;
    }

    public int GetTotalSelectedMapEnemies(MapData mapData)
    {
        int totalEnemies = 0;
        EnemyWaveData enemyWaveData = GetSelectedMapEnemyWaveData(mapData);
        foreach(PathWayWave pathWayEnemyWave in enemyWaveData.pathWayWaveList)
        {
            foreach (EnemyWave enemyWave in pathWayEnemyWave.EnemyWaveList)
            {
                totalEnemies += enemyWave.primaryEnemyCount;
                totalEnemies += enemyWave.secondaryEnemyCount;
            }
        }
        return totalEnemies;
    }

    public List<EnemyWave> GetSelectedMapEnemyWaveList(int pathID, MapData mapData)
    {

        EnemyWaveData enemyWaveData = GetSelectedMapEnemyWaveData(mapData);
        foreach(PathWayWave pathWayWave in enemyWaveData.pathWayWaveList)
        {
            if(pathWayWave.pathID == pathID) return pathWayWave.EnemyWaveList;
        }
        return null;
    }
}
