using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataList", menuName = "MapData Config/MapDataList", order = 1)]
public class MapDataListSO : ScriptableObject
{
    public List<MapData> mapDataList = new List<MapData>();

    // public List<Vector2> GetInitMapPos()
    // {
    //     List<Vector2> posList = new List<Vector2>();
    //     foreach(MapData mapData in mapDataList)
    //     {
    //         Vector2 pos = mapData.position.ToVector2();
    //         posList.Add(pos);
    //     }
    //     return posList;
    // }

    // public void Test()
    // {
    //     Debug.Log(mapDataList[0].initPos.ToVector2());
    // }
}
