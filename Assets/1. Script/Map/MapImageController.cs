using UnityEngine;

public class MapImageController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void PrepareGame()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();  
    }

    public void LoadSpriteImage(MapData mapData)
    {
        int mapID = mapData.mapID;
        ShowMapImage();
        Sprite mapSprite = MapImageHandler.GetMapSprite(mapID);
        spriteRenderer.sprite = mapSprite;
    }

    private void ShowMapImage()
    {
        gameObject.SetActive(true);
    }
    public void HideMapImage()
    {
        gameObject.SetActive(false);
    }
}
