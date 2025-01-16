using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public UnitPool unitPool;
    public List<Enemy> ActiveEnemies = new List<Enemy>();
    public int totalEnemiesDie = 0;
    public event Action<UnitBase> OnEnemyDeath;
    public event Action OnEnemyReachEndPoint;
    
    private void Start()
    {
        foreach(var enemy in ActiveEnemies)
        {
            enemy.GetAnimation();
            enemy.unitAnimation.UnitPlayWalk();
        }
    }

    private void OnDisable()
    {
        OnEnemyDeath = null;
        OnEnemyReachEndPoint = null;
    }

    private void Update()
    {
        foreach(var enemy in ActiveEnemies)
        {
            enemy.EnemyAction();
            enemy.SetMovingDirection();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemy.OnEnemyDeath          += HandleEnemyDeath;
        enemy.OnEnemyReachEndPoint  += HandleEnemyReachEndPoint;
        ActiveEnemies.Add(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        ActiveEnemies.Remove(enemy);
        // notifi for GamePlayManaer
        totalEnemiesDie++;
        OnEnemyDeath?.Invoke(enemy);
        //Play die animation
        enemy.unitAnimation.UnitPlayDie();
        // wait to finish die animation then return unit pool
        StartCoroutine(enemy.ReturnPoolAfterPlayAnimation(unitPool));

       
    }

    private void HandleEnemyReachEndPoint(Enemy enemy)
    {
        ActiveEnemies.Remove(enemy);
        totalEnemiesDie++;
        OnEnemyReachEndPoint?.Invoke();
        unitPool.ReturnEnemy(enemy);
    }
}
