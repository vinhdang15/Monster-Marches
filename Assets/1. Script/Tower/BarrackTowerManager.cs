using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackTowerManager : TowerBaseManager
{
    [SerializeField] TowerViewBase                      barrackPerfab;
    [SerializeField] BarrackSpawnGuardPointConfigSO     barrackSpawnGuardPointConfigSO;
    [SerializeField] SoldierManager                     soldierManager;
    [SerializeField] SpawnGuardPointPath                spawnGuardPointPath;
    [SerializeField] List<TowerPresenter> barackTowerList = new();
    
    public Dictionary<TowerPresenter, BarackTowerInfor> barrackTowerInfor = new Dictionary<TowerPresenter, BarackTowerInfor>();

    public void PrepareGame()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        soldierManager = FindObjectOfType<SoldierManager>();
        spawnGuardPointPath = FindObjectOfType<SpawnGuardPointPath>();
        barrackSpawnGuardPointConfigSO = spawnGuardPointPath.barrackSpawnGuardPointConfigSO;
    }
    
    private void Init(Vector3 pos, TowerType barrackType, EmptyPlot emptyPlot)
    {
        TowerData towerData = TowerDataReader.Instance.towerDataListSO.GetTowerData(barrackType.ToString(), 1);
        TowerPresenter barrackPresenter = base.InitBuildingPresenter(barrackPerfab, towerData, pos);

        base.AddTowerPersenterEmptyPlot(barrackPresenter, emptyPlot);

        barrackTowerInfor[barrackPresenter] = new BarackTowerInfor();
        GetBarrackGuradPoint(barrackPresenter);
        barrackTowerInfor[barrackPresenter].barrackGuardPoint.transform.position = barrackSpawnGuardPointConfigSO.GetNearestPoint(barrackPresenter.transform).position;
        SpawnBarrackSoldier(barrackPresenter);
    }

    public void InitBarack(Vector3 pos, EmptyPlot emptyPlot)
    {
        Init(pos, TowerType.Barrack, emptyPlot);
    }

    // assign barrack guardPoint reference to barrackTowerInfor
    private void GetBarrackGuradPoint(TowerPresenter barrackPresenter)
    {
        barrackTowerInfor[barrackPresenter].barrackGuardPoint = barrackPresenter.transform.GetChild(1).GetComponent<GuardPoint>();
    }

    // call from GameplayManager
    public void SetNewGuardPointPos(TowerPresenter barrackPresenter, Vector2 pos)
    {
        barrackTowerInfor[barrackPresenter].barrackGuardPoint.SetNewGuardPointPos(pos);
    }

    #region PROCESS SPAWN SOLDIER
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