using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

// this class use to load data to play game
// cause itch.io can't dowload bundle from github sever
// so for ich.io using PlayerPrefs + streamingAssetsPath

// this class contain two ways to get data
// SONListLoader<T>.LoadJSONListAsync() for load bundle from github sever
// SONListLoader<T>.LoadTextFileAsync(string fullPath) for PlayerPrefs + streamingAssetsPath
public class JSONDataLoader
{
    private static bool   hasSaveGameReporter = false;
    private static string mapProgressDataSavePath = Path.Combine(Application.persistentDataPath, "MapProgressData.json");

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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            mapDesignDataList       = await LoadMapDesignData();
            decorObjDataList        = await LoadDecorObjData();
            wayPointDataList        = await LoadWaypointData();
            towerDataList           = await LoadTowerData();
            bulletDataList          = await LoadBulletData();
            bulletEffectDataList    = await LoadBulletEffectData();
            unitDataList            = await LoadUnitData();
            skillDataList           = await LoadSkillData();
            enemyWaveDataList       = await LoadEnemyWaveData();
        }
        else
        {
            mapDesignDataList       = await LoadMapDesignDataFromSever();
            decorObjDataList        = await LoadDecorObjDataFromSever();
            wayPointDataList        = await LoadWaypointDataFromSever();
            towerDataList           = await LoadTowerDataFromSever();
            bulletDataList          = await LoadBulletDataFromSever();
            bulletEffectDataList    = await LoadBulletEffectDataFromSever();
            unitDataList            = await LoadUnitDataFromSever();
            skillDataList           = await LoadSkillDataFromSever();
            enemyWaveDataList       = await LoadEnemyWaveDataFromSever();
        }

        mapProgressDataList         = await LoadMapProgressDataAsync();
        
    }

    #region  MAP DESIGNE DATA
    private static async Task<List<MapDesignData>> LoadMapDesignDataFromSever()
    {
        List<MapDesignData> dataList = await JSONListLoader<MapDesignData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<MapDesignData>> LoadMapDesignData()
    {
        List<MapDesignData> dataList = await JSONListLoader<MapDesignData>.LoadTextFileAsync(AssetPathManager.mapDesignDataJsonPath);
        return dataList;
    }
    #endregion

    #region  MAP PROGRESS DATA
    public static void SaveMapProgressData(List<MapProgressData> mapProgressDataList)
    {
        string json = JsonConvert.SerializeObject(mapProgressDataList, Formatting.Indented, new Vector2Converter());
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            PlayerPrefs.SetString("MapProgressData", json);
            PlayerPrefs.Save();
        }
        else
        {
            File.WriteAllText(mapProgressDataSavePath, json);
            Debug.Log("Create completed MapProgressData JSON file");
        }
    }

    private static async Task<List<MapProgressData>> LoadMapProgressDataAsync()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return await LoadFromPlayerPrefsAsync();
        }
        else
        {
            return await LoadFromAddressableOrFileAsync();
        }
    }

    private static async Task<List<MapProgressData>> LoadFromPlayerPrefsAsync()
    {
        if (PlayerPrefs.HasKey("MapProgressData"))
        {
            hasSaveGameReporter = true;
            string json = PlayerPrefs.GetString("MapProgressData");
            mapProgressDataList = JsonConvert.DeserializeObject<List<MapProgressData>>(json);
        }
        else
        {
            hasSaveGameReporter = false;
            // if there is no PlayerPrefs yet => Load from StreamingAssets 
            mapProgressDataList = await JSONListLoader<MapProgressData>.LoadTextFileAsync(AssetPathManager.mapProgressDataJsonPath);
            string json = JsonConvert.SerializeObject(mapProgressDataList, Formatting.Indented, new Vector2Converter());
            PlayerPrefs.SetString("MapProgressData", json);
            // for WebGL
            PlayerPrefs.Save();
        }
        return mapProgressDataList;
    }

    private static async Task<List<MapProgressData>> LoadFromAddressableOrFileAsync()
    {
        // if had save data => Load from mapProgressDataSavePath
        if (File.Exists(mapProgressDataSavePath))
        {
            hasSaveGameReporter = true;
            string json = File.ReadAllText(mapProgressDataSavePath);
            mapProgressDataList = JsonConvert.DeserializeObject<List<MapProgressData>>(json);
        }
        else
        {
            hasSaveGameReporter = false;
            // if not => Load from sever
            mapProgressDataList = await JSONListLoader<MapProgressData>.LoadJSONListAsync();
        }
        return mapProgressDataList;
    }

    public static bool HasSaveGameData()
    {
        return hasSaveGameReporter;
    }

    public static async Task ResetMapProgressData()
    {
        DeleteCurrentMapProgressSaveData();

        mapProgressDataList = await LoadMapProgressDataAsync();
        hasSaveGameReporter = true;
    }

    private static void DeleteCurrentMapProgressSaveData()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (PlayerPrefs.HasKey("MapProgressData"))
            {
                PlayerPrefs.DeleteKey("MapProgressData");
                Debug.Log("Delete MapProgressData PlayerPrefs");
            }
        }
        else
        {
            if (File.Exists(mapProgressDataSavePath))
            {
                File.Delete(mapProgressDataSavePath);
                hasSaveGameReporter = false;
                Debug.Log("Delete MapProgressData JSON");
            }
        }
        
    }
    #endregion

    #region DECOR OBJECT DATA
    private static async Task<List<DecorObjData>> LoadDecorObjDataFromSever()
    {
        List<DecorObjData> dataList = await JSONListLoader<DecorObjData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<DecorObjData>> LoadDecorObjData()
    {
        List<DecorObjData> dataList = await JSONListLoader<DecorObjData>.LoadTextFileAsync(AssetPathManager.decorObjDataJsonPath);
        return dataList;
    }
    #endregion

    #region WAY POINT DATA
    private static async Task<List<WayPointData>> LoadWaypointDataFromSever()
    {
        List<WayPointData> dataList = await JSONListLoader<WayPointData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<WayPointData>> LoadWaypointData()
    {
        List<WayPointData> dataList = await JSONListLoader<WayPointData>.LoadTextFileAsync(AssetPathManager.waypointDataJsonPath);
        return dataList;
    }
    #endregion

    #region TOWER DATA
    private static async Task<List<TowerData>> LoadTowerDataFromSever()
    {
        List<TowerData> dataList = await JSONListLoader<TowerData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<TowerData>> LoadTowerData()
    {
        List<TowerData> dataList = await JSONListLoader<TowerData>.LoadTextFileAsync(AssetPathManager.towerDataJsonPath);
        return dataList;
    }
    #endregion

    #region BULLET DATA
    private static async Task<List<BulletData>> LoadBulletDataFromSever()
    {
        List<BulletData> dataList = await JSONListLoader<BulletData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<BulletData>> LoadBulletData()
    {
        List<BulletData> dataList = await JSONListLoader<BulletData>.LoadTextFileAsync(AssetPathManager.bulletDataJsonPath);
        return dataList;
    }
    #endregion

    #region BULLET EFFECT DATA
    private static async Task<List<BulletEffectData>> LoadBulletEffectDataFromSever()
    {
        List<BulletEffectData> dataList = await JSONListLoader<BulletEffectData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<BulletEffectData>> LoadBulletEffectData()
    {
        List<BulletEffectData> dataList = await JSONListLoader<BulletEffectData>.LoadTextFileAsync(AssetPathManager.bulletEffectDataJsonPath);
        return dataList;
    }
    #endregion

    #region UNIT DATA
    private static async Task<List<UnitData>> LoadUnitDataFromSever()
    {
        List<UnitData> dataList = await JSONListLoader<UnitData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<UnitData>> LoadUnitData()
    {
        List<UnitData> dataList = await JSONListLoader<UnitData>.LoadTextFileAsync(AssetPathManager.unitDataJsonPath);
        return dataList;
    }
    #endregion

    #region SKILL DATA
    private static async Task<List<SkillData>> LoadSkillDataFromSever()
    {
        List<SkillData> dataList = await JSONListLoader<SkillData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<SkillData>> LoadSkillData()
    {
        List<SkillData> dataList = await JSONListLoader<SkillData>.LoadTextFileAsync(AssetPathManager.SkillDataJsonPath);
        return dataList;
    }
    #endregion

    #region ENEMY WAVE DATA
    private static async Task<List<EnemyWaveData>> LoadEnemyWaveDataFromSever()
    {
        List<EnemyWaveData> dataList = await JSONListLoader<EnemyWaveData>.LoadJSONListAsync();
        return dataList;
    }

    private static async Task<List<EnemyWaveData>> LoadEnemyWaveData()
    {
        List<EnemyWaveData> dataList = await JSONListLoader<EnemyWaveData>.LoadTextFileAsync(AssetPathManager.enemyWaveDataJsonPath);
        return dataList;
    }
    #endregion
}