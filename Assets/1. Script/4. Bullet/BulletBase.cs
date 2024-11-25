using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public string   type;//                  { get; set; }
    public int      Damage                   { get; set; }
    public float    Speed                    { get; set; }
    public string   EffectType               { get; set; }
    public bool     isReachEnemyPos          = false;
    public List<IEffect> effects = new List<IEffect>();
    protected UnitBase            targetEnemy;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;
    public event Action<BulletBase> OnReachEnemyPos;
    
    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        
    }

    public void InitBullet(BulletData _bulletData, CSVEffectDataReader effectDataReader, UnitBase _enemy)
    {
        InitBulletData(_bulletData, _enemy);
        InitBulletEffect(_bulletData, effectDataReader);
    }

    private void InitBulletData(BulletData _bulletData, UnitBase _enemy)
    {
        Speed                   = _bulletData.speed;
        Damage                  = _bulletData.damage;
        EffectType              = _bulletData.effectTyes;
        targetEnemy             = _enemy;
    }

    private void InitBulletEffect(BulletData _bulletData, CSVEffectDataReader effectDataReader)
    {
        string[] effectTypes = _bulletData.effectTyes.Split(";");
        foreach(string effecType in effectTypes)
        {
            EffectData effectData = effectDataReader.effectDataList.GetEffectData(effecType);
            if(effectData == null)
            {
                Debug.Log($"{this.name} have no effect");
                continue;
            }
            IEffect effect = EffectFactory.CreateEffect(effectData.effectType, effectData.effectValue, effectData.effectDuration, 
                                                        effectData.effectOccursTime, effectData.effectRange);
            if(effect == null)
            {
                Debug.Log($"{this} bullet not have {effect}");
                continue;
            }
            effects.Add(effect);
        }
    }

    protected virtual void CalBulletRotation()
    {
        Vector2 moveDir = bulletLastPos - (Vector2)transform.position;
        float tangentAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0, tangentAngle + 180), 1f);
    }

    protected virtual void AdjustBulletSpeed()
    {
        return;
    }

    // public virtual void MoveToTarget()
    // {
    //     if(targetEnemy != null)
    //     {
    //         enemyPos = targetEnemy.transform.position;
    //     }
    //     if(!isReachEnemyPos) transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);

    //     if((Vector2)transform.position == enemyPos)
    //     {
    //         isReachEnemyPos = true;
    //         InvokeOnReachEnemyPos();
    //     }
    // }

    public virtual IEnumerator MoveToTargetCoroutine()
    {
        while(!isReachEnemyPos)
        {
            UpdateEnemyPos();
            MoveTowards();
            UpdateBulletSpeedAndDirection();

            if((Vector2)transform.position == enemyPos)
            {
                isReachEnemyPos = true;
                InvokeOnReachEnemyPos();
                yield break;
            }
            yield return null;
        }
    }

    protected void UpdateEnemyPos()
    {
        if(targetEnemy == null) return;
        enemyPos = targetEnemy.transform.position;
    }
    protected void UpdateBulletSpeedAndDirection()
    {
        if(bulletLastPos != null)
        {
            if(bulletLastPos != (Vector2)transform.position)
            {
                CalBulletRotation();
                AdjustBulletSpeed();
            } 
        }
        bulletLastPos = transform.position;
    }

    protected virtual void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);
    }

    protected void InvokeOnReachEnemyPos()
    {
        OnReachEnemyPos?.Invoke(this);
    }

    public void ReachingEnemyPos()
    {
        // deal Damage
        if (targetEnemy.CurrentHp == 0) return;
        targetEnemy.TakeDamage(Damage);
        
        // Cause effect
        if(effects.Count != 0)
        {
            foreach(var effect in effects)
            {
                StartCoroutine(effect.ApplyEffect(targetEnemy));
            }
        }
    }
}

