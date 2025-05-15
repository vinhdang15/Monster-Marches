using System.Threading.Tasks;
using UnityEngine;

public class MapDisplayController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigi2D;
    private PolygonCollider2D polygonCollider2D;

    public async Task PrepareGameAsync()
    {
        await MapSpriteManager.PreloadAllMapSprites();
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = true;
        rigi2D =  gameObject.AddComponent<Rigidbody2D>();
        rigi2D.bodyType = RigidbodyType2D.Static;
    }

    public void LoadIntroSprite()
    {
        Sprite mapSelectionSprite = MapSpriteManager.GetMapSprite("Intro");
        spriteRenderer.sprite = mapSelectionSprite;
        ShowMapImage();
        UpdatePolygonColliderToMatchSprite();
    }

    public void LoadSelectedMapSprite(MapData mapData)
    {
        int mapID = mapData.mapID;
        Sprite mapSprite = MapSpriteManager.GetMapSprite(mapID.ToString());
        spriteRenderer.sprite = mapSprite;
        ShowMapImage();
        UpdatePolygonColliderToMatchSprite();
    }

    public void LoadWorldMapSprite()
    {
        Sprite WorldMapSprite = MapSpriteManager.GetMapSprite("WorldMap");
        spriteRenderer.sprite = WorldMapSprite;
        ShowMapImage();
        UpdatePolygonColliderToMatchSprite();
    }

    public PolygonCollider2D GetPolygonCollider2D()
    {
        return polygonCollider2D;
    }

    private void UpdatePolygonColliderToMatchSprite()
{
    if (spriteRenderer.sprite != null && polygonCollider2D != null)
    {
        Destroy(polygonCollider2D);
        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
    }
    else
    {
        Debug.LogWarning("SpriteRenderer or PolygonCollider2D is not assigned!");
    }
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
