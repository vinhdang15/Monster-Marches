using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackTowerManager : TowerBaseManager
{
    [SerializeField] SoldierManager         soldierManager;
    [SerializeField] TowerView              barrackPerfab;
    public Dictionary<TowerPresenter, BarackTowerInfor> barackTowerInfor = new Dictionary<TowerPresenter, BarackTowerInfor>();

    public  void Init(Vector3 pos, BarrackType barrackType, EmptyPlot emptyPlot)
    {
        TowerData towerData = towerDataReader.towerDataList.GetTowerData(barrackType.ToString().Trim().ToLower(), 1);
        
        TowerPresenter barrackPresenter = InitBuildingPresenter(barrackPerfab, towerData, pos);
        base.AddTowerPersenterEmptyPlot(barrackPresenter, emptyPlot);

        barackTowerInfor[barrackPresenter] = new BarackTowerInfor();
        GetBarrackGuradPoint(barrackPresenter);
        SpawnBarackSoldier(barrackPresenter);
    }

    private void GetBarrackGuradPoint(TowerPresenter barrackPresenter)
    {
        barackTowerInfor[barrackPresenter].barackGuardPoint = barrackPresenter.transform.GetChild(0).GetComponent<GuardPoint>();
    }

    public void InitBarack(Vector3 pos, EmptyPlot emptyPlot)
    {
        Init(pos,BarrackType.Barrack, emptyPlot);
    }

    public void SetNewGuardPointPos(TowerPresenter barrackPresenter, Vector2 pos)
    {
        barackTowerInfor[barrackPresenter].barackGuardPoint.transform.position = pos;
        barackTowerInfor[barrackPresenter].barackGuardPoint.StartMoveSoldierToGuardPoint();
    }

    #region PROCESS SPAWN SOLDIER
    private void SpawnBarackSoldier(TowerPresenter barrackPresenter)
    {
        if(barrackPresenter.towerModel.TowerType != BarrackType.Barrack.ToString().Trim().ToLower()) return;
        string soldierName = barrackPresenter.towerModel.BulletType;
        Vector2 initPos = barrackPresenter.towerView.GetSpawnBulletPos();

        GuardPoint barackGuardPoint = barackTowerInfor[barrackPresenter].barackGuardPoint;
        soldierManager.BarrackSpawnSoldier(soldierName, initPos, barackGuardPoint);
    }
    #endregion
}

public class BarackTowerInfor
{
    public GuardPoint barackGuardPoint;
}
