using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WayPointDataList", menuName = "WayPoint Config/WayPointDataSO", order = 1)]
public class WayPointDataListSO : ScriptableObject
{
    public List<WayPointData> wayPointDatas = new();
}
