using System;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public string   type;//                  { get; set; }
    public int      Damage                   { get; set; }
    public float    Speed                    { get; set; }
    public string   EffectType               { get; set; }
    public bool     isReachEnemyPos          = false;
    public IEffect effect;
    [SerializeField] UnitBase            targetEnemy;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;
    public event Action<BulletBase, UnitBase> OnReachEnemyPos;
    
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
        EffectData effectData = effectDataReader.effectDataList.GetEffectData(_bulletData.effectTyes);
        if(effectData != null)
        {
            effect = EffectFactory.CreateEffect(effectData.effectType, effectData.effectValue, effectData.effectDuration, effectData.effectOccursTime, effectData.effectRange);
        }
        else
        {
            Debug.Log($"{type} bullet have no effect");
        }
        
    }

    public void UpdateBulletDirection()
    {
        if(bulletLastPos != null)
        {
            if(bulletLastPos != (Vector2)transform.position)
            {
                CalBulletRotation();
            } 
        }
        bulletLastPos = transform.position;
    }

    protected virtual void CalBulletRotation()
    {
        Vector2 moveDir = bulletLastPos - (Vector2)transform.position;
        float tangentAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0, tangentAngle + 180), 1f);
    }

    public void MoveToTarget()
    {
        if(targetEnemy != null)
        {
            enemyPos = targetEnemy.transform.position;
        }
        if(!isReachEnemyPos) transform.position = Vector2.MoveTowards(transform.position, enemyPos, Speed*Time.deltaTime);

        if((Vector2)transform.position == enemyPos)
        {
            isReachEnemyPos = true;
            OnReachEnemyPos?.Invoke(this, targetEnemy);
        }
    }   
}

