using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackTowerView : TowerViewBase
{
    [SerializeField] Transform spawnSoldierTrans;

    public Vector2 GetSpawnSoldierPos()
    {
        return spawnSoldierTrans.position;
    }

    public void OpenGateAnimation()
    {
        towerAnimation.PlaySpawnSoldier();
    }
}
