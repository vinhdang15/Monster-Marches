using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimationFactory : MonoBehaviour
{
    public static IBulletAnimation CreateBulletAnimationHandler(string animationType, Animator animator)
    {
        switch(animationType)
        {
            case "immediate":
                return new BulletAnimation(animator);
            case "explosive":
                return new BulletExplodeAnimation(animator);
            default:
                Debug.Log("Unknow Animation Damage Type");
                return new BulletAnimation(animator);
        }
    }
}
