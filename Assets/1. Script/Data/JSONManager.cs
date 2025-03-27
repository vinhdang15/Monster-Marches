using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class JSONManager
{
    private static string mapDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/MapData.json");
    private static string emptyPlotDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/EmptyPlotData.json");
    private static string towerDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/TowerData.json");
    private static string bulletDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/BulletData.json");
    private static string unitDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/UnitData.json");

    // For MapData
    public static void SaveMapDataToJson(List<MapData> mapDataList)
    {
        string json = JsonConvert.SerializeObject(mapDataList, Formatting.Indented);
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
        return JsonConvert.DeserializeObject<List<MapData>>(json);
    }

    // For EmptyPlot Data
    public static void SaveEmptyPlotDataToJson(List<EmptyPlotDataHolder> emptyPlotDataHolderList) 
    {
        List<EmptyPlotSerializableData> serializableDataList = new();
        foreach(EmptyPlotDataHolder i in emptyPlotDataHolderList)
        {
            EmptyPlotSerializableData serializableData = new(i.mapID, i.emptyPlotList);
            serializableDataList.Add(serializableData);
        }
        string json = JsonConvert.SerializeObject(serializableDataList, Formatting.Indented);
        File.WriteAllText(emptyPlotDataJsonPath, json);
        Debug.Log("Create completed EmptyPlotData JSON file");
    }

    public static List<EmptyPlotSerializableData> LoadEmptyPlotDataFromJson()
    {
        if(!File.Exists(emptyPlotDataJsonPath))
        {
            Debug.Log("there is no EmptyPlotData Json file");
            return null;
        }
        string json = File.ReadAllText(emptyPlotDataJsonPath);
        return JsonConvert.DeserializeObject<List<EmptyPlotSerializableData>>(json);
    }

    // For Tower Data
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

    // For Bullet Data
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

    // For Unit Data
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
}