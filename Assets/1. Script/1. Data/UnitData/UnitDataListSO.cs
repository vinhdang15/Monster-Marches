using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDataList", menuName = "Data Config/UnitDataList", order = 4)]
public class UnitDataListSO : ScriptableObject
{
    public List<UnitData> unitDataList = new List<UnitData>();

    public UnitData GetUnitData(string unitName)
    {
        return unitDataList.Find(data => data.unitName == unitName);
    }

    public string GetUnitType(string unitName)
    {
        UnitData unitData = unitDataList.Find(data => data.unitName == unitName);
        return unitData.unitType;
    }
}
