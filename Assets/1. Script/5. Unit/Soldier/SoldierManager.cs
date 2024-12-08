using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    [SerializeField] UnitPool unitPool;
    private List<GuardPoint> guardPoints = new List<GuardPoint>();

    private void Update()
    {
        updateAllSoldierToGuardPoint();
    }

    public void BarrackSpawnSoldier(string unitName, Vector2 initPos, GuardPoint guardPoint)
    {
        for(int i = 0; i < 3; i++)
        {
            Soldier soldier = unitPool.GetSoldier(unitName, initPos);
            soldier.index = i;
            guardPoint.soldiers.Add(soldier);
        }
        guardPoints.Add(guardPoint);
    }
    public void updateAllSoldierToGuardPoint()
    {
        foreach(var guardPoint in guardPoints)
        {
            guardPoint.MoveSoldierToGuardPoint();
        }
    }
}
