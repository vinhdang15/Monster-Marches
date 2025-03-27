using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDataList", menuName = "Data Config/UnitDataList", order = 4)]
public class UnitDataListSO : ScriptableObject
{
    public List<UnitData> unitDataList = new List<UnitData>();

    public UnitData GetUnitData(string unitId)
    {
        return unitDataList.Find(data => data.id == unitId);
    }

    public string GetUnitType(string unitId)
    {
        UnitData unitData = unitDataList.Find(data => data.id == unitId);
        return unitData.unitType;
    }

    public int GetUnitDamage(string unitId)
    {
        UnitData unitData = unitDataList.Find(data => data.id == unitId);
        return unitData.attackDamage;
    }
}
