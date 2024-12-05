using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : UnitBase
{
    public int index;
    public void Move(Vector2 guardPoint)
    {
        transform.position = Vector2.MoveTowards(transform.position, guardPoint, Speed *Time.deltaTime);
    }
    
    public override void SetMovingDirection()
    {
        
    }
}
