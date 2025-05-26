using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WayPointDataReader : MonoBehaviour
{
    public static WayPointDataReader    Instance { get; private set; }
    public WayPointDataListSO           wayPointDataListSO;

    public void PrepareGame()
    {
        wayPointDataListSO.wayPointDatas = JSONDataLoader.wayPointDataList;
    }

    public List<Vector2> GetSelectedMapEmptyPlotPos(MapData mapData)
    {
        List<Vector2> emptyPlotDataList = new();
        int mapID = mapData.mapID;
        foreach(var wpdata in wayPointDataListSO.wayPointDatas)
        {
            if(wpdata.mapID != mapID) continue;
            emptyPlotDataList = wpdata.emptyPlotPosList;
            break;
        }
        return emptyPlotDataList;
    }

    public List<Vector2> GetEndPointPos(MapData mapData)
    {
        List<Vector2> endPointPosList = new();
        int mapID = mapData.mapID;
        foreach(var wpData in wayPointDataListSO.wayPointDatas)
        {
            if(wpData.mapID != mapID) continue;
            endPointPosList = wpData.endPointPosList;
            break;
        }
        return endPointPosList;
    }

    public List<Vector2> GetInitGuardPointPosList(MapData mapData)
    {
        List<Vector2> initGuardPointPosList = new();
        int mapID = mapData.mapID;
        foreach(var wpData in wayPointDataListSO.wayPointDatas)
        {
            if(wpData.mapID != mapID) continue;
            initGuardPointPosList = wpData.initGuardPointPosList;
            break;
        }
        return initGuardPointPosList;
    }
    
    public List<MainPathWayInfo> GetMainPathWayInfoList(MapData mapData)
    {
        List<MainPathWayInfo> pathWayDataList = new();
        int mapID = mapData.mapID;
        foreach(var wpData in wayPointDataListSO.wayPointDatas)
        {
            if(wpData.mapID != mapID) continue;
            pathWayDataList = wpData.mainPathWayInforList;
            break;
        }
        return pathWayDataList;
    }
}