using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    public Animator animator;

    public void GetAnimation()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        // if(animator == null) Debug.Log("sai");
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
        animator.SetFloat("Blend",2);
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
}
