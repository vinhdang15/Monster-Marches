using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public event Action<UnitBase> EnemyDieHandler;
    
    private void Update()
    {
        foreach(var enemy in enemies)
        {
            enemy.Move();
            enemy.SetMovingDirection();
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemy.OnEnemyDeath += HandleEnemyDeath;
        enemies.Add(enemy);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        EnemyDieHandler?.Invoke(enemy);
        enemies.Remove(enemy);
    }
}
