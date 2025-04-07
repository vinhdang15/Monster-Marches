using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class JSONManager
{
    private static string mapDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/MapData.json");
    private static string waypointDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/WayPointData.json");
    private static string towerDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/TowerData.json");
    private static string bulletDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/BulletData.json");
    private static string unitDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/UnitData.json");
    private static string SkillDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/SkillData.json");
    private static string bulletEffectDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/BulletEffectData.json");

    #region  MAP DATA
    public static void SaveMapDataToJson(List<MapData> mapDataList)
    {
        string json = JsonConvert.SerializeObject(mapDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(mapDataJsonPath, json);
        Debug.Log("Create completed MapData JSON file");
    }

    public static List<MapData> LoadMapDataFromJson()
    {
        if(!File.Exists(mapDataJsonPath))
        {
            Debug.Log("there is no MapData Json file");
            return new List<MapData>();
        }
        string json = File.ReadAllText(mapDataJsonPath);

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        return JsonConvert.DeserializeObject<List<MapData>>(json);
    }
    #endregion 

    #region WAY POINT DATA
    public static void SaveWayPointDataToJson(List<WayPointData> wayPointDataList) 
    {
        string json = JsonConvert.SerializeObject(wayPointDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(waypointDataJsonPath, json);
        Debug.Log("Create completed WayPointData JSON file");
    }

    public static List<WayPointData> LoadMapWayPointDataFromJson()
    {
        if(!File.Exists(waypointDataJsonPath))
        {
            Debug.Log("there is no WayPointData Json file");
            return null;
        }
        string json = File.ReadAllText(waypointDataJsonPath);
        
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        return JsonConvert.DeserializeObject<List<WayPointData>>(json, settings);
    }
    #endregion

    #region TOWER DATA
    public static void SaveTowerDataToJson(List<TowerData> towerDataList)
    {
        string json = JsonConvert.SerializeObject(towerDataList, Formatting.Indented);
        File.WriteAllText(towerDataJsonPath, json);
        Debug.Log("Create completed towerData JSON file");
    }

    public static List<TowerData> LoadTowerDataFromJson()
    {
        if(!File.Exists(towerDataJsonPath))
        {
            Debug.Log("there is no TowerData Json file");
            return new List<TowerData>();
        }
        string json = File.ReadAllText(towerDataJsonPath);
        return JsonConvert.DeserializeObject<List<TowerData>>(json);
    }
    #endregion

    #region BULLET DATA
    public static void SaveBulletDataToJson(List<BulletData> bulletDataList)
    {
        string json = JsonConvert.SerializeObject(bulletDataList, Formatting.Indented);
        File.WriteAllText(bulletDataJsonPath, json);
        Debug.Log("Create completed bulletData JSON file");
    }

    public static List<BulletData> LoadBulletDataFromJson()
    {
        if(!File.Exists(bulletDataJsonPath))
        {
            Debug.Log("there is no bulletData Json file");
            return new List<BulletData>();
        }
        string json = File.ReadAllText(bulletDataJsonPath);
        return JsonConvert.DeserializeObject<List<BulletData>>(json);
    }
    #endregion

    #region BULLET EFFECT DATA
    public static void SaveBulletEffectDataToJson(List<BulletEffectData> bulletEffectDataList)
    {
        string json = JsonConvert.SerializeObject(bulletEffectDataList, Formatting.Indented);
        File.WriteAllText(bulletEffectDataJsonPath, json);
        Debug.Log("Create completed BulletEffectData JSON file");
    }

    public static List<BulletEffectData> LoadBulletEffectDataFromJson()
    {
        if(!File.Exists(bulletEffectDataJsonPath))
        {
            Debug.Log("there is no BulletEffectData Json file");
            return new List<BulletEffectData>();
        }
        string json = File.ReadAllText(bulletEffectDataJsonPath);
        return JsonConvert.DeserializeObject<List<BulletEffectData>>(json);
    }
    #endregion

    #region UNIT DATA
    public static void SaveUnitDataToJson(List<UnitData> unitDataList)
    {
        string json = JsonConvert.SerializeObject(unitDataList, Formatting.Indented);
        File.WriteAllText(unitDataJsonPath, json);
        Debug.Log("Create completed UnitData JSON file");
    }

    public static List<UnitData> LoadUnitDataFromJson()
    {
        if(!File.Exists(unitDataJsonPath))
        {
            Debug.Log("there is no bulletData Json file");
            return new List<UnitData>();
        }
        string json = File.ReadAllText(unitDataJsonPath);
        return JsonConvert.DeserializeObject<List<UnitData>>(json);
    }
    #endregion

    #region SKILL DATA
    public static void SaveSkillDataToJson(List<SkillData> skillDataList)
    {
        string json = JsonConvert.SerializeObject(skillDataList, Formatting.Indented);
        File.WriteAllText(SkillDataJsonPath, json);
        Debug.Log("Create completed SkillData JSON file");
    }

    public static List<SkillData> LoadSkillDataFromJson()
    {
        if(!File.Exists(SkillDataJsonPath))
        {
            Debug.Log("there is no bulletData Json file");
            return new List<SkillData>();
        }
        string json = File.ReadAllText(SkillDataJsonPath);
        return JsonConvert.DeserializeObject<List<SkillData>>(json);
    }
    #endregion
}