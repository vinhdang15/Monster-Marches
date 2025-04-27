using System.Threading.Tasks;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigi2D;
    private PolygonCollider2D polygonCollider2D;

    public async Task PrepareGameAsync()
    {
        await SpriteHandler.PreloadAllMapSprites();
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = true;
        rigi2D =  gameObject.AddComponent<Rigidbody2D>();
        rigi2D.bodyType = RigidbodyType2D.Static;
    }

    public void LoadIntroSprite()
    {
        Sprite mapSelectionSprite = SpriteHandler.GetMapSprite("Intro");
        spriteRenderer.sprite = mapSelectionSprite;
        ShowMapImage();
        UpdatePolygonColliderToMatchSprite();
    }

    public void LoadSelectedMapSprite(MapData mapData)
    {
        int mapID = mapData.mapID;
        Sprite mapSprite = SpriteHandler.GetMapSprite(mapID.ToString());
        spriteRenderer.sprite = mapSprite;
        ShowMapImage();
        UpdatePolygonColliderToMatchSprite();
    }

    public void LoadMapSelectionSprite()
    {
        Sprite mapSelectionSprite = SpriteHandler.GetMapSprite("WorldMap");
        spriteRenderer.sprite = mapSelectionSprite;
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
        polygonCollider2D.pathCount = 0;
        polygonCollider2D.SetPath(0, spriteRenderer.sprite.vertices);
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
