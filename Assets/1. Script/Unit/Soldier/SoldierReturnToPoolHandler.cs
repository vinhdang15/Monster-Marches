using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierReturnToPoolHandler : MonoBehaviour
{
    private Soldier soldier;
    SpriteRenderer spriteRenderer;

    public void Init(Soldier soldier)
    {
        this.soldier = soldier;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator FadeOut(float time, Action oncomplete = null)
    {     
        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < time && soldier)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / time);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure the alpha is set to 0 at the end
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        oncomplete?.Invoke();
        spriteRenderer.color = originalColor;
    }
}
