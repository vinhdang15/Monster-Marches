using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class UnitBase : MonoBehaviour
{
    public string   UnitType        { get; set; }
    public string   UnitName        { get; set; }      
    public int      MaxHP           { get; set; }
    public float    MoveSpeed       { get; set; }
    public float    AttackSpeed     { get; set; }
    public int      Damage          { get; set; }
    public int      Gold            { get; set; }
    public string   SpecialAbility  { get; set; }
    public float    CurrentHp       { get; set; }
    public float    CurrentSpeed    { get; set; }
    public bool     IsUnderEffect   { get; set; }
    protected Vector2 CurrentPos    { get; set; }
    [SerializeField] HealthBar healthBar;
    protected float randomDelay;
    protected float timeDelay;
    protected AudioSource audioSource;
    [SerializeField] protected SoundEffectSO soundEffectSO;
    public Dictionary<string, IEffect> underEffect = new Dictionary<string, IEffect>();

    // Animation
    public UnitAnimation unitAnimation;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ResetTimeDelay();
    }

    protected void ResetTimeDelay(float timeWait = 0)
    {
        randomDelay = Random.Range(0.0f, 0.3f);
        timeDelay = randomDelay + timeWait;
    }

    #region INIT UNIT
    public virtual void InItUnit(UnitData _unitData)
    {
        InitUnitData(_unitData);
        SetupCurrentHp();
        SetDefaultSpeed();
    }
    public virtual void InitUnitData(UnitData _unitData)
    {
        UnitType            = _unitData.unitType;
        UnitName            = _unitData.unitName;
        MaxHP               = _unitData.maxHP;
        MoveSpeed           = _unitData.moveSpeed;
        AttackSpeed         = _unitData.attackSpeed;
        Damage              = _unitData.damage;
        Gold                = _unitData.gold;
        SpecialAbility      = _unitData.specialAbility;
    }
    
    public virtual void InitState()
    {
        SetupCurrentHp();
        SetDefaultSpeed();
    }

    private void SetupCurrentHp()
    {
        CurrentHp = MaxHP;
    }

    private void SetDefaultSpeed()
    {
        CurrentSpeed = MoveSpeed;
    }

    private void ResetHpBar()
    {
        healthBar.Reset();
    }

    public void ResetHp()
    {
        SetupCurrentHp();
        ResetHpBar();
    }

    public void ResetCurrentSpeed()
    {
        CurrentSpeed = MoveSpeed;
    }
    #endregion

    #region UNIT ACTION STATE
    public virtual void TakeDamage(float damage)
    {
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        UpdateHealthBar();
    }

    public abstract void DealDamage();

    private void UpdateHealthBar()
    {
        healthBar.ResizeOnCurrentHp(MaxHP, CurrentHp);
    }

    public void ApplyBulletEffect(List<IEffect> effects)
    {
        foreach(IEffect effect in effects)
        {
            EffectBase effectBase = effect as EffectBase;
            if(underEffect.ContainsKey(effectBase.type))
            {
                continue;
            }
            underEffect.Add(effectBase.type, effect);
            StartCoroutine(effect.ApplyEffect(this));
        }
    }
    #endregion

    #region GET ANIMATION
    public void GetAnimation()
    {
        unitAnimation = GetComponent<UnitAnimation>();
        unitAnimation.GetAnimation();
    }
    #endregion

    public virtual void ResetUnit()
    {
        ResetHp();
        SetDefaultSpeed();
        unitAnimation.ResetColor();
        underEffect.Clear();
    }

    protected IEnumerator ExecuteWithDelay(Action action)
    {
        float randomDelay = Random.Range(0f, 0.5f);
        yield return new WaitForSeconds(randomDelay);
        action?.Invoke();
    }
}
