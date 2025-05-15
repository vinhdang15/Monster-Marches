using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataList", menuName = "Data Config/TowerDataList", order = 1)]
public class TowerDataListSO : ScriptableObject
{
    public List<TowerData> towerDataList = new List<TowerData>();

    public List<int> TowerInitGoldList = new List<int>();

    public TowerData GetTowerData(string towerType, int level)
    {
        return towerDataList.Find(data => data.towerType == towerType && data.level == level);
    }

    public int GetGoldInit(string towerType)
    {
        TowerData tower = towerDataList.Find(data => data.towerType == towerType && data.level == 1);
        return tower.goldRequired;
    }

    public int GetGoldRequired(string towerType, int level)
    {
        TowerData tower = towerDataList.Find(data => data.towerType == towerType && data.level == level);
        if(tower == null) return 0;
        return tower.goldRequired;
    }

    public float GetRangeDetect(string towerType, int level)
    {
        TowerData tower = towerDataList.Find(data => data.towerType == towerType && data.level == level);
        if(tower == null) return 0;
        return tower.rangeDetect;
    }

    public string GetDescription(string towerType, int level)
    {
        TowerData tower = towerDataList.Find(data => data.towerType == towerType && data.level == level);
        if(tower == null) return null;
        return tower.descriptions;
    }

    public float GetSpawnRate(string towerType, int level)
    {
        TowerData tower = towerDataList.Find(data => data.towerType == towerType && data.level == level);
        if(tower == null) return 0;
        return tower.spawnRate;
    }

    public string GetTowerSpawnObject(string towerType, int level)
    {
        TowerData tower = towerDataList.Find(data => data.towerType == towerType && data.level == level);
        if(tower == null) return null;
        return tower.SpawnObject;
    }

    public int GetTowerMaxLevel(string towerType)
    {
        int max = 0;
        foreach(TowerData tower in towerDataList)
        {   
            
            if(tower.towerType == towerType && tower.level > max)
            {
                max = tower.level;
            }
        }
        return max;
    }

}