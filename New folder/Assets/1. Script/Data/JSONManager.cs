using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class JSONManager
{
    private static string mapDataJsonPath = Path.Combine(Application.persistentDataPath, "JSON/mapData.json");

    public static void SaveMapDataToJson(List<MapData> mapDataList)
    {
        string json = JsonConvert.SerializeObject(mapDataList, Formatting.Indented);
        File.WriteAllText(mapDataJsonPath, json);
        Debug.Log("Create completed JSON file");
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
}