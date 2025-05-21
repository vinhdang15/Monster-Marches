using System.Collections.Generic;
using UnityEngine;

public class JSONCreator : MonoBehaviour
{
    public static JSONCreator Instance;
    MapDesignDataForJson mapDesignDataForJson;
    UnitDataForJon unitDataForJon;
    SkillDataForJson skillDataForJson;
    TowerDataForJson towerDataForJson;
    BulletDataForJson bulletDataForJson;
    BulletEffectDataForJson bulletEffectDataForJson;
    EnemyWaveDataInfo enemyWaveDataInfo;
    [SerializeField] DecorObjDataProcessor decorObjDataProc;
    [SerializeField] WayPointDataProcessor wayPointDataProc;
    
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
        mapDesignDataForJson = GetComponent<MapDesignDataForJson>();
        towerDataForJson = GetComponent<TowerDataForJson>();
        bulletDataForJson = GetComponent<BulletDataForJson>();
        bulletEffectDataForJson = GetComponent<BulletEffectDataForJson>();
        unitDataForJon = GetComponent<UnitDataForJon>();
        skillDataForJson = GetComponent<SkillDataForJson>();
        enemyWaveDataInfo = GetComponent<EnemyWaveDataInfo>();
    }

    public void CreateMapDesignDataJson()
    {
        List<MapDesignData> mapDesignDatas = mapDesignDataForJson.GetMapDesignDataForJson();
        JSONManager.SaveMapDesignDataToJson(mapDesignDatas);
    }

    public void CreateMapProgressDataJson()
    {
        List<MapProgressData> mapProgressDatas = new()
        {
            new MapProgressData
            {
                mapID = 1,
                activate = true,
                starsPoint = 0,
            }
        };
        JSONManager.SaveMapProgressDataToJson(mapProgressDatas);
    }

    // mapObjCollector
    public void CreateMapObjDataJson()
    {
        List<DecorObjData> decorObjDatas = decorObjDataProc.ExtractMapObjData();
        JSONManager.SaveMapObjDataToJson(decorObjDatas);
    }

    public void CreateWayPointDataJson()
    {
        List<WayPointData> wayPointDatas = wayPointDataProc.ExtractWayPointData();
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

    public void CreateEnemyWaveDataInfoJson()
    {
        List<EnemyWaveData> EnemyWaveDatas = enemyWaveDataInfo.GetEnemyWaveDataList();
        JSONManager.SaveEnemyWayDataToJson(EnemyWaveDatas);
    }
}
