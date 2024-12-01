using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimation : IBulletAnimation
{
    protected Animator animator;

    public BulletAnimation(Animator animator)
    {
        this.animator = animator;
    }

    public virtual IEnumerator PlayHitNullAnimation()
    {
        animator.SetTrigger("HitNull");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("HitNull"));
        yield return new WaitForSeconds(GetCurrentAnimationLength());
        yield break;
    }

    public virtual void PlayDealDamageAnimation()
    {
        animator.SetTrigger("DealDamage");
    }

    public virtual float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
