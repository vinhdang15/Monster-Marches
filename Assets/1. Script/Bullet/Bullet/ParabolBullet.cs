using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolBullet : BulletBase
{
    private float height = 1.5f;
    private float elapedtime = 0f;

    // use adjustBulletTypeSpeed to convert ParabolBullet speed value
    // to the same frame of reference as magic ball bullet speed value
    private float adjustBulletTypeSpeed = 0.18f;
    
    public override void MoveToTarget()
    {   
        if(isSetUpStartPos == false) return;
        if(isReachEnemyPos == true) return;

        elapedtime += Time.deltaTime;

        // percent complete
        float t = Mathf.Clamp01(elapedtime * Speed * adjustBulletTypeSpeed);

        // update targer pos
        UpdateEnemyPos();
        
        // Update X coordinates
        float x = Mathf.Lerp(startPos.x, enemyPos.x, t);

        // Update Y coordinates
        // áp gia tốc khi để mũi tên bay chậm hơn bay lao lên, và nhanh hơn khi lao xuống 
        // float adjustedT = EaseInOut(t);
        float y = Mathf.Lerp(startPos.y, enemyPos.y, t) + height * (1 - 4 * (t - 0.5f) * (t - 0.5f));

        transform.position = new Vector2(x,y);
        UpdateBulletDirection();

        if((Vector2)transform.position == enemyPos)
        {
            isReachEnemyPos = true;
            PlayAnimationWhenReachEnemyPos();
            ApplyBulletEffect();
        }
    }

    private float EaseInOut(float t)
{
    if (t < 0.5f)
    {
        // return 2 * t * t; // Ease-In
        return t * 0.85f; // Ease-In
    }
    else
    {
        // return 1 - Mathf.Pow(-2 * t + 2, 2) / 2; // Ease-Out
        return t * t;
    }
}

    public override void ResetBullet()
    {
        base.ResetBullet();
        elapedtime = 0f;
    }
}
