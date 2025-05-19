using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> ActiveUnitList = new();
    public int totalEnemiesDie = 0;
    public event Action<UnitBase> OnEnemyDeath;
    public event Action OnEnemyReachEndPoint;

    public void ResetEnemyManager()
    {
        totalEnemiesDie = 0;
        foreach(Enemy enemy in ActiveUnitList)
        {
            UnitPool.Instance.ReturToUnitPool(enemy);
        }
        ActiveUnitList.Clear();
    }

    private void OnDisable()
    {
        OnEnemyDeath = null;
        OnEnemyReachEndPoint = null;
    }

    private void Update()
    {
        foreach(var enemy in ActiveUnitList)
        {
            enemy.EnemyAction();
            enemy.SetMovingDirection();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemy.OnEnemyDeath          += HandleEnemyDeath;
        enemy.OnEnemyReachEndPoint  += HandleEnemyReachEndPoint;
        ActiveUnitList.Add(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        ActiveUnitList.Remove(enemy);
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
        ActiveUnitList.Remove(enemy);
        totalEnemiesDie++;
        OnEnemyReachEndPoint?.Invoke();
        UnitPool.Instance.ReturToUnitPool(enemy);
    }
}
