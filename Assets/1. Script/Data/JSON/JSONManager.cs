using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

public class JSONManager
{
    private static string assetPath = "E:/Funix_Course/Project-2D/Monster Marches/Assets/AddressableFile/JSON";
    private static string persistentDataPath = Application.persistentDataPath;
    private static bool hasSaveGameData = false;
    private static string mapProgressDataJsonPath = Path.Combine(persistentDataPath, "MapProgressData.json");
    private static string mapDesignDataJsonPath = Path.Combine(assetPath, "MapDesignData.json");
    private static string decorObjDataJsonPath = Path.Combine(assetPath, "DecorObjData.json");
    private static string waypointDataJsonPath = Path.Combine(assetPath, "WayPointData.json");
    private static string towerDataJsonPath = Path.Combine(assetPath, "TowerData.json");
    private static string bulletDataJsonPath = Path.Combine(assetPath, "BulletData.json");
    private static string unitDataJsonPath = Path.Combine(assetPath, "UnitData.json");
    private static string SkillDataJsonPath = Path.Combine(assetPath, "SkillData.json");
    private static string bulletEffectDataJsonPath = Path.Combine(assetPath, "BulletEffectData.json");
    private static string enemyWaveDataJsonPath = Path.Combine(assetPath, "EnemyWaveData.json");

    public static List<MapDesignData> mapDesignDataList = new();
    public static List<MapProgressData> mapProgressDataList = new();
    public static List<DecorObjData> decorObjDataList = new();
    public static List<WayPointData> wayPointDataList = new();
    public static List<TowerData> towerDataList = new();
    public static List<BulletData> bulletDataList = new();
    public static List<BulletEffectData> bulletEffectDataList = new();
    public static List<UnitData> unitDataList = new();
    public static List<SkillData> skillDataList = new();
    public static List<EnemyWaveData> enemyWaveDataList = new();
    

    public static async Task PrepareGameAsync()
    {
        mapDesignDataList       = await LoadMapDesignDataFromJsonSever();
        mapProgressDataList     = await LoadMapProgressData();
        decorObjDataList        = await LoadDecorObjDataFromJsonSever();
        wayPointDataList        = await LoadWaypointDataFromJsonSever();
        towerDataList           = await LoadTowerDataFromJsonSever();
        bulletDataList          = await LoadBulletDataFromJsonSever();
        bulletEffectDataList    = await LoadBulletEffectDataFromJsonSever();
        unitDataList            = await LoadUnitDataFromJsonSever();
        skillDataList           = await LoadSkillDataFromJsonSever();
        enemyWaveDataList       = await LoadEnemyWaveDataFromJsonSever();
    }

