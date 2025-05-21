using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private List<PathWaySegment> pathWaySegmentList;
    [SerializeField] List<Vector2> wayPointList;
    int wayPointIndex = 0;

    private void Start()
    {
        wayPointIndex = 0;
    }

    public void PrepareGame(List<PathWaySegment> pathWaySegmentList, int pathWaySegmentIndex)
    {
        SetPathWaySegmentList(pathWaySegmentList);
        GetWayPointList(pathWaySegmentIndex);
        SetInitPosInWayPointList();
    }

    private void SetPathWaySegmentList(List<PathWaySegment> pathWaySegmentList)
    {
        this.pathWaySegmentList = pathWaySegmentList;
    }
    
    private void GetWayPointList(int pathWaySegmentIndex)
    {
        PathWaySegment pathWaySegment = pathWaySegmentList[pathWaySegmentIndex];
        wayPointList = pathWaySegment.WayPointList;
    }
    private void SetInitPosInWayPointList()
    {
        // Reset value
        transform.position = wayPointList[0];
        wayPointIndex = 1;
    }

    public void FollowPath(float speed)
    {
        if (wayPointIndex < wayPointList.Count)
        {
            if (Vector2.SqrMagnitude((Vector2)transform.position - wayPointList[wayPointIndex]) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, wayPointList[wayPointIndex], speed * Time.deltaTime);
            }
            else
            {
                wayPointIndex++;
            }
        }
    }
}
