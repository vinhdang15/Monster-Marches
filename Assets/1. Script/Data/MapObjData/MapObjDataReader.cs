using System.Collections.Generic;
using UnityEngine;

public class MapObjDataReader : MonoBehaviour
{
    public static MapObjDataReader    Instance { get; private set; }
    public MapObjDataListSO           mapObjDataListSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        mapObjDataListSO.mapObjDatas = JSONManager.mapObjDataList;
    }

    public List<DecorObjectInfo> GetDecorObjectInfoList(MapData mapData)
    {
        List<DecorObjectInfo> posList = new();
        int mapID = mapData.mapID;

        foreach(var mapObjdata in mapObjDataListSO.mapObjDatas)
        {
            if(mapObjdata.mapID != mapID) continue;
            posList = mapObjdata.decorObjectInforList;
            break;
        }
        return posList;
    }
}
