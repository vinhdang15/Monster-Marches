using System.Collections.Generic;
using UnityEngine;

public class DecorObjDataReader : MonoBehaviour
{
    public static DecorObjDataReader    Instance { get; private set; }
    public DecorObjDataListSO           decorObjDataListSO;

    public void PrepareGame()
    {
        decorObjDataListSO.decorObjDatas = JSONManager.decorObjDataList;
    }

    public List<DecorObjectInfo> GetDecorObjectInfoList(MapData mapData)
    {
        List<DecorObjectInfo> posList = new();
        int mapID = mapData.mapID;

        foreach(var decorObjdata in decorObjDataListSO.decorObjDatas)
        {
            if(decorObjdata.mapID != mapID) continue;
            posList = decorObjdata.decorObjectInforList;
            break;
        }
        return posList;
    }
}
