using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public string   BulletID               { get; set; }
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
    private bool isEnemyExitTowerView = false;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;
    protected BulletAnimation            bulletAnimation;
    public event Action<BulletBase>      OnFinishBulletAnimation;

    public TowerPresenter towerPresenter;

    [SerializeField] SoundEffectSO soundEffectSO;
    
    private void OnDisable()
    {
        OnFinishBulletAnimation = null;
    }

    #region INIT BULLET
    public void InitBullet(BulletData bulletData, BulletEffectDataReader bulletEffectDataReader)
    {
        InitBulletData(bulletData);
        InitBulletEffect(bulletData, bulletEffectDataReader);
        InitBulletAnimation();
    }

    private void InitBulletData(BulletData bulletData)
    {
        
        BulletID                = bulletData.bulletID;
        Speed                   = bulletData.speed;
        Damage                  = bulletData.damage;
        EffectType              = bulletData.effectTyes;
        DealDamageDelay         = bulletData.dealDamageDelay;
    }

    private void InitBulletEffect(BulletData bulletData, BulletEffectDataReader bulletEffectDataReader)
    {
        string[] effectTypes = bulletData.effectTyes.Split(";");
        foreach(string effecType in effectTypes)
        {
            BulletEffectData effectData = bulletEffectDataReader.bulletEffectDataSO.GetBulletEffectData(effecType);
            if(effectData == null) continue;
            IEffect effect = EffectFactory.CreateEffect(effectData.effectType, effectData.effectValue, effectData.effectDuration, 
                                                        effectData.effectOccursTime, effectData.effectRange);
            if(effect == null)
            {
                Debug.Log($"{this} bullet not have {effecType}");
                continue;
            }
            effects.Add(effect);
            
            if(effecType.Contains ("AoE"))
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

     public void InitBulletParent(TowerPresenter _towerPresenter)
    {
        towerPresenter = _towerPresenter;
        towerPresenter.towerViewBase.OnEnemyExit += HandleEnemyExit;
    }

    private void HandleEnemyExit(Enemy enemy, TowerViewBase towerViewBase)
    {
        if(targetEnemy == enemy)
        {
            isEnemyExitTowerView = true;
        }
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
        }
    }

    protected void UpdateEnemyPos()
    {
        if(targetEnemy.CurrentHp == 0 || isEnemyExitTowerView == true) return;
        enemyPos = targetEnemy.transform.position;
    }

    protected virtual void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);
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

    protected virtual void SetBulletDirection()
    {
        if(!this.BulletID.Contains("bomb")) RotateInMovingDirection();
        else RotateInCircle();
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
        bool canDealDamage = hadAOEEffectType || (!isEnemyExitTowerView && targetEnemy != null && !targetEnemy.isDead);
        if (canDealDamage)
        {
            bulletAnimation.PlayDealDamageAnimation();
            ApplyBulletEffect();
        }
        else
        {
            bulletAnimation.PlayHitNullAnimation();
        }
        StartCoroutine(FinishAnimationCoroutine());
    }

    // Need to using yield return null brefor GetCurrentAnimationLength
    // cause animator still need one last frame to make stransition
    protected IEnumerator FinishAnimationCoroutine()
    {
        yield return null;
        yield return new WaitForSeconds(bulletAnimation.GetCurrentAnimationLength());
        FinishBulletAnimationHandler();
    }

    protected void ApplyBulletEffect()
    {
        if (effects.Count == 0) return;
        if (targetEnemy.gameObject.activeSelf && targetEnemy.CurrentHp > 0)
        {
            targetEnemy.ApplyBulletEffect(effects);
        }
    }

    private void FinishBulletAnimationHandler()
    {
        OnFinishBulletAnimation?.Invoke(this);
    }

    // Reset bullet state before return to pool
    public virtual void ResetBullet()
    {
        isReachEnemyPos = false;
        isSetUpStartPos = false;
        isEnemyExitTowerView = false;
        targetEnemy = null;
    }
    #endregion

    #region BULLET SOUND
    public void BulletWhistleSound()
    {
        switch(BulletID)
        {
            case string t when t == global::BulletID.Arrow_1.ToString() ||
                                t == global::BulletID.Arrow_2.ToString():
                AudioManager.Instance.PlaySound(soundEffectSO.arrowSound);
                break;
            case string t when t == global::BulletID.MagicBall_1.ToString() ||
                                t == global::BulletID.MagicBall_2.ToString():
                AudioManager.Instance.PlaySound(soundEffectSO.MagicBallWhistleSound);
                break;
            case string t when t == global::BulletID.Bomb_1.ToString() ||
                                t == global::BulletID.Bomb_2.ToString():
                AudioManager.Instance.PlaySound(soundEffectSO.bomWhistleSound);
                break;
        }
    }

    public void BulletHittingSound()
    {
        switch(BulletID)
        {
            case string t when t == global::BulletID.MagicBall_1.ToString() ||
                                t == global::BulletID.MagicBall_2.ToString():
                AudioManager.Instance.PlaySound(soundEffectSO.MagicBallHitSound);
                break;
            case string t when t == global::BulletID.Bomb_1.ToString() ||
                                t == global::BulletID.Bomb_2.ToString():
                AudioManager.Instance.PlaySound(soundEffectSO.bomExplosionSound);
                break;
        }
    }
    #endregion
}

