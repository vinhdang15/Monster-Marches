using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EndPointManager : MonoBehaviour
{
    [SerializeField] EndPoint endPointPrefab;
    [SerializeField] List<EndPoint> endPointList;
    private WayPointDataReader wayPointDataReader;

    public void PrepareGame(WayPointDataReader wayPointDataReader)
    {
        this.wayPointDataReader = wayPointDataReader;
    }

    public void CreateEndPoint(MapData mapData)
    {
        List<Vector2> pos = wayPointDataReader.GetEndPointPos(mapData);
        foreach (var i in pos)
        {
            EndPoint endPoint = Instantiate(endPointPrefab, i, quaternion.identity, transform);
            endPointList.Add(endPoint);
        }
        ;
    }

    public void ClearEndPoints()
    {
        foreach(EndPoint endPoint in endPointList)
        {
            Destroy(endPoint.gameObject);   
        }
        endPointList.Clear();
    }
}
