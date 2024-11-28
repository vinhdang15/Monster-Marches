using System;
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
    public Dictionary<string, IEffect> activeEffect = new Dictionary<string, IEffect>();

    public virtual void InitUnit(UnitData _enemyData)
    {
        Type            = _enemyData.type;
        MaxHP           = _enemyData.maxHP;
        Speed           = _enemyData.speed;
        Damage          = _enemyData.damage;
        Gold            = _enemyData.gold;
        SpecialAbility  = _enemyData.specialAbility;
    }

    public virtual void InitState()
    {
        SetupCurrentHp();
        SetDefaultSpeed();
    }

    public void SetupCurrentHp()
    {
        CurrentHp = MaxHP;
    }

    public void SetDefaultSpeed()
    {
        CurrentSpeed = Speed;
    }

    public void UpdateHealthBar()
    {
        healthBar.ResizeOnCurrentHp(MaxHP, CurrentHp);
    }

    public void ResetHpBar()
    {
        healthBar.Reset();
    }

    public void ResetCurrentSpeed()
    {
        CurrentSpeed = Speed;
    }

    public abstract void Move();

    public abstract void SetMovingDirection();

    public virtual void TakeDamage(float damage)
    {
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        UpdateHealthBar();
    }

    public abstract void Die();

    public void ApplyEffect(List<IEffect> effects)
    {
        foreach(IEffect effect in effects)
        {
            EffectBase effectBase = effect as EffectBase;
            if(activeEffect.ContainsKey(effectBase.type))
            {
                Debug.Log(activeEffect.ContainsKey(effectBase.type).ToString());
                continue;
            }
            activeEffect.Add(effectBase.type, effect);
            StartCoroutine(effect.ApplyEffect(this));
        }
    }
}
