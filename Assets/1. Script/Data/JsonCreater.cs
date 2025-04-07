using System.Collections.Generic;
using UnityEngine;

public class JsonCreater : MonoBehaviour
{
    public static JsonCreater Instance;
    MapDataForJson mapdataForJson;
    UnitDataForJon unitDataForJon;
    SkillDataForJson skillDataForJson;
    TowerDataForJson towerDataForJson;
    BulletDataForJson bulletDataForJson;
    BulletEffectDataForJson bulletEffectDataForJson;
    [SerializeField] WayPointDataProcessor wayPointDataList;
    
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
        towerDataForJson = GetComponent<TowerDataForJson>();
        bulletDataForJson = GetComponent<BulletDataForJson>();
        bulletEffectDataForJson = GetComponent<BulletEffectDataForJson>();
        unitDataForJon = GetComponent<UnitDataForJon>();
        skillDataForJson = GetComponent<SkillDataForJson>();
    }

    public void CreateMapDataJson()
    {
        List<MapData> mapDatas = mapdataForJson.GetMapDataForJson();
        JSONManager.SaveMapDataToJson(mapDatas);
    }

    public void CreateWayPointDataJson()
    {
        List<WayPointData> wayPointDatas = wayPointDataList.ExtractWayPointData();
        JSONManager.SaveWayPointDataToJson(wayPointDatas);
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

    public void CreateBulletEffectDataJson()
    {
        List<BulletEffectData> bulletEffectDatas = bulletEffectDataForJson.GetBulletEffectDatas();
        JSONManager.SaveBulletEffectDataToJson(bulletEffectDatas);
    }

    public void CreateUnitDataJson()
    {
        List<UnitData> unitDatas = unitDataForJon.GetUnitDataForJson();
        JSONManager.SaveUnitDataToJson(unitDatas);
    }

    public void CreateSkillDataJson()
    {
        List<SkillData> skillDatas = skillDataForJson.GetSkillDataForJson();
        JSONManager.SaveSkillDataToJson(skillDatas);
    }
}
