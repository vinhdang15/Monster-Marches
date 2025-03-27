using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPoint : MonoBehaviour
{
    public List<Transform> guardpoint = new List<Transform>();
    public List<Soldier> soldiers = new List<Soldier>();
    public List<Enemy> enemiesInRange = new List<Enemy>();
    public float barackRateSpawn;
    public event Action<GuardPoint> OnBarrackDestroy;

    private void OnDisable()
    {
        OnBarrackDestroy?.Invoke(this);
    }

    public void AddSoldier(Soldier soldier)
    {
        soldier.enemiesInRange = this.enemiesInRange;
        soldiers.Add(soldier);
        soldier.unitAnimation.UnitPlayWalk();
        SetSoldiersStandingPos();
    }
    
    public void SetNewGuardPointPos(Vector2 pos)
    {
        transform.position = pos;
        SetSoldiersStandingPos();
        StartCoroutine(SoldierMovingToStandingPos());
    }

    private IEnumerator SoldierMovingToStandingPos()
    {
        foreach(var soldier in soldiers)
        {
            if(!soldier.gameObject.activeSelf) continue;
            soldier.GuardPointChange();
            yield return new WaitForSeconds(0.1f);
        }
    }

    #region SOLDIER MOVE TO GURAD POSITION
    public void SetSoldiersStandingPos()
    {
        for(int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].guardPos = guardpoint[i].position;
        }
    }
    #endregion

    #region DETECT ENEMY
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.OnEnemyDeath += HandleOnEnemyDie;
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemiesInRange.Remove(enemy);

            if(IsActiveEnemyInRange()) return;
            StartCoroutine(SoldierMovingToStandingPos());
        }
    }

    private void HandleOnEnemyDie(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }
    
    public bool IsActiveEnemyInRange()
    {
        foreach(var enemy in enemiesInRange)
        {
            if(enemy.CurrentHp > 0 || !enemy.gameObject.activeSelf) return true;
        }
        return false;
    }
    #endregion
}