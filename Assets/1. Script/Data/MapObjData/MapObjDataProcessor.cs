using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjDataProcessor : MonoBehaviour
{
    // Read data from input (MapObjCollector) to create MapObjData
    [SerializeField] MapObjCollector mapObjCollector;

    private void Awake()
    {
        mapObjCollector = GetComponent<MapObjCollector>();
    }

    public List<MapObjData> ExtractMapObjData()
    {
        List<MapObjData> mapObjDataList = new();
        foreach(GameDecorObjList mapObj in mapObjCollector.gameDecorObjList)
        {
            MapObjData data = new()
            {
                mapID = mapObj.mapID,
                decorObjectInforList = ExtractTreePatchInfoList(mapObj.mapDecorObjHolderList),
            };
            mapObjDataList.Add(data);
        } 
        return mapObjDataList;
    }

    private List<DecorObjectInfo> ExtractTreePatchInfoList(List<MapDecorHolderObj> mapTreePatchList)
    {
        List<DecorObjectInfo> treePatchDataList = new();
        foreach(var treePatch in mapTreePatchList)
        { 
            DecorObjectInfo treePatchInfo = new()
            {
                decorID = treePatch.decorObjID,
                decorObjectPosList = ExtractChildPos(treePatch.decorObjHolder),
            };

            treePatchDataList.Add(treePatchInfo);
        }
        return treePatchDataList;
    }

    private List<Vector2> ExtractChildPos(Transform holder)
    {
        List<Vector2> pos = new();
        foreach(Transform child in holder)
        {
            Vector2 childPos =  child.transform.position;
            pos.Add(childPos);
        }
        return pos;
    }
}
