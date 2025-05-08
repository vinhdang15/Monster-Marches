using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class UnitPrefabManager
{
    private static Dictionary<string, GameObject> unitPrefabDic = new();

    public static async Task PreloadAllUnit()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(AddressLabel.Unit.ToString(), null);
        await handle.Task;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach(var unit in handle.Result)
            {
                string key = unit.name;
                if(!unitPrefabDic.ContainsKey(key))
                {
                    unitPrefabDic.Add(key, unit);
                }
            }
        }
    }

    public static GameObject GetUnitPrefab(string unitID)
    {
        if(unitPrefabDic.TryGetValue(unitID, out GameObject unit))
        {
            return unit;
        }
        
        Debug.Log($"there is no {unit} image");
        return null;
    }

    public static Dictionary<string, GameObject> GetUnitPrefabDic()
    {
        return unitPrefabDic;
    }

}
