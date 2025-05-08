using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EmptyPlot : MonoBehaviour
{
    public float X { get; private set; }
    public float Y { get; private set; }
    [SerializeField] private List<Sprite> sprites = new();
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;
    private float frameRate = 0.2f;

    public void PrepareGame()
    {
        GetComponents();
        SetDefaultSprite();
    }
    
    private void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    
    public Vector2 GetPos()
    {
        return transform.position;
    }

    public void PlayBuildingFX(Action HandleDustFX, Action InitTower)
    {
        StartCoroutine(PlayBuildingFXCoroutine(HandleDustFX, InitTower));
    }

    private IEnumerator PlayBuildingFXCoroutine(Action HandleDustFX, Action InitTower)
    {
        for(int i = 1; i < sprites.Count; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameRate);
            if(i == 1) HandleDustFX?.Invoke();
            if(i == 2) InitTower?.Invoke();
        }
        SetDefaultSprite();
    }

    private void SetDefaultSprite()
    {
         spriteRenderer.sprite = sprites[0];
    }

    public void DisableCollider()
    {
        polygonCollider2D.enabled = false;
    }

    public void EnableCollider()
    {
        polygonCollider2D.enabled = true;
    }
}
