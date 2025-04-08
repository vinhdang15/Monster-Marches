using UnityEngine;
using System.IO;

public class MapImageHandler : MonoBehaviour
{
    private static string mapFolderPath = Path.Combine(Application.persistentDataPath, "MAP");

    public static Sprite GetMapSprite(string mapID)
    {
        string mapImagePath = Path.Combine(mapFolderPath, mapID + ".png");
        if(File.Exists(mapImagePath))
        {
            byte[] imageData = File.ReadAllBytes(mapImagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            texture.Apply();

            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return newSprite;
        }
        else
        {
            Debug.Log("There is no image for mapName");
            return null;
        }
    }
}
