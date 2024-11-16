using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    // On a path there will be three rows of moves represented by each WaveConfigSO in waveConfigList
    public PathConfigSO PathConfigSO { get; set; }
    [SerializeField] List<Transform> waypoints;
    int wayPointIndex = 0;
    
    public int waveConfigIndex;

    public void OnSetPosInPathWay(int _pathWayIndex)
    {
        PathConfigSO.index = _pathWayIndex;

        transform.position = PathConfigSO.GetStartingWaypoint().position;
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
