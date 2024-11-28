using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolBullet : BulletBase
{
    [HideInInspector] public Vector2 instantiatePos;
    [HideInInspector] public int Trajectory_num = 50;
    private float initialAngleDeg;
    private float initialAngleRad;
    private float config = 0.1f;
    private float V;
    private float gravity = 9.8f;

    private List<Vector2> trajectoryPoints = new List<Vector2>();
    private float speedY = 0f;
    // LineRenderer lineRenderer;

    protected override void Awake()
    {
        base.Start();
        // lineRenderer = GetComponent<LineRenderer>();
        // lineRenderer.positionCount = Trajectory_num;
    }

    protected virtual void CalTrajectory()
    {
        if(targetEnemy == null) return;
        CalInitialAngle();
        calV();
        trajectoryPoints.Clear();
        for(int i = 0; i < Trajectory_num; i++)
        {
            float time = i * config;
            float X = V * Mathf.Cos(initialAngleRad) * time;
            float Y = V * Mathf.Sin(initialAngleRad) * time - 0.5f * gravity * time * time;
            Vector2 pos = instantiatePos + new Vector2(X, Y);
            // lineRenderer.SetPosition(i, pos);
            if(trajectoryPoints.Count <= i)
            {
                trajectoryPoints.Add(pos);
            }
            else
            {
                trajectoryPoints[i] = pos;
            }      
        }
    }

    private void CalInitialAngle()
    {
        float X = targetEnemy.transform.position.x - instantiatePos.x;
        float Y = targetEnemy.transform.position.y - instantiatePos.y;
        Vector2 dir = new Vector2(X, Y).normalized;
        float dotProduct = Vector2.Dot(dir, Vector2.up);

        float angleRad = Mathf.Acos(dotProduct);
        initialAngleDeg = 90 - angleRad * Mathf.Rad2Deg/2;
        initialAngleDeg = Mathf.Clamp(90 - angleRad * Mathf.Rad2Deg / 2, 60, 90);
        initialAngleRad = initialAngleDeg * Mathf.Deg2Rad;
    }

    private void calV()
    {
        float X = targetEnemy.transform.position.x - instantiatePos.x;
        float Y = targetEnemy.transform.position.y - instantiatePos.y;
        
        if(X < 0)
        {
            initialAngleRad = -Mathf.Abs(initialAngleRad);
            config = -Mathf.Abs(config);
        }
        else
        {
            initialAngleRad = Mathf.Abs(initialAngleRad);
            config = Mathf.Abs(config);
        }

        float v2 = gravity * X * X / ((Mathf.Tan(initialAngleRad) * X - Y) * 2 * Mathf.Cos(initialAngleRad) * Mathf.Cos(initialAngleRad));
        v2 = Mathf.Abs(v2);
        V = Mathf.Sqrt(v2);
    }

    protected override void AdjustBulletSpeed()
    {
        if(bulletLastPos.y > transform.position.y)
        {
            speedY = Speed * 1.2f; 
        }
        else
        {
            speedY = Speed * 0.7f;
        }
    }

    public override IEnumerator MoveToTargetCoroutine()
    {
        // only init instantiatePoint when start coroutine
        instantiatePos = transform.position;
        
        CalTrajectory();
        for (int i = 0; i < trajectoryPoints.Count; i++)
        {  
            Vector2 position = trajectoryPoints[i];
            while ((Vector2)transform.position != position)
            {
                transform.position = Vector2.MoveTowards(transform.position, position, speedY * Time.deltaTime);
                UpdateBulletSpeedAndDirection();
                CalTrajectory();
                UpdateEnemyPos();
                if(Vector2.Distance(transform.position, enemyPos) <= 0.2f)
                {
                    isReachEnemyPos = true;
                    StartCoroutine(DealDamageToEnemy());
                    yield break;
                }
                yield return null;
            } 
        }  
    }
}
