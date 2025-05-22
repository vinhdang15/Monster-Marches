using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour
{
    private List<GuardPoint> guardPoints = new List<GuardPoint>();
    [SerializeField] List<Soldier> activeSoldiers = new List<Soldier>();

    private void Update()
    {
        if(activeSoldiers.Count == 0) return;
        foreach(var soldier in activeSoldiers)
        {
            soldier.SoldierAction();
        }
    }

    public void BarrackSpawnSoldier(BarrackTowerView barrackTowerView, string unitID,
                                    Vector2 initPos,GuardPoint guardPoint,
                                    Vector2 barrackGatePos, float revivalSpeed)
    {
        guardPoint.OnBarrackDestroy += HandleOnBarrackDestroy;

        for(int i = 0; i < 3; i++)
        {
            Soldier soldier = UnitPool.Instance.GetUnitBase(unitID, initPos) as Soldier;
            soldier.SoldierInitInfor(barrackTowerView, i, barrackGatePos, revivalSpeed);
            soldier.OnSoldierDeath += HandleSoldierDie;
            guardPoint.AddSoldier(soldier);
            activeSoldiers.Add(soldier);
        }
        guardPoints.Add(guardPoint);
    }

    private void HandleSoldierDie(Soldier soldier)
    {
        //Play die animation
        soldier.unitAnimation.UnitPlayDie();
        // wait to finish die animation then return to barack tower wait to respawn
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
        foreach (var soldier in guardPoint.soldiers)
        {
            soldier.StopAllCoroutines();
            activeSoldiers.Remove(soldier);
            soldier.OnSoldierDeath -= HandleSoldierDie;
            soldier.barrackTowerView = null;
            soldier.isBarrackUpgrade = false;
            soldier.ReturnToUnitPool();
        }
        guardPoint.soldiers.Clear();
    }

    public void ClearSoldierManager()
    {
        StopAllCoroutines();

        foreach (Soldier soldier in activeSoldiers)
        {
            soldier.OnSoldierDeath -= HandleSoldierDie;
            soldier.barrackTowerView = null;
            soldier.isBarrackUpgrade = false;
            UnitPool.Instance.ReturnToUnitPool(soldier);
        }
        activeSoldiers.Clear();
    }
}
