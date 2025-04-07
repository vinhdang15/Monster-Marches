using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataList", menuName = "MapData Config/MapDataList", order = 1)]
public class MapDataListSO : ScriptableObject
{
    public List<MapData> mapDataList = new List<MapData>();
}
