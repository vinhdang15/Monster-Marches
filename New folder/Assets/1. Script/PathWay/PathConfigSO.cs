using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Path", menuName = "Path Config/Path")]
public class PathConfigSO : ScriptableObject
{
    [SerializeField] private List<Transform>    pathPrefab;
    [HideInInspector] public int                index;
    public Transform                            cautionTranf;
    
    public Transform GetStartingWaypoint()
    {
        return pathPrefab[index].GetChild(0);
    }

    public List<Transform> GetWayPoints()
    {
        var wavePoints = new List<Transform>();
        foreach (Transform child in pathPrefab[index])
        {
            wavePoints.Add(child);
        }
        return wavePoints;
    }

    public Vector2 GetCautionPos()
    {
        return cautionTranf.GetChild(0).position;
    }
}
