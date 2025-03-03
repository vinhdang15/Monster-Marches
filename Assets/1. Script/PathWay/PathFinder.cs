using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    // In each path (PathConfigSO), there is three Moving row
    public PathConfigSO PathConfigSO { get; set; }
    [SerializeField] List<Transform> waypoints;
    int wayPointIndex = 0;

    private void Start()
    {
        wayPointIndex = 0;
    }
    public void OnSetPosInPathWay(int _pathWayIndex)
    {
        PathConfigSO.index = _pathWayIndex;

        transform.position = PathConfigSO.GetStartingWaypoint().position;
        // rest waypoints, wayPointIndex
        wayPointIndex = 0;
        waypoints =  PathConfigSO.GetWayPoints();
    }
    
    public void FollowPath(float speed)
    {
        if (wayPointIndex < waypoints.Count)
        {
            if(transform.position != waypoints[wayPointIndex].position)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[wayPointIndex].position, speed * Time.deltaTime);
            }
            else
            {
                wayPointIndex++;
            }
        }
    }
}
