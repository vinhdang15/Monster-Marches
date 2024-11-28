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
    public virtual IEnumerator PlayDealDamageAnimation()
    {
        animator.SetTrigger("DealDamage");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield break;
    }

    public virtual float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
