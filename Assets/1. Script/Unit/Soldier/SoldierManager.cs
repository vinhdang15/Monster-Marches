using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    private List<GuardPoint> guardPoints = new List<GuardPoint>();
    [SerializeField] List<Soldier> totalsoldiers = new List<Soldier>();

    private void Update()
    {
        if(totalsoldiers.Count == 0) return;
        foreach(var soldier in totalsoldiers)
        {
            soldier.SoldierAction();
        }
    }

    public void BarrackSpawnSoldier(BarrackTowerView barrackTowerView, string unitName, Vector2 initPos, GuardPoint guardPoint, Vector2 barrackGatePos, float revivalSpeed)
    {
        guardPoint.OnBarrackDestroy += HandleOnBarrackDestroy;

        for(int i = 0; i < 3; i++)
        {
            Soldier soldier = UnitPool.Instance.GetSoldier(unitName, initPos);
            soldier.SoldierInitInfor(barrackTowerView, i, barrackGatePos, revivalSpeed);
            soldier.OnSoldierDeath += HandleSoldierDie;
            guardPoint.AddSoldier(soldier);
            totalsoldiers.Add(soldier);
        }
        guardPoints.Add(guardPoint);
    }

    private void HandleSoldierDie(Soldier soldier)
    {
        //totalsoldiers.Remove(soldier);
        //Play die animation
        soldier.unitAnimation.UnitPlayDie();
        // wait to finish die animation then return barack tower wait to respawn
        StartCoroutine(soldier.RevivalCoroutine());
    }

    private void HandleOnBarrackDestroy(GuardPoint guardPoint)
    {
        ReturnSoldierToPool(guardPoint);
        guardPoint.OnBarrackDestroy -= HandleOnBarrackDestroy;
    }

    public IEnumerator OnBarrackUpgrade(GuardPoint guardPoint)
    {
        float soldierFadeOutTime = 0.5f;
        foreach(var soldier in guardPoint.soldiers)
        {
            soldier.isBarrackUpgrade = true;
            StartCoroutine(soldier.FadeOut(soldierFadeOutTime, () => ReturnSoldierToPool(guardPoint)));
        }
        yield return null;
    }

    private void ReturnSoldierToPool(GuardPoint guardPoint)
    {
        foreach(var soldier in guardPoint.soldiers)
        {
            totalsoldiers.Remove(soldier);
            soldier.OnSoldierDeath -= HandleSoldierDie;
            soldier.barrackTowerView = null;
            soldier.SoldierReturnToUnitPool();
            soldier.isBarrackUpgrade = false;
        }
        guardPoint.soldiers.Clear();
    }

    public void ReturnAllSoldierToPool()
    {
        foreach(var soldier in totalsoldiers)
        {
            soldier.OnSoldierDeath -= HandleSoldierDie;
            soldier.barrackTowerView = null;
            soldier.SoldierReturnToUnitPool();
            soldier.isBarrackUpgrade = false;
        }
    }
}
