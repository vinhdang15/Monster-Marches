using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataList", menuName = "ScriptableObjects/TowerDataList", order = 1)]
public class TowerDataList : ScriptableObject
{
    public List<TowerData> towerDataList = new List<TowerData>();

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
}