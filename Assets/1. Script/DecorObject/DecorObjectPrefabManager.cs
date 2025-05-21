using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class DecorObjectPrefabManager
{
    private static Dictionary<string, GameObject> decorObjectPrefabDic = new();

    public static async Task PreloadAllEnvironmentObjPrefalb()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(AddressLabel.DecorObject.ToString(), null);
        await handle.Task;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach(var obj in handle.Result)
            {
                string key = obj.name;
                if(!decorObjectPrefabDic.ContainsKey(key))
                {
                    decorObjectPrefabDic.Add(key, obj);
                }
            }
        }
    }

    public static Dictionary<string, GameObject> GetDecorObjectPrefabDic()
    {
        return decorObjectPrefabDic;
    }
}
