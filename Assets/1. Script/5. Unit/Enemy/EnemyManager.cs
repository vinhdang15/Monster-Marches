using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> Totalenemies = new List<Enemy>();
    public event Action<UnitBase> EnemyDieHandler;
    
    private void Start()
    {
        foreach(var enemy in Totalenemies)
        {
            enemy.GetAnimation();
            enemy.unitAnatation.UnitPlayWalk();
        }
    }
    private void Update()
    {
        foreach(var enemy in Totalenemies)
        {
            enemy.Move();
            enemy.SetMovingDirection();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemy.OnEnemyDeath += HandleEnemyDeath;
        Totalenemies.Add(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        EnemyDieHandler?.Invoke(enemy);
        enemy.unitAnatation.UnitPlayDie();
        Totalenemies.Remove(enemy);
    }
}
