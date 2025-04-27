using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveDataInfo : MonoBehaviour
{
    private List<EnemyWaveData> EnemyWaveDataList= new()
    {
        new EnemyWaveData
        {
            mapID = 1,
            pathWayWaveList = new()
            {
                new PathWayWave
                {
                    pathID = 1,
                    EnemyWaveList = new()
                    {
                        new EnemyWave
                        {
                            waveNumber = 1,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 2,
                            timeBetweenEachSpawn = 1f,
                            timeWaitForNextWave = 10f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 2,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 2,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 3,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 5,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 4,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 3,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 5,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 5,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                    }
                },
            }
        },
        
        new EnemyWaveData
        {
            mapID = 2,
            pathWayWaveList = new()
            {
                new PathWayWave
                {
                    pathID = 1,
                    EnemyWaveList = new()
                    {
                        new EnemyWave
                        {
                            waveNumber = 1,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 2,
                            timeBetweenEachSpawn = 1f,
                            timeWaitForNextWave = 10f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 2,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 0,
                            timeBetweenEachSpawn = 0f,
                            timeWaitForNextWave = 0f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 3,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 5,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 4,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 3,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 5,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 3,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                    }
                },

                new PathWayWave
                {
                    pathID = 2,
                    EnemyWaveList = new()
                    {
                        new EnemyWave
                        {
                            waveNumber = 1,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 0,
                            timeBetweenEachSpawn = 0f,
                            timeWaitForNextWave = 0f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 2,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 3,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 3,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 0,
                            timeBetweenEachSpawn = 0f,
                            timeWaitForNextWave = 0f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 4,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 5,
                            timeBetweenEachSpawn = 1.2f,
                            timeWaitForNextWave = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber = 5,
                            enemyID = "Monster_1",
                            numberEnemyInWave = 10,
                            timeBetweenEachSpawn = 0.8f,
                            timeWaitForNextWave = 5f,
                        },
                    }
                }
            }  
        }
    };

    public List<EnemyWaveData> GetEnemyWaveDataList()
    {
        return EnemyWaveDataList;
    }
}

[System.Serializable]
public class EnemyWaveData
{
    public int mapID;
    public List<PathWayWave> pathWayWaveList;
}

[System.Serializable]
public class PathWayWave
{
    
    public int pathID;
    public List<EnemyWave> EnemyWaveList;
}

[System.Serializable]
public class EnemyWave
{   
    public int waveNumber;
    public string enemyID;
    public int numberEnemyInWave;
    public float timeBetweenEachSpawn;
    public float timeWaitForNextWave;
}