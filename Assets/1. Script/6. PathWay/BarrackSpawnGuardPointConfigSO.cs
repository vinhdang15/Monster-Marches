using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnGuardPoint", menuName = "SpawnGuardPoint Config/SpawnGuardPoint")]
public class BarrackSpawnGuardPointConfigSO : ScriptableObject
{
    [SerializeField] private List<Transform>    pathPrefab;

    public Transform GetNearestPoint(Transform barrackTowerView)
    {
        Transform nearestPoint = null;
        float shortestDistance = float.MaxValue;;
        foreach(Transform child in pathPrefab)
        {
            foreach(Transform child_2 in child)
            {
                float distance = Vector2.Distance(barrackTowerView.position, child_2.position);
                if(distance > shortestDistance) continue;
                shortestDistance = distance;
                nearestPoint = child_2;
            }
        }
        return nearestPoint;
    }
}
