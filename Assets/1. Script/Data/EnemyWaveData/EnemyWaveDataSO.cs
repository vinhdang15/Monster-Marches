using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveDataList", menuName = "EnemyWave Config/EnemyWaveDataSO", order = 1)]
public class EnemyWaveDataSO : ScriptableObject
{
    public List<EnemyWaveData> EnemyWaveDatas = new();
}
