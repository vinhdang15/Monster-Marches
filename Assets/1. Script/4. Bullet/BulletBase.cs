using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public string      type;//                        { get; set; }
    protected float    Speed                    { get; set; }
    protected int      Damage                   { get; set; }
    protected int      DamageOverTimeCount      { get; set; }
    protected string   DamageTarget             { get; set; }
    protected float    DamageRange              { get; set; }
    protected string   DamageEffect             { get; set; }
    public bool        isReachEnemyPos          = false;
    //[SerializeField] Animator animator;
    [HideInInspector] public UnitBase       targetEnemy;
    protected Vector2                    enemyPos;
    [HideInInspector] public Vector2     bulletLastPos;

    public delegate void BulletReachEnemyPosHandler(BulletBase bulletBase);
    public BulletReachEnemyPosHandler OnReachEnemyPos;
    
    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        
    }

    public void InitBullet(BulletData _bulletData, UnitBase _enemy)
    {
        Speed                   = _bulletData.speed;
        Damage                  = _bulletData.damage;
        DamageOverTimeCount     = _bulletData.damageOverTimeCount;
        DamageTarget            = _bulletData.damageTarget;
        DamageRange             = _bulletData.damageRange;
        DamageEffect            = _bulletData.damageEffect;
        targetEnemy             = _enemy;
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
            OnReachEnemyPos?.Invoke(this);
            isReachEnemyPos = true;
        }
    }

    public void DamageCount(int damageOverTimeCount)
    {
        
    }
}

