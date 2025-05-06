using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFX : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new();
    private SpriteRenderer spriteRenderer;
    private float frameRate = 0.05f;
    public void PrepareGame()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayBuildingFX(Vector2 vector2)
    {
        SetPos(vector2);
        StartCoroutine(PlayBuildingFXCoroutine());
    }

    private void SetPos(Vector2 vector2)
    {
        transform.position = vector2;
    }

    private IEnumerator PlayBuildingFXCoroutine()
    {
        for(int i = 1; i < sprites.Count; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameRate);
        }
    }
}