    #region  MAP DESIGNE DATA
    public static void SaveMapDesignDataToJson(List<MapDesignData> mapDataList)
    {
        string json = JsonConvert.SerializeObject(mapDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(mapDesignDataJsonPath, json);
        
        Debug.Log("Create completed MapData JSON file");
    }

    public static async Task<List<MapDesignData>> LoadMapDesignDataFromJsonSever()
    {
        List<MapDesignData> dataList = await JSONListLoader<MapDesignData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region  MAP PROGRESS DATA
    public static void SaveMapProgressDataToJson(List<MapProgressData> mapProgressDataList)
    {
        string json = JsonConvert.SerializeObject(mapProgressDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(mapProgressDataJsonPath, json);
        Debug.Log("Create completed MapProgressData JSON file");
    }
    
    private static async Task<List<MapProgressData>> LoadMapProgressData()
    {
        if(File.Exists(mapProgressDataJsonPath))
        {
            string json = File.ReadAllText(mapProgressDataJsonPath);
            mapProgressDataList = JsonConvert.DeserializeObject<List<MapProgressData>>(json);
            hasSaveGameData = true;
        }
        else
        {
            mapProgressDataList = await LoadMapProgressDataFromSever();
        }
        return mapProgressDataList;
    }

    public static bool HasSaveGameData()
    {
        return hasSaveGameData;
    }

    public static void SetHasSaveGameData(bool setBool)
    {
        hasSaveGameData = setBool;
    }

    // Load data from Addressable sever
    private static async Task<List<MapProgressData>> LoadMapProgressDataFromSever()
    {
        List<MapProgressData> dataList = await JSONListLoader<MapProgressData>.LoadJSONListAsync();                                  
        return dataList;
    }

    public static IEnumerator ResetMapProgressData()
    {
        DeleteCurrentMapProgressSaveData();
        
        var reloadInitMapProgressData = SetNewMapProgressDataListFromJsonServer();
        yield return new WaitUntil(() => reloadInitMapProgressData.IsCompleted);
    }

    private static void DeleteCurrentMapProgressSaveData()
    {
        if (File.Exists(mapProgressDataJsonPath))
        {
            File.Delete(mapProgressDataJsonPath);
            hasSaveGameData = false;
            Debug.Log("Delete MapProgressData");
        }
    }

    private static async Task SetNewMapProgressDataListFromJsonServer()
    {
        mapProgressDataList = await LoadMapProgressData();
        hasSaveGameData = true;
    }

    #endregion 

    #region WAY POINT DATA
    public static void SaveMapObjDataToJson(List<DecorObjData> decorObjDataList)
    {
        string json = JsonConvert.SerializeObject(decorObjDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(decorObjDataJsonPath, json);
        Debug.Log("Create completed DecorObjData JSON file");
    }

    private static async Task<List<DecorObjData>> LoadDecorObjDataFromJsonSever()
    {
        List<DecorObjData> dataList = await JSONListLoader<DecorObjData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region WAY POINT DATA
    public static void SaveWayPointDataToJson(List<WayPointData> wayPointDataList)
    {
        string json = JsonConvert.SerializeObject(wayPointDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(waypointDataJsonPath, json);
        Debug.Log("Create completed WayPointData JSON file");
    }

    private static async Task<List<WayPointData>> LoadWaypointDataFromJsonSever()
    {
        List<WayPointData> dataList = await JSONListLoader<WayPointData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region TOWER DATA
    public static void SaveTowerDataToJson(List<TowerData> towerDataList)
    {
        string json = JsonConvert.SerializeObject(towerDataList, Formatting.Indented);
        File.WriteAllText(towerDataJsonPath, json);
        Debug.Log("Create completed towerData JSON file");
    }

    private static async Task<List<TowerData>> LoadTowerDataFromJsonSever()
    {
        List<TowerData> dataList = await JSONListLoader<TowerData>.LoadJSONListAsync();         
        return dataList;
    }
    #endregion

    #region BULLET DATA
    public static void SaveBulletDataToJson(List<BulletData> bulletDataList)
    {
        string json = JsonConvert.SerializeObject(bulletDataList, Formatting.Indented);
        File.WriteAllText(bulletDataJsonPath, json);
        Debug.Log("Create completed bulletData JSON file");
    }

   private static async Task<List<BulletData>> LoadBulletDataFromJsonSever()
    {
        List<BulletData> dataList = await JSONListLoader<BulletData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region BULLET EFFECT DATA
    public static void SaveBulletEffectDataToJson(List<BulletEffectData> bulletEffectDataList)
    {
        string json = JsonConvert.SerializeObject(bulletEffectDataList, Formatting.Indented);
        File.WriteAllText(bulletEffectDataJsonPath, json);
        Debug.Log("Create completed BulletEffectData JSON file");
    }

    private static async Task<List<BulletEffectData>> LoadBulletEffectDataFromJsonSever()
    {
        List<BulletEffectData> dataList = await JSONListLoader<BulletEffectData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region UNIT DATA
    public static void SaveUnitDataToJson(List<UnitData> unitDataList)
    {
        string json = JsonConvert.SerializeObject(unitDataList, Formatting.Indented);
        File.WriteAllText(unitDataJsonPath, json);
        Debug.Log("Create completed UnitData JSON file");
    }

    private static async Task<List<UnitData>> LoadUnitDataFromJsonSever()
    {
        List<UnitData> dataList = await JSONListLoader<UnitData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region SKILL DATA
    public static void SaveSkillDataToJson(List<SkillData> skillDataList)
    {
        string json = JsonConvert.SerializeObject(skillDataList, Formatting.Indented);
        File.WriteAllText(SkillDataJsonPath, json);
        Debug.Log("Create completed SkillData JSON file");
    }

    private static async Task<List<SkillData>> LoadSkillDataFromJsonSever()
    {
        List<SkillData> dataList = await JSONListLoader<SkillData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion

    #region ENEMY WAY DATA
    public static void SaveEnemyWayDataToJson(List<EnemyWaveData> EnemyWaveDataList)
    {
        string json = JsonConvert.SerializeObject(EnemyWaveDataList, Formatting.Indented);
        File.WriteAllText(enemyWaveDataJsonPath, json);
        Debug.Log("Create completed EnemyWayData JSON file");
    }

    private static async Task<List<EnemyWaveData>> LoadEnemyWaveDataFromJsonSever()
    {
        List<EnemyWaveData> dataList = await JSONListLoader<EnemyWaveData>.LoadJSONListAsync();
        return dataList;
    }
    #endregion
}