using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
}
