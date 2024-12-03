using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDataList", menuName = "Data Config/UnitDataList", order = 4)]
public class UnitDataListSO : ScriptableObject
{
    public List<UnitData> enemyDataList = new List<UnitData>();

    public UnitData GetUnitData(string enemyName)
    {
        
        return enemyDataList.Find(data => data.unitName == enemyName);
    }
}
