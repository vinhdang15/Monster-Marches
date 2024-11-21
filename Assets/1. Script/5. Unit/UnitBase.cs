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
    Dictionary<string, IEffect> activeEffect = new Dictionary<string, IEffect>();

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

    #region APPLY EFFECT
    // slow
    public void ApplySlow(IEffect effect, string type, float value, float duration, int OccursTime, float range)
    {
        if(activeEffect.ContainsKey(type)) return;
        activeEffect.Add(type, effect);
        StartCoroutine(SlowCoroutine(type, value, duration));
    }

    private IEnumerator SlowCoroutine(string type, float value, float duration)
    {
        CurrentSpeed = Speed * (1 - value/100);
        yield return new WaitForSeconds(duration);
        activeEffect.Remove(type);
        ResetCurrentSpeed();
    }

    // DoT damage
    public void ApplyDamageOverTimeCoroutine(IEffect effect, string type, float value, float duration, int OccursTime, float range)
    {
        if(activeEffect.ContainsKey(type)) return;
        activeEffect.Add(type, effect);
        StartCoroutine(DamageOverTimeCoroutine(type, value,duration,OccursTime));
    }

    private IEnumerator DamageOverTimeCoroutine(string type, float value, float duration, int OccursTime)
    {
        float timeAmong = duration / OccursTime;
        while(OccursTime - 1 > 0)
        {   
            yield return new WaitForSeconds(timeAmong);
            if(CurrentHp > 0)
            {
                TakeDamage(value);
                OccursTime --;
            }
            else
            {
                OccursTime = 0;
            }
        }
        activeEffect.Remove(type);
    }

    // AoE damage
    public void ApplyAreaOfEffect(string type, float value, float duration, int OccursTime, float range)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));
        foreach(var enemyColl in hitColliders)
        {
            Enemy enemy = enemyColl.GetComponent<Enemy>();
            if(enemy != this) enemy.TakeDamage(value);
        }
    }
    #endregion
}
