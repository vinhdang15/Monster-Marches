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
    public string   AnimationDamageType      { get; set; }
    public float    DealDamageDelay          { get; set; }
    public bool     isReachEnemyPos          = false;
    public List<IEffect> effects = new List<IEffect>();
    protected UnitBase                   targetEnemy;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;
    public event Action<BulletBase>      OnFinishBulletAnimation;
    protected IBulletAnimation bulletAnimation;
    protected Animator animator;
    
    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        
    }

    public void InitBullet(BulletData _bulletData, CSVEffectDataReader effectDataReader)
    {
        InitBulletData(_bulletData);
        InitBulletEffect(_bulletData, effectDataReader);
        InitBulletAnimation(_bulletData);
    }

    private void InitBulletData(BulletData _bulletData)
    {
        Speed                   = _bulletData.speed;
        Damage                  = _bulletData.damage;
        EffectType              = _bulletData.effectTyes;
        AnimationDamageType     = _bulletData.animationDamageType;
        DealDamageDelay         = _bulletData.dealDamageDelay;
    }

    private void InitBulletEffect(BulletData _bulletData, CSVEffectDataReader effectDataReader)
    {
        string[] effectTypes = _bulletData.effectTyes.Split(";");
        foreach(string effecType in effectTypes)
        {
            EffectData effectData = effectDataReader.effectDataList.GetEffectData(effecType);
            if(effectData == null)
            {
                // Debug.Log($"{this.name} have no effect");
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

    private void InitBulletAnimation(BulletData _bulletData)
    {
        animator = GetComponent<Animator>();
        string animationDamageType = _bulletData.animationDamageType;
        bulletAnimation = BulletAnimationFactory.CreateBulletAnimationHandler(animationDamageType,animator);
    }

    public void InitBulletTarget(UnitBase _enemy)
    {
        targetEnemy = _enemy;
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
                StartCoroutine(DealDamageToEnemy());
                yield break;
                
            }
            yield return null;
        }
    }

    protected void UpdateEnemyPos()
    {
        if(targetEnemy == null)
        {
            Debug.Log("enemy is null");
            return;
        }
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

    protected void InvokeOnFinishBulletAnimation()
    {
        OnFinishBulletAnimation?.Invoke(this);
    }

    protected IEnumerator DealDamageToEnemy()
    {
        // Start change animation state
        StartCoroutine(bulletAnimation.PlayDealDamageAnimation());
        Debug.Log(bulletAnimation.GetCurrentAnimationLength());
        yield return new WaitForSeconds(DealDamageDelay*bulletAnimation.GetCurrentAnimationLength());
        // Deal Damage
        if (targetEnemy.CurrentHp == 0) yield break;
        targetEnemy.TakeDamage(Damage);
        
        // Cause effect
        if(effects.Count > 0)
        {
            targetEnemy.ApplyEffect(effects);
        }
        
        // Wait until animation state finish
        yield return new WaitForSeconds(bulletAnimation.GetCurrentAnimationLength());  
        InvokeOnFinishBulletAnimation();
        yield break;
    }

    // Reset bullet state before return to pool
    public void ResetBullet()
    {
        isReachEnemyPos = false;
        targetEnemy = null;
    }
}

