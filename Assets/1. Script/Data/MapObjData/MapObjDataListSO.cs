using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapObjDataList", menuName = "MapObj Config/MapObjDataListSO", order = 1)]
public class MapObjDataListSO : ScriptableObject
{
    public List<MapObjData> mapObjDatas = new();
}
