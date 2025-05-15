using System.Collections.Generic;
using UnityEngine;

public class WayPointDataProcessor : MonoBehaviour
{
    // Read data from input (WayPointCollector) to create WayPointData
    [SerializeField] WayPointCollector wayPointCollector;

    private void Awake()
    {
        wayPointCollector = GetComponent<WayPointCollector>();
    }

    public List<WayPointData> ExtractWayPointData()
    {
        List<WayPointData> wayPointDataList = new();
        foreach(MapWayPoint mapWayPoint in wayPointCollector.mapWayPointList)
        {
            WayPointData data = new()
            {
                mapID = mapWayPoint.mapID,
                endPointPosList = ExtractChildPos(mapWayPoint.endPointHolder),
                emptyPlotPosList = ExtractChildPos(mapWayPoint.emptyPlotHolder),
                initGuardPointPosList = ExtractChildPosFromList(mapWayPoint.initGuardPointPosHolder),
                mainPathWayInforList = ExtractMainPathWayInfoList(mapWayPoint.mainPathWayList)

            };
            wayPointDataList.Add(data);
        } 
        return wayPointDataList;
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

    private List<Vector2> ExtractChildPosFromList(List<Transform> holder)
    {
        List<Vector2> pos = new();
        foreach(Transform tranf in holder)
        {
            foreach(Transform child in tranf)
            {
                Vector2 childPos =  child.transform.position;
                pos.Add(childPos);
            }   
        }
        return pos;
    }

    private List<MainPathWayInfo> ExtractMainPathWayInfoList(List<MainPathWay> mainPathWayList)
    {
        List<MainPathWayInfo> pathWayDataList = new();
        foreach(var mainPathWay in mainPathWayList)
        { 
            MainPathWayInfo mainpathWayInfo = new()
            {
                pathWayID = mainPathWay.pathWayID,
                cautionBtnPos = mainPathWay.cautionBtnPosHolder.position,
                pathWaySegmentList = ExtractPathWaySegmentList(mainPathWay.pathWaySegmentHolderList),
            };

            pathWayDataList.Add(mainpathWayInfo);
        }
        return pathWayDataList;
    }

    private List<PathWaySegment> ExtractPathWaySegmentList(List<Transform> pathWaySegmentHolderList)
    {
        List<PathWaySegment> pathWaySegmentList = new();
        foreach(Transform pathWaySegmentHolder in pathWaySegmentHolderList)
        {
            PathWaySegment pathWaySegment = new()
            {
                WayPointList = ExtractChildPos(pathWaySegmentHolder)
            };
            pathWaySegmentList.Add(pathWaySegment);
        }
        return pathWaySegmentList;
    }
}
