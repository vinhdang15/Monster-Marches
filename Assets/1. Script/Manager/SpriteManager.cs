using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class SpriteManager
{
    private static Dictionary<string, Sprite> spritesDic = new();

    public static async Task PreloadSpritesWithLabel(string label)
    {
        var handle = Addressables.LoadAssetsAsync<Sprite>(label,null);
        await handle.Task;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach(var sprite in handle.Result)
            {
                string key = sprite.name;
                if(!spritesDic.ContainsKey(key))
                {
                    spritesDic.Add(key, sprite);
                }
            }
        }
    }

    public static Sprite GetMapSprite(string mapID)
    {
        if(spritesDic.TryGetValue(mapID, out Sprite sprite))
        {
            return sprite;
        }
        
        Debug.Log($"there is no {mapID} image");
        return null;
    }
}