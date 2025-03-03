using UnityEngine;

public class TowerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void PlayShootingInLeft()
    {
        animator.SetTrigger("ShootingLeft");
    }

    public void PlayShootingInRight()
    {
        animator.SetTrigger("ShootingRight");
    }

    public void PlaySpawnSoldier()
    {
        animator.SetTrigger("SpawnSoldier");
    }

    // public bool IsAnimationPlaying(string animationName)
    // {
    //     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //     return 
    // }

    public float GetCurrentAnimationLength()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
