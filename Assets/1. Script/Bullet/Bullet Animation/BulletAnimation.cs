using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void PlayHitNullAnimation()
    {
        animator.SetTrigger("HitNull");
    }

    public void PlayDealDamageAnimation()
    {
        animator.SetTrigger("DealDamage");
    }

    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void ResetSpriteAlpha()
    {
        if (spriteRenderer.color.a != 1.0f)
        {
            Color color = spriteRenderer.color;
            color.a = 1.0f;
            spriteRenderer.color = color;
        }
    }
}
