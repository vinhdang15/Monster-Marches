using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> ActiveEnemies = new();
    public int totalEnemiesDie = 0;
    public event Action<UnitBase> OnEnemyDeath;
    public event Action OnEnemyReachEndPoint;

    public void ClearEnemyManager()
    {
        totalEnemiesDie = 0;
        foreach(Enemy enemy in ActiveEnemies)
        {
            UnitPool.Instance.ReturnEnemy(enemy);
        }
        ActiveEnemies.Clear();
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
        // notify for GamePlayManaer
        totalEnemiesDie++;
        OnEnemyDeath?.Invoke(enemy);
        //Play die animation
        enemy.unitAnimation.UnitPlayDie();
        // wait to finish die animation then return unit pool
        StartCoroutine(enemy.ReturnPoolAfterPlayAnimation());
    }

    private void HandleEnemyReachEndPoint(Enemy enemy)
    {
        ActiveEnemies.Remove(enemy);
        totalEnemiesDie++;
        OnEnemyReachEndPoint?.Invoke();
        UnitPool.Instance.ReturnEnemy(enemy);
    }
}
