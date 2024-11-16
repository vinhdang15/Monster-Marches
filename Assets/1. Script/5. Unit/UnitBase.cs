using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public string   Type            { get; set;}
    public string   enemyName;            
    public int      MaxHP           { get; set;}
    public float    Speed           { get; set;}
    public int      Damage          { get; set;}
    public int      Gold            { get; set;}
    public string   SpecialAbility  { get; set;}
    public float    CurrentHp       { get; set;}
    public float    CurrentSpeed    { get; set;}
    public bool     IsUnderEffect   { get; set;}
    protected Vector2 CurrentPos    { get; set;}
    [SerializeField] HealthBar healthBar;

    public virtual void InitUnit(UnitData _enemyData)
    {
        Type            = _enemyData.type;
        MaxHP           = _enemyData.maxHP;
        Speed           = _enemyData.speed;
        Damage          = _enemyData.damage;
        Gold            = _enemyData.gold;
        SpecialAbility  = _enemyData.specialAbility;
    }

    protected void SetupCurrentHp()
    {
        CurrentHp = MaxHP;
    }

    protected void ResetHpBar()
    {
        healthBar.Reset();
    }

    public void SetDefaultSpeed()
    {
        CurrentSpeed = Speed;
    }

    public abstract void Move();

    public abstract void SetMovingDirection();
}
