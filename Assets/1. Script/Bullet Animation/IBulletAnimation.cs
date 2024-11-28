using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletAnimation
{
    //void PlayMoveAnimation();
    IEnumerator PlayDealDamageAnimation();
    float GetCurrentAnimationLength();
}
