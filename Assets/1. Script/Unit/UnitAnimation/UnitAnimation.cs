using System.Collections;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private Animator        animator;
    private SpriteRenderer  spriteRenderer;
    private float           attackSpeed;
    private float           randomStartTime;
    // unit can has more than one effect color, using currentColor to keep them all
    private Color           currentColor;
    private Coroutine       effectFlashColorCoroutine;

    private void Awake()
    {
        GetRandomStartTime();
        Transform sprite = transform.GetChild(0);
        spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }

    public void GetAttackSpeed(float attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }

    private void GetRandomStartTime()
    {
        randomStartTime = Random.Range(0, 0.3f);
    }
    public void GetAnimation()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    #region ANIMATION
    public void UnitPlayWalk()
    {
        animator.SetFloat("Blend", 0);
    }

    public void UnitPlayIdle()
    {
        animator.SetFloat("Blend", 1);
    }

    public void UnitPlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void UnitPlayDie()
    {
        animator.SetTrigger("Die");
    }

    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
    #endregion

    #region EFFECT COLOR
    // Effect flash color
    public void ApplyEffectFlashColor(Color effectColor)
    {
        effectFlashColorCoroutine = StartCoroutine(EffectFlashColor(effectColor));
    }

    private IEnumerator EffectFlashColor(Color effectColor)
    {
        if(effectFlashColorCoroutine != null) yield break;

        currentColor = spriteRenderer.color;
        spriteRenderer.color = effectColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = currentColor;
    }

    // Effect color
    public void ApplyEffectColor(Color effectColor)
    {
        spriteRenderer.color = effectColor;
    }

    public void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }
    #endregion
}