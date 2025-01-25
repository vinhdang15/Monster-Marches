using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class UnitBase : MonoBehaviour
{
    private string      UnitType        { get; set; }
    public string       UnitName        { get; set; }      
    private int         MaxHP           { get; set; }
    public float        MoveSpeed       { get; set; }
    public float        AttackSpeed     { get; set; }
    protected int       Damage          { get; set; }
    public int          Gold            { get; set; }
    private string      SpecialAbility  { get; set; }
    public bool         isdead = false;
    public float        CurrentHp       { get; set; }
    public float        CurrentSpeed    { get; set; }
    protected Vector2   CurrentPos      { get; set; }
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
        //ResetTimeDelay();
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
    
    // public virtual void InitState()
    // {
    //     SetupCurrentHp();
    //     SetDefaultSpeed();
    // }

    private void SetDefaultSpeed()
    {
        CurrentSpeed = MoveSpeed;
    }

    public void ResetHp()
    {
        SetupCurrentHp();
        ResetHpBar();
    }

    private void SetupCurrentHp()
    {
        CurrentHp = MaxHP;
    }

    private void ResetHpBar()
    {
        healthBar.Reset();
    }

    public void ResetCurrentSpeed()
    {
        CurrentSpeed = MoveSpeed;
    }
    #endregion

    #region GET ANIMATION
    public void GetAnimation()
    {
        unitAnimation = GetComponent<UnitAnimation>();
        unitAnimation.GetAnimation();
    }
    #endregion

    #region UNIT ACTION STATE
    public virtual void TakeDamage(float damage)
    {
        if(isdead) return;
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        UpdateHealthBar();
    }

    public abstract void DealDamage();

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

    private void UpdateHealthBar()
    {
        healthBar.ResizeOnCurrentHp(MaxHP, CurrentHp);
    }

    protected void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    protected void ShowHealthBar()
    {
        healthBar.gameObject.SetActive(true);
    }
    #endregion

    public virtual void ResetUnit()
    {
        ResetHp();
        ShowHealthBar();
        SetDefaultSpeed();
        unitAnimation.ResetColor();
        underEffect.Clear();
    }
}
