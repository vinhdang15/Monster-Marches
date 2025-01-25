using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public string   BulletType               { get; set; }
    public int      Damage                   { get; set; }
    public float    Speed                    { get; set; }
    public string   EffectType               { get; set; }
    public float    DealDamageDelay          { get; set; }
    public bool     hadAOEEffectType         = false;
    public bool     isReachEnemyPos          = false;
    public bool     isSetUpStartPos          = false;
    public Vector2 startPos;
    public List<IEffect> effects = new List<IEffect>();
    [SerializeField] protected UnitBase  targetEnemy;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;
    protected BulletAnimation            bulletAnimation;
    public event Action<BulletBase>      OnFinishBulletAnimation;

    [SerializeField] SoundEffectSO soundEffectSO;
    
    private void OnDisable()
    {
        OnFinishBulletAnimation = null;
    }

    #region INIT BULLET
    public void InitBullet(BulletData _bulletData)
    {
        InitBulletData(_bulletData);
        InitBulletEffect(_bulletData);
        InitBulletAnimation();
    }

    private void InitBulletData(BulletData _bulletData)
    {
        BulletType              = _bulletData.bulletType;
        Speed                   = _bulletData.speed;
        Damage                  = _bulletData.damage;
        EffectType              = _bulletData.effectTyes;
        DealDamageDelay         = _bulletData.dealDamageDelay;
    }

    private void InitBulletEffect(BulletData _bulletData)
    {
        string[] effectTypes = _bulletData.effectTyes.Split(";");
        foreach(string effecType in effectTypes)
        {
            EffectData effectData = CSVEffectDataReader.Instance.effectDataList.GetEffectData(effecType);
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
            
            if(effecType.Contains ("aoe"))
            {
                hadAOEEffectType = true;
            }
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
        if(targetEnemy.isdead)return;
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

    public virtual void SetBulletInitAngle(float spawnBulletDirection)
    {
        if(spawnBulletDirection == 0) return;
        transform.Rotate(new Vector3(0, 0, spawnBulletDirection));
    }

    protected virtual void SetBulletDirection()
    {
        if(!this.BulletType.Contains("bomb")) RotateInMovingDirection();
        else RotateInCircle();
    }

    private void RotateInCircle()
    {
        float RotateSpeed = 270f;
        transform.Rotate(Vector3.forward,RotateSpeed*Time.deltaTime, Space.Self);
    }

    protected virtual void RotateInMovingDirection()
    {
        Vector2 moveDir = bulletLastPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0, angle + 180), 1f);
    }

    protected virtual void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);
    }
    #endregion

    #region BuLLET REACHING ENEMY POS, DEAL DAMAGE AND ANOTATION
    // Animation event action
    public void DealDamageToEnemy()
    {
        BulletHittingSound();
        targetEnemy.TakeDamage(Damage);
    }

    protected void PlayAnimationWhenReachEnemyPos()
    {
        if(!targetEnemy.isdead || hadAOEEffectType)
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
        yield return null;
        yield return new WaitForSeconds(bulletAnimation.GetCurrentAnimationLength());
        InvokeOnFinishBulletAnimation();
    }

    protected void ApplyBulletEffect()
    {
        if(effects.Count == 0) return;
        if(targetEnemy.gameObject.activeSelf && targetEnemy.CurrentHp > 0)
        {
            targetEnemy.ApplyBulletEffect(effects);
        }
        else if(hadAOEEffectType)
        {
            ApplyBulletExplodeEffect();
        }
    }

    private void ApplyBulletExplodeEffect()
    {
        foreach(IEffect effect in effects)
        {
            EffectBase effectBase = effect as EffectBase;
            if(!effectBase.type.Contains("aoe")) return;
            effectBase.ApplyExplodeEffect(enemyPos);
        }
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

    #region BULLET SOUND
    public void BulletWhistleSound()
    {
        switch(BulletType)
        {
            case string t when t == BulletTypea.Arrow1.ToString().Trim().ToLower() ||
                                t == BulletTypea.Arrow2.ToString().Trim().ToLower():
                AudioManager.Instance.PlaySound(soundEffectSO.arrowSound);
                break;
            case string t when t == BulletTypea.MagicBall1.ToString().Trim().ToLower() ||
                                t == BulletTypea.MagicBall2.ToString().Trim().ToLower():
                AudioManager.Instance.PlaySound(soundEffectSO.MagicBallWhistleSound);
                break;
            case string t when t == BulletTypea.Bomb1.ToString().Trim().ToLower() ||
                                t == BulletTypea.Bomb2.ToString().Trim().ToLower():
                AudioManager.Instance.PlaySound(soundEffectSO.bomWhistleSound);
                break;
        }
    }

    public void BulletHittingSound()
    {
        switch(BulletType)
        {
            case string t when t == BulletTypea.MagicBall1.ToString().Trim().ToLower() ||
                                t == BulletTypea.MagicBall2.ToString().Trim().ToLower():
                AudioManager.Instance.PlaySound(soundEffectSO.MagicBallHitSound);
                break;
            case string t when t == BulletTypea.Bomb1.ToString().Trim().ToLower() ||
                                t == BulletTypea.Bomb2.ToString().Trim().ToLower():
                AudioManager.Instance.PlaySound(soundEffectSO.bomExplosionSound);
                break;
        }
    }
    #endregion
}

