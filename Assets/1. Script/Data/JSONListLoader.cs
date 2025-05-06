using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;

public static class JSONListLoader<T>
{
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
            Debug.LogError($"Failed to load JSON assets with label '{AddressLabel.JSON}'");
            return null;
        }

        List<T> dataList = new();
        foreach (var textAsset in handle.Result)
        {
            if (textAsset.name == typeof(T).Name)
            dataList = JsonConvert.DeserializeObject<List<T>>(textAsset.text, settings);
        }
        return dataList;
    }
}