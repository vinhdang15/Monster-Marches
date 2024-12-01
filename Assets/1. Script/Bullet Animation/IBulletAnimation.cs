using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletAnimation
{
    IEnumerator PlayHitNullAnimation();
    void PlayDealDamageAnimation();
    float GetCurrentAnimationLength();
}
