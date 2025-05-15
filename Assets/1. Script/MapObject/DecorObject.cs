using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorObject : MonoBehaviour
{
    public bool isStaticObj;
    public bool isAnimatedObj;
    public bool isDecayObj;
    [HideInInspector] public string decorID;
    [SerializeField] private List<Sprite> sprites = new();
    [SerializeField] private SpriteRenderer spriteRenderer;
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

    public void ChangeSprite()
    {
        if(index >= sprites.Count - 1) return;
        index++;
        spriteRenderer.sprite = sprites[index];
    }

    public void LoopChangeSprite()
    {
        if(index >= sprites.Count - 1) index = 0;
        else index++;
        spriteRenderer.sprite = sprites[index];
    }

    public void StartHealing()
    {
        if(index < 1) return;
        index--;
        spriteRenderer.sprite = sprites[index];
    }

    public void SetDefaultSprite()
    {
        index = 0;
        spriteRenderer.sprite = sprites[index];
    }

    public void SetRandomDefaultSprite()
    {
        index = Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[index];
    }
}
