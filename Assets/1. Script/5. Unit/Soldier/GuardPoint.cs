using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPoint : MonoBehaviour
{
    public List<Transform> guardpoint = new List<Transform>();
    public List<Soldier> soldiers = new List<Soldier>();

    public void MoveSoldierToGuardPoint()
    {
        for ( int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].Move(guardpoint[i].position);
        }
    }

    public void StartMoveSoldierToGuardPoint()
    {
        for ( int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].isReachGuardPos = false;
        }
    }
}
