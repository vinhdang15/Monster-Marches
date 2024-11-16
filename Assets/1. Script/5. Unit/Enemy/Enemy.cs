using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitBase
{
    [SerializeField] PathFinder pathFinder;

    public void GetPathConfigSO(PathConfigSO _pathFinder)
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.PathConfigSO = _pathFinder;
    }
    public override void Move()
    {
        pathFinder.FollowPath(CurrentSpeed);
    }
    public override void SetMovingDirection()
    {
        if(CurrentPos == null) return;
        float x = transform.position.x - CurrentPos.x;
        if(x < 0) transform.localScale = new(-1,1);
        else if(x < 0) transform.localScale = new(1,1);
        CurrentPos = transform.position;
    }

    public void SetPosInPathWave(int _pathWaveIndex)
    {
        pathFinder.OnSetPosInPathWay(_pathWaveIndex);
    }
}
