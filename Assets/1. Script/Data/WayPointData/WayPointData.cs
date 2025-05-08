using System.Collections.Generic;
using UnityEngine;

public class WayPointData
{
    public int mapID;
    public List<Vector2>            endPointPosList;
    public List<Vector2>            emptyPlotPosList;
    public List<Vector2>            initGuardPointPosList;
    public List<TreePatchInfo>      treePatchInforList;
    public List<MainPathWayInfo>    mainPathWayInforList;
}

[System.Serializable]
public class TreePatchInfo
{
    public string treePatchID;
    public List<Vector2> treePatchList;
}

[System.Serializable]
public class MainPathWayInfo
{
    public int pathWayID;
    public Vector2 cautionBtnPos;
    public List<PathWaySegment> pathWaySegmentList;
}

[System.Serializable]
public class PathWaySegment
{
    public List<Vector2> WayPointList = new();
}
