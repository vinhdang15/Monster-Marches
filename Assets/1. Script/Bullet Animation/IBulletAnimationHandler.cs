using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletAnimationHandler
{
    void PlayMoveAnimation();
    void PlayDealDamageAnimation();
    float GetCurrentAnimationLength();
}
