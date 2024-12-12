using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEvent : MonoBehaviour
{
    UnitBase unitBase;
    private void Awake()
    {
        GetUnitBase();
    }

    private void GetUnitBase()
    {
        unitBase = transform.parent.GetComponent<UnitBase>();
    }

    // animation event function
    public void DealDamageToUnit()
    {
        unitBase.DealDamage();
    }
}
