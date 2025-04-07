using System.Collections.Generic;
using UnityEngine;

public class WayPointCollector : MonoBehaviour
{
    public List<MapWayPoint> mapWayPointList;
}

[System.Serializable]
public class MapWayPoint
{
    public int mapID;
    public Transform endPointHolder;
    public Transform emptyPlotHolder;
    public List<Transform> initGuardPointPosHolder;
    public List<MainPathWay> mainPathWayList;
}

[System.Serializable]
public class MainPathWay
{
    public int pathWayID;
    public Transform cautionBtnPosHolder;
    public List<Transform> pathWaySegmentHolderList;
}