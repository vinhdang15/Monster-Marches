using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;
using System.IO;

public static class JSONListLoader<T>
{
    #region  FOR PC VERSION
    // private static string persistentDataPath = Application.persistentDataPath;

    // private static readonly JsonSerializerSettings settings = new()
    // {
    //     NullValueHandling = NullValueHandling.Ignore,
    //     MissingMemberHandling = MissingMemberHandling.Ignore
    // };
    // /// Load JSON, parse into List<T>
    // public static async Task<List<T>> LoadJSONListAsync()
    // {
    //     // set persistentDataPath to save JSON when have internet
    //     string localPath = SetLocalPath();

    //     var handle = Addressables.LoadAssetsAsync<TextAsset>(AddressLabel.JSON.ToString(), null);
    //     await handle.Task;

    //     if (handle.Status != AsyncOperationStatus.Succeeded)
    //     {
    //         Debug.LogError($"Failed to load JSON assets with label '{AddressLabel.JSON}' from sever");
    //         // return LoadJSONListFromPersistentDataPath(localPath);
    //     }


    //     // var handle = Addressables.LoadAssetsAsync<TextAsset>(AddressLabel.JSON.ToString(), null);
    //     // var completed = await Task.WhenAny(handle.Task, Task.Delay(5000));

    //     // if (completed != handle.Task || handle.Status != AsyncOperationStatus.Succeeded)
    //     // {
    //     //     Debug.LogWarning("Không thể tải JSON từ Addressables, fallback sang local.");
    //     //     return LoadJSONListFromPersistentDataPath(localPath);
    //     // }


    //     List<T> dataList = new();
    //     foreach (var textAsset in handle.Result)
    //     {
    //         if (textAsset.name == typeof(T).Name)
    //         {
    //             dataList = JsonConvert.DeserializeObject<List<T>>(textAsset.text, settings);
    //             // SaveJsonAtPersistentDataPath(dataList, localPath);
    //         }
    //     }

    //     Addressables.Release(handle);
    //     return dataList;
    // }

    // // save JSON from sever to PersistentDataPath
    // private static void SaveJsonAtPersistentDataPath(List<T> dataList, string localPath)
    // {
    //     string json = JsonConvert.SerializeObject(dataList, Formatting.Indented, new Vector2Converter());
    //     File.WriteAllText(localPath, json);
    //     Debug.Log($"save completed {typeof(T).Name} JSON file to PersistentDataPath");
    // }

    // // if there is no internet load JSON from PersistentDataPath
    // private static List<T> LoadJSONListFromPersistentDataPath( string localPath)
    // {
    //     List<T> dataList = new();
    //     if (File.Exists(localPath))
    //     {
    //         string json = File.ReadAllText(localPath);
    //         dataList = JsonConvert.DeserializeObject<List<T>>(json);
    //     }
    //     Debug.Log("Load from localPath");
    //     return dataList;
    // }

    // private static string SetLocalPath()
    // {
    //     string fileName = typeof(T).Name + ".json";
    //     return Path.Combine(persistentDataPath, fileName);
    // }
    #endregion

    private static readonly JsonSerializerSettings settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };
    /// Load JSON, parse into List<T>
    public static async Task<List<T>> LoadJSONListAsync()
    {
        var handle = Addressables.LoadAssetsAsync<TextAsset>(AddressLabel.JSON.ToString(), null);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load JSON assets with label '{AddressLabel.JSON}' from sever");
        }

        List<T> dataList = new();
        foreach (var textAsset in handle.Result)
        {
            if (textAsset.name == typeof(T).Name)
            {
                dataList = JsonConvert.DeserializeObject<List<T>>(textAsset.text, settings);
            }
        }

        Addressables.Release(handle);
        return dataList;
    }
}