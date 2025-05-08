using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePatch : MonoBehaviour
{
    public string treePatchID;
    [SerializeField] private List<Sprite> sprites = new();

    private SpriteRenderer spriteRenderer;
    [SerializeField] private int index = 0;

    public void PrepareGame()
    {
        GetComponents();
        SetDefaultSprite();
    }

    private void GetComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartDecay()
    {
        if(index >= sprites.Count -1) return;
        index++;
        spriteRenderer.sprite = sprites[index];
    }

    public void SetDefaultSprite()
    {
        index = 0;
        spriteRenderer.sprite = sprites[index];
    }
}
