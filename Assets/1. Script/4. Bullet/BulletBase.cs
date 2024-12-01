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

    #region INIT BULLET
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
    #endregion

    #region BULLET MOVING AND ROTATION
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
                AdjustBulletSpeed();
                CalBulletRotation(); 
            } 
        }
        bulletLastPos = transform.position;
    }

    protected virtual void AdjustBulletSpeed()
    {
        return;
    }

    protected virtual void CalBulletRotation()
    {
        if(!this.type.Contains("Bomb")) RotateInMovingDirection();
        else RotateInCircle();
    }

    private void RotateInCircle()
    {
        float RotateSpeed = 180f;
        transform.Rotate(Vector3.forward,RotateSpeed*Time.deltaTime, Space.Self);
    }

    protected virtual void RotateInMovingDirection()
    {
        Vector2 moveDir = bulletLastPos - (Vector2)transform.position;
        float tangentAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0, tangentAngle + 180), 1f);
    }

    protected virtual void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);
    }
    #endregion

    #region BuLLET REACHING ENEMY POS, DEAL DAMAGE AND ANOTATION 
    protected IEnumerator DealDamageToEnemy()
    {
        if (targetEnemy.CurrentHp != 0)
        {
            // Start PlayDealDamageAnimation state
            bulletAnimation.PlayDealDamageAnimation();
            yield return new WaitForSeconds(DealDamageDelay*bulletAnimation.GetCurrentAnimationLength());
            // Deal Damage
            targetEnemy.TakeDamage(Damage);
        
            // Cause effect
            if(effects.Count > 0)
            {
                targetEnemy.ApplyBulletEffect(effects);
            }
            
            // Wait until animation state finish
            yield return new WaitForSeconds(bulletAnimation.GetCurrentAnimationLength());  
            InvokeOnFinishBulletAnimation();
            yield break;
        } 
        else
        {
            
            // Wait until animation state finish
            yield return StartCoroutine(bulletAnimation.PlayHitNullAnimation());

            InvokeOnFinishBulletAnimation();
            Debug.Log( this.name + ": enemy already die, bullet return to pool");
            yield break;
        }
    }

    protected void InvokeOnFinishBulletAnimation()
    {
        OnFinishBulletAnimation?.Invoke(this);
    }

    // Reset bullet state before return to pool
    public void ResetBullet()
    {
        isReachEnemyPos = false;
        targetEnemy = null;
    }
    #endregion
}

