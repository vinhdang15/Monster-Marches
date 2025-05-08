using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class TreePatchPrefabManager
{
    private static Dictionary<string, GameObject> TreePrefabDic = new();

    public static async Task PreloadAllTreePrefab()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(AddressLabel.TreePatch.ToString(), null);
        await handle.Task;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach(var treePatch in handle.Result)
            {
                string key = treePatch.name;
                if(!TreePrefabDic.ContainsKey(key))
                {
                    TreePrefabDic.Add(key, treePatch);
                }
            }
        }
    }

    public static Dictionary<string, GameObject> GetTreePatchPrefabDic()
    {
        return TreePrefabDic;
    }
}
