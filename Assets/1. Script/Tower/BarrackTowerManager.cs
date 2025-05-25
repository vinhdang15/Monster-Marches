using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackTowerManager : TowerBaseManager
{
    [SerializeField] TowerViewBase                      barrackPerfab;
    [SerializeField] SoldierManager                     soldierManager;
    [SerializeField] List<TowerPresenter> barackTowerList = new();
    private List<Vector2> initGuardPointPosList = new();
    private UnitDataReader unitDataReader;
    private WayPointDataReader wayPointDataReader;
    
    public Dictionary<TowerPresenter, BarackTowerInfor> barrackTowerInfor = new Dictionary<TowerPresenter, BarackTowerInfor>();

    public void PrepareGame(TowerDataReader towerDataReader, UnitDataReader unitDataReader, WayPointDataReader wayPointDataReader)                       
    {
        this.towerDataReader = towerDataReader;
        this.unitDataReader = unitDataReader;
        this.wayPointDataReader = wayPointDataReader;
        LoadComponents();
    }

    public void InitializeGuardPointPosList(MapData mapData)
    {
        initGuardPointPosList = wayPointDataReader.GetInitGuardPointPosList(mapData);
    }


    private void LoadComponents()
    {
        soldierManager = FindObjectOfType<SoldierManager>();
    }
    
    private void Init(Vector3 pos, TowerType barrackType, EmptyPlot emptyPlot,
                        TowerDataReader towerDataReader, UnitDataReader unitDataReader)
                       
    {
        TowerData towerData = towerDataReader.towerDataListSO.GetTowerData(barrackType.ToString(), 1);
        TowerPresenter barrackPresenter = base.InitBuildingPresenter(barrackPerfab, towerData, pos,
                                                                    towerDataReader, null,
                                                                    unitDataReader);

        base.AddTowerPersenterEmptyPlot(barrackPresenter, emptyPlot);

        barrackTowerInfor[barrackPresenter] = new BarackTowerInfor();
        GetBarrackGuradPoint(barrackPresenter);
        barrackTowerInfor[barrackPresenter].barrackGuardPoint.transform.position = GetNearestPoint(barrackPresenter.transform, initGuardPointPosList);
        SpawnBarrackSoldier(barrackPresenter);
        barackTowerList.Add(barrackPresenter);
    }

    #region INIT BUILDING
    public void InitBarack(Vector3 pos, EmptyPlot emptyPlot)
    {
        Init(pos, TowerType.Barrack, emptyPlot, towerDataReader, unitDataReader);
    }

    // Assign barrack guardPoint reference to barrackTowerInfor
    private void GetBarrackGuradPoint(TowerPresenter barrackPresenter)
    {
        barrackTowerInfor[barrackPresenter].barrackGuardPoint = barrackPresenter.transform.GetChild(1).GetComponent<GuardPoint>();
    }

    // Call from GameplayManager
    public void SetNewGuardPointPos(TowerPresenter barrackPresenter, Vector2 pos)
    {
        barrackTowerInfor[barrackPresenter].barrackGuardPoint.SetNewGuardPointPos(pos);
    }
    #endregion

    public override void UpgradeTower(TowerPresenter towerPresenter)
    {
        base.UpgradeTower(towerPresenter);
    }

    #region SPAWN SOLDIER
    public void SpawnBarrackSoldier(TowerPresenter barrackPresenter)
    {
        StartCoroutine(SpawnBarrackSoldierCoroutine(barrackPresenter));
    }
    
    private IEnumerator SpawnBarrackSoldierCoroutine(TowerPresenter barrackPresenter)
    {
        string soldierName = barrackPresenter.towerModel.SpawnObject;
        BarrackTowerView barrackTowerView = barrackPresenter.towerViewBase as BarrackTowerView;
        Vector2 initPos = barrackTowerView.GetSpawnSoldierPos();

        GuardPoint barackGuardPoint = barrackTowerInfor[barrackPresenter].barrackGuardPoint;

        Vector2 barrackGatePos = (barrackPresenter.towerViewBase as BarrackTowerView).GetSpawnSoldierPos();
        float soldierRevivalSpeed = barrackPresenter.towerModel.SpawnRate;
        barrackTowerView.OpenGateAnimation();
        yield return new WaitForSeconds(0.5f);
        soldierManager.BarrackSpawnSoldier(barrackTowerView, soldierName, initPos, barackGuardPoint, barrackGatePos, soldierRevivalSpeed);
    }
    #endregion

    #region UPGRADE SOLDIER
    public void ReplaceSoldier(TowerPresenter barrackPresenter)
    {
        StartCoroutine(ReplaceSoldierCoroutine(barrackPresenter));
    }

    private IEnumerator ReplaceSoldierCoroutine(TowerPresenter barrackPresenter)
    {
        GuardPoint barackGuardPoint = barrackTowerInfor[barrackPresenter].barrackGuardPoint;
        yield return soldierManager.OnBarrackUpgrade(barackGuardPoint);
        yield return StartCoroutine(SpawnBarrackSoldierCoroutine(barrackPresenter));
    }
    #endregion

    #region SET GUARD POINT
    private Vector2 GetNearestPoint(Transform barrackTowerView, List<Vector2> pos)
    {
        Vector2 nearestPoint = new();
        float shortestDistance = float.MaxValue; ;
        foreach (Vector2 childPos in pos)
        {
            float distance = Vector2.Distance(barrackTowerView.position, childPos);
            if (distance > shortestDistance) continue;
            shortestDistance = distance;
            nearestPoint = childPos;
        }
        return nearestPoint;
    }
    #endregion

    public void CleanupSelectedTower(TowerPresenter selectedTower)
    {
        barackTowerList.Remove(selectedTower);
        Destroy(selectedTower.gameObject);
    }
    
    public void ClearBarrackTowers()
    {
        foreach(var barrackTower in barackTowerList)
        {
            Destroy(barrackTower.gameObject);
        }
        barackTowerList.Clear();
    }
}

public class BarackTowerInfor
{
    public GuardPoint barrackGuardPoint;
}