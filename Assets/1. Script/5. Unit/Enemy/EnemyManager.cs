using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public UnitPool unitPool;
    public List<Enemy> totalenemies = new List<Enemy>();
    public event Action<UnitBase> EnemyDieHandler;
    
    private void Start()
    {
        foreach(var enemy in totalenemies)
        {
            enemy.GetAnimation();
            enemy.unitAnimation.UnitPlayWalk();
        }
    }
    private void Update()
    {
        foreach(var enemy in totalenemies)
        {
            enemy.EnemyAction();
            enemy.SetMovingDirection();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemy.OnEnemyDeath += HandleEnemyDeath;
        totalenemies.Add(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        totalenemies.Remove(enemy);
        // notifi for GamePlayManaer
        EnemyDieHandler?.Invoke(enemy);
        //Play die animation
        enemy.unitAnimation.UnitPlayDie();
        // wait to finish die animation then return unit pool
        StartCoroutine(enemy.ReturnPoolAfterPlayAnimation(unitPool));
    }
}
