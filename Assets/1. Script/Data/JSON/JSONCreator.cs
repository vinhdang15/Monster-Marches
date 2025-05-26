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
        JSONDataExporter.ExportMapDesignData(mapDesignDatas);
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
        JSONDataExporter.ExportMapProgressData(mapProgressDatas);
    }

    // mapObjCollector
    public void CreateDecorObjDataJson()
    {
        List<DecorObjData> decorObjDatas = decorObjDataProc.ExtractMapObjData();
        JSONDataExporter.ExportDecorObjData(decorObjDatas);
    }

    public void CreateWayPointDataJson()
    {
        List<WayPointData> wayPointDatas = wayPointDataProc.ExtractWayPointData();
        JSONDataExporter.ExportWayPointData(wayPointDatas);
    }

    public void CreateTowerDataJson()
    {
        List<TowerData> towerDatas = towerDataForJson.GetTowerDataForJson();
        JSONDataExporter.ExportTowerData(towerDatas);
    }
    
    public void CreateBulletDataJson()
    {
        List<BulletData> bulletDatas = bulletDataForJson.GetBulletDataForJson();
        JSONDataExporter.ExportBulletData(bulletDatas);
    }

    public void CreateBulletEffectDataJson()
    {
        List<BulletEffectData> bulletEffectDatas = bulletEffectDataForJson.GetBulletEffectDatas();
        JSONDataExporter.ExportBulletEffectData(bulletEffectDatas);
    }

    public void CreateUnitDataJson()
    {
        List<UnitData> unitDatas = unitDataForJon.GetUnitDataForJson();
        JSONDataExporter.ExportUnitData(unitDatas);
    }

    public void CreateSkillDataJson()
    {
        List<SkillData> skillDatas = skillDataForJson.GetSkillDataForJson();
        JSONDataExporter.ExportSkillData(skillDatas);
    }

    public void CreateEnemyWaveDataInfoJson()
    {
        List<EnemyWaveData> EnemyWaveDatas = enemyWaveDataInfo.GetEnemyWaveDataList();
        JSONDataExporter.ExportEnemyWayData(EnemyWaveDatas);
    }
}
