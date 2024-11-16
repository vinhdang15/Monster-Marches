using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    
    private void Update()
    {
        foreach(var enemy in enemies)
        {
            enemy.SetDefaultSpeed();
            enemy.Move();
            enemy.SetMovingDirection();
        }
    }
}
