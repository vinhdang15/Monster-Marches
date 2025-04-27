using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class SpriteHandler
{
    // private string mapImageFolderPath = "Assets/AddressableFile/MapImage/";

    private static Dictionary<string, Sprite> mapSpritesDic = new();

    public static async Task PreloadAllMapSprites()
    {
        var handle = Addressables.LoadAssetsAsync<Sprite>(Address.MapImage.ToString(),null);
        await handle.Task;

        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach(var sprite in handle.Result)
            {
                string key = sprite.name;
                if(!mapSpritesDic.ContainsKey(key))
                {
                    mapSpritesDic.Add(key, sprite);
                }
            }
        }
    }

    public static Sprite GetMapSprite(string mapID)
    {
        if(mapSpritesDic.TryGetValue(mapID, out Sprite sprite))
        {
            return sprite;
        }
        
        Debug.Log($"there is no {mapID} image");
        return null;
    }
}