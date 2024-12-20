using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EndPointManager : MonoBehaviour
{
    [SerializeField] List<Transform> endPointsPosList;
    [SerializeField] EndPoint endPointPrefab;

    private void Start()
    {
        InitEndPoint();
    }
    private void InitEndPoint()
    {
        foreach(var endPointsPos in endPointsPosList)
        {
            EndPoint endPoint = Instantiate(endPointPrefab, endPointsPos.position, endPointsPos.rotation, transform);
        }
        
    }
}
