using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    [SerializeField] UnitPool unitPool;
    private List<GuardPoint> guardPoints = new List<GuardPoint>();
    private List<Soldier> totalsoldiers = new List<Soldier>();

    private void Update()
    {
        foreach(var soldier in totalsoldiers)
        {
            soldier.SoldierAction();
        }
    }

    public void BarrackSpawnSoldier(string unitName, Vector2 initPos, GuardPoint guardPoint)
    {
        for(int i = 0; i < 3; i++)
        {
            Soldier soldier = unitPool.GetSoldier(unitName, initPos);
            soldier.index = i;
            soldier.GetAnimation();
            soldier.GetOffsetPos();
            soldier.OnSoldierDeath += HandleSoldierDie;
            guardPoint.AddSoldier(soldier);
            totalsoldiers.Add(soldier);
        }
        guardPoints.Add(guardPoint);
    }

    private void HandleSoldierDie(Soldier soldier)
    {
        totalsoldiers.Remove(soldier);
        //Play die animation
        soldier.unitAnimation.UnitPlayDie();
        // wait to finish die animation then return unit pool
        StartCoroutine(soldier.ReturnPoolAfterPlayAnimation(unitPool));
    }
}
