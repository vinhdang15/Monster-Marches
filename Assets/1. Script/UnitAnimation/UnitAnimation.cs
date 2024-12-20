using System.Collections;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private Animator        animator;
    private SpriteRenderer  spriteRenderer;
    private float attackSpeed;
    private float randomStartTime;
    private Coroutine attackCoroutine;

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
        randomStartTime = Random.Range(0,0.3f);
    }
    public void GetAnimation()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void SpriteSortingOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    public void UnitPlayWalk()
    {
        animator.SetFloat("Blend",0);
    }

    public void UnitPlayIdle()
    {
        animator.SetFloat("Blend",1);
    }

    public void UnitPlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void UnitPlaySpecialAbility()
    {
        // animator.SetFloat("Blend",3);
    }

    public void UnitPlayDie()
    {
        animator.SetTrigger("Die");
    }

    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void ResetColor()
    {
        spriteRenderer.color = Color.white;
    }
}
