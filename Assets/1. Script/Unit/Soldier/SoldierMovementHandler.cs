using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMovementHandler : MonoBehaviour
{
    private Soldier soldier;

    public void Init(Soldier soldier)
    {
        this.soldier = soldier;
    }

    public void MoveTo(Vector2 pos)
    {
        soldier.unitAnimation.UnitPlayWalk();
        transform.position = Vector2.MoveTowards(transform.position, pos, soldier.CurrentSpeed *Time.deltaTime);
    }
}
