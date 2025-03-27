using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonCreater : MonoBehaviour
{
    public static JsonCreater Instance;
    MapDataForJson mapdataForJson;
    EmptyPlotDataHolderListForJson emptyPlotDataForJson;
    TowerDataForJson towerDataForJson;
    BulletDataForJson bulletDataForJson;
    UnitDataForJon unitDataForJon;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadComponents();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    private void LoadComponents()
    {
        mapdataForJson = GetComponent<MapDataForJson>();
        emptyPlotDataForJson = GetComponent<EmptyPlotDataHolderListForJson>();
        towerDataForJson = GetComponent<TowerDataForJson>();
        bulletDataForJson = GetComponent<BulletDataForJson>();
        unitDataForJon = GetComponent<UnitDataForJon>();
    }

    public void CreateMapDataJson()
    {
        List<MapData> mapDatas = mapdataForJson.GetMapDataForJson();
        JSONManager.SaveMapDataToJson(mapDatas);
    }

    public void CreateEmptyPlotDataJson()
    {
        List<EmptyPlotDataHolder> emptyPlotDatas = emptyPlotDataForJson.GetEmptyPlotDataForJson();
        JSONManager.SaveEmptyPlotDataToJson(emptyPlotDatas);
    }

    public void CreateTowerDataJson()
    {
        List<TowerData> towerDatas = towerDataForJson.GetTowerDataForJson();
        JSONManager.SaveTowerDataToJson(towerDatas);
    }

    public void CreateBulletDataJson()
    {
        List<BulletData> bulletDatas = bulletDataForJson.GetBulletDataForJson();
        JSONManager.SaveBulletDataToJson(bulletDatas);
    }

    public void CreateUnitDataJson()
    {
        List<UnitData> unitDatas = unitDataForJon.GetUnitDataForJson();
        JSONManager.SaveUnitDataToJson(unitDatas);
    }
}
