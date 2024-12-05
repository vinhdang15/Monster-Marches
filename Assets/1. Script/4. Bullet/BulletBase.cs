using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public string   Type => gameObject.name.Trim().ToLower();
    public int      Damage                   { get; set; }
    public float    Speed                    { get; set; }
    public string   EffectType               { get; set; }
    public float    DealDamageDelay          { get; set; }
    public bool     isReachEnemyPos          = false;
    public bool     isSetUpStartPos          = false;
    public Vector2 startPos;
    public List<IEffect> effects = new List<IEffect>();
    protected UnitBase                   targetEnemy;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;
    protected BulletAnimation            bulletAnimation;
    public event Action<BulletBase>      OnFinishBulletAnimation;
    
    private void OnDisable()
    {
        OnFinishBulletAnimation = null;
    }

    #region INIT BULLET
    public void InitBullet(BulletData _bulletData, CSVEffectDataReader effectDataReader)
    {
        InitBulletData(_bulletData);
        InitBulletEffect(_bulletData, effectDataReader);
        InitBulletAnimation();
    }

    private void InitBulletData(BulletData _bulletData)
    {
        Speed                   = _bulletData.speed;
        Damage                  = _bulletData.damage;
        EffectType              = _bulletData.effectTyes;
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

    private void InitBulletAnimation()
    {
        bulletAnimation = GetComponent<BulletAnimation>();
    }

    public void InitBulletTarget(UnitBase _enemy)
    {
        targetEnemy = _enemy;
    }
    #endregion

    #region BULLET MOVING AND ROTATION
    public virtual void MoveToTarget()
    {
        if(isReachEnemyPos == true) return;
        UpdateEnemyPos();
        MoveTowards();
        UpdateBulletDirection();

        if((Vector2)transform.position == enemyPos)
        {
            isReachEnemyPos = true;
            PlayAnimationWhenReachEnemyPos();
            ApplyBulletEffect();         
        }
    }

    protected void UpdateEnemyPos()
    {
        if(!targetEnemy.gameObject.activeSelf)return;
        enemyPos = targetEnemy.transform.position;
    }

    protected void UpdateBulletDirection()
    {
        if(bulletLastPos != null)
        {
            if(bulletLastPos != (Vector2)transform.position)
            {
                SetBulletDirection(); 
            } 
        }
        bulletLastPos = transform.position;
    }

    protected virtual void AdjustBulletSpeed()
    {
        return;
    }

    protected virtual void SetBulletDirection()
    {
        if(!this.Type.Contains("Bomb")) RotateInMovingDirection();
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
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0, angle + 180), 1f);
        // transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    protected virtual void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);
    }
    #endregion

    #region BuLLET REACHING ENEMY POS, DEAL DAMAGE AND ANOTATION 
    public void DealDamageToEnemy()
    {
        targetEnemy.TakeDamage(Damage);
    }

    protected void PlayAnimationWhenReachEnemyPos()
    {
        if(targetEnemy != null && targetEnemy.CurrentHp > 0)
        {
            bulletAnimation.PlayDealDamageAnimation();
        }
        else
        {
            bulletAnimation.PlayHitNullAnimation();
        }
        StartCoroutine(FinishAnotationCoroutine());

    }

    protected IEnumerator FinishAnotationCoroutine()
    {
        yield return new WaitForSeconds(bulletAnimation.GetCurrentAnimationLength());
        InvokeOnFinishBulletAnimation();
    }

    protected void ApplyBulletEffect()
    {
        if(effects.Count == 0) return;
        if(!targetEnemy.gameObject.activeSelf || targetEnemy.CurrentHp == 0) return;
        targetEnemy.ApplyBulletEffect(effects);
    }
    
    protected void InvokeOnFinishBulletAnimation()
    {
        OnFinishBulletAnimation?.Invoke(this);
    }

    // Reset bullet state before return to pool
    public virtual void ResetBullet()
    {
        isReachEnemyPos = false;
        isSetUpStartPos = false;
        targetEnemy = null;
    }
    #endregion
}

