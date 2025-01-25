using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackTowerManager : TowerBaseManager
{
    [SerializeField] SoldierManager                     soldierManager;
    [SerializeField] TowerViewBase                      barrackPerfab;
    [SerializeField] BarrackSpawnGuardPointConfigSO     barrackSpawnGuardPointConfigSO;
    public Dictionary<TowerPresenter, BarackTowerInfor> barackTowerInfor = new Dictionary<TowerPresenter, BarackTowerInfor>();

    private void Awake()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        soldierManager = GameObject.Find("SoldierManager").GetComponent<SoldierManager>();
    }
    
    public  void Init(Vector3 pos, TowerType barrackType, EmptyPlot emptyPlot)
    {
        TowerData towerData = CSVTowerDataReader.Instance.towerDataList.GetTowerData(barrackType.ToString().Trim().ToLower(), 1);
        TowerPresenter barrackPresenter = base.InitBuildingPresenter(barrackPerfab, towerData, pos);

        base.AddTowerPersenterEmptyPlot(barrackPresenter, emptyPlot);

        barackTowerInfor[barrackPresenter] = new BarackTowerInfor();
        GetBarrackGuradPoint(barrackPresenter);
        barackTowerInfor[barrackPresenter].barackGuardPoint.transform.position = barrackSpawnGuardPointConfigSO.GetNearestPoint(barrackPresenter.transform).position;
        StartCoroutine(SpawnBarrackSoldierCoroutine(barrackPresenter));
    }

    // assign barack guardPoint reference to barackTowerInfor
    private void GetBarrackGuradPoint(TowerPresenter barrackPresenter)
    {
        barackTowerInfor[barrackPresenter].barackGuardPoint = barrackPresenter.transform.GetChild(1).GetComponent<GuardPoint>();
    }

    public void InitBarack(Vector3 pos, EmptyPlot emptyPlot)
    {
        Init(pos,TowerType.Barrack, emptyPlot);
    }

    // call from GameplayManager
    public void SetNewGuardPointPos(TowerPresenter barrackPresenter, Vector2 pos)
    {
        barackTowerInfor[barrackPresenter].barackGuardPoint.SetNewGuardPointPos(pos);
    }

    #region PROCESS SPAWN SOLDIER
    private IEnumerator SpawnBarrackSoldierCoroutine(TowerPresenter barrackPresenter)
    {
        string soldierName = barrackPresenter.towerModel.SpawnObject;
        BarrackTowerView barrackTowerView = barrackPresenter.towerView as BarrackTowerView;
        Vector2 initPos = barrackTowerView.GetSpawnSoldierPos();

        GuardPoint barackGuardPoint = barackTowerInfor[barrackPresenter].barackGuardPoint;

        Vector2 barrackGatePos = (barrackPresenter.towerView as BarrackTowerView).GetSpawnSoldierPos();
        float soldierRevivalSpeed = barrackPresenter.towerModel.SpawnRate;
        barrackTowerView.OpenGateAnimation();
        yield return new WaitForSeconds(0.8f);
        soldierManager.BarrackSpawnSoldier(barrackTowerView, soldierName, initPos, barackGuardPoint, barrackGatePos, soldierRevivalSpeed);
    }
    #endregion
}

public class BarackTowerInfor
{
    public GuardPoint barackGuardPoint;
}