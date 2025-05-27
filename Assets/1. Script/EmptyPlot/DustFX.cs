using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFX : MonoBehaviour
{
    public bool IsRunning { get; private set; }
    [SerializeField] private List<Sprite> sprites = new();
    private SpriteRenderer spriteRenderer;
    private float frameRate = 0.05f;
    public void PrepareGame()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public IEnumerator Play(Vector2 pos)
    {
        transform.position = pos;
        IsRunning = true;

        for (int i = 1; i < sprites.Count; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameRate);
        }
        IsRunning = false;
    }
}
