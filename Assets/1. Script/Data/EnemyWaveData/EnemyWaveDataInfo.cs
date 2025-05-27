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
                            waveNumber              = 1,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 4,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.7f,
                            timeWaitForNextWave     = 10f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 2,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 6,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.5f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 3,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 6,
                            secondaryEnemyID        = UnitID.Enemy_B_1.ToString(),
                            secondaryEnemyCount     = 2,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 4,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 6,
                            secondaryEnemyID        = UnitID.Enemy_B_1.ToString(),
                            secondaryEnemyCount     = 4,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 5,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 2,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 10f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 6,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 10,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 0.9f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 7,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 10,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 4,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
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
                            waveNumber              = 1,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 4,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.7f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 2,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.4f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 3,
                            primaryEnemyID          = UnitID.Enemy_B_1.ToString(),
                            primaryEnemyCount       = 4,
                            secondaryEnemyID        = UnitID.Enemy_C_1.ToString(),
                            secondaryEnemyCount     = 4,
                            timeBetweenEachSpawn    = 1.4f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 4,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 4,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.8f,
                            timeWaitForNextWave     = 10f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 5,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 6,
                            primaryEnemyID          = UnitID.none.ToString(),
                            primaryEnemyCount       = 0,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 7,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 1,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 5,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 3,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
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
                            waveNumber              = 1,
                            primaryEnemyID          = UnitID.none.ToString(),
                            primaryEnemyCount       = 0,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 2,
                            primaryEnemyID          = UnitID.none.ToString(),
                            primaryEnemyCount       = 0,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 3,
                            primaryEnemyID          = UnitID.none.ToString(),
                            primaryEnemyCount       = 0,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 4,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 3,
                            secondaryEnemyID        = UnitID.Enemy_B_1.ToString(),
                            secondaryEnemyCount     = 2,
                            timeBetweenEachSpawn    = 1.4f,
                            timeWaitForNextWave     = 10f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 5,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 6,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 1,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 7,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 7,
                            secondaryEnemyID        = UnitID.none.ToString(),
                            secondaryEnemyCount     = 0,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 7f,
                        },
                        new EnemyWave
                        {
                            waveNumber              = 8,
                            primaryEnemyID          = UnitID.Enemy_C_1.ToString(),
                            primaryEnemyCount       = 5,
                            secondaryEnemyID        = UnitID.Enemy_A_1.ToString(),
                            secondaryEnemyCount     = 2,
                            timeBetweenEachSpawn    = 1.2f,
                            timeWaitForNextWave     = 5f,
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
    public int      waveNumber;
    public string   primaryEnemyID;
    public int      primaryEnemyCount;
    public string   secondaryEnemyID;
    public int      secondaryEnemyCount;
    public float    timeBetweenEachSpawn;
    public float    timeWaitForNextWave;
}