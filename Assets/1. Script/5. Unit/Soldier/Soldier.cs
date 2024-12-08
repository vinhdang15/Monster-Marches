using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : UnitBase
{
    public int index;
    public bool isReachGuardPos = false;
    public void Move(Vector2 guardPoint)
    {
        if(!isReachGuardPos)
        {
            MoveToGuardPoint(guardPoint);
        }
        // need to set isReachGuardPos to false when pick need guard point.
        
    }

    private void MoveToGuardPoint(Vector2 guardPoint)
    {
        if((Vector2)transform.position == guardPoint)
        {
            isReachGuardPos = true;
        }
        transform.position = Vector2.MoveTowards(transform.position, guardPoint, Speed *Time.deltaTime);
    }
    
    public override void SetMovingDirection()
    {
        
    }
}
