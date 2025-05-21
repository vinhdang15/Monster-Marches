using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class UnitBase : MonoBehaviour
{
    private string      UnitType        { get; set; }
    public string       UnitID          { get; set; }      
    public int          MaxHP           { get; set; }
    public float        MoveSpeed       { get; set; }
    public float        AttackSpeed     { get; set; }
    protected int       Damage          { get; set; }
    public int          Gold            { get; set; }
    private string      SkillType       { get; set; }
    public bool         isDead = false;
    public float        CurrentHp       { get; set; }
    public float        CurrentSpeed    { get; set; }
    protected Vector2   CurrentPos      { get; set; }
    public Dictionary<string, IEffect> underEffect = new Dictionary<string, IEffect>();
    public List<ISkill> skills = new List<ISkill>();
    protected UnitSkillHandler unitSkillHandler;
    [SerializeField] HealthBar healthBar;
    protected float randomDelay;
    protected float attackCooldown;
    protected AudioSource audioSource;
    [SerializeField] protected SoundEffectSO soundEffectSO;
    

    // Animation
    public UnitAnimation unitAnimation;

    protected virtual void Awake()
    {
        InitComponents();
        audioSource = GetComponent<AudioSource>();
    }

    private void InitComponents()
    {
        unitSkillHandler = gameObject.AddComponent<UnitSkillHandler>();
        unitSkillHandler.Init(this);
    }

    protected float ResetTimeDelay(float timeWait = 0)
    {
        randomDelay = Random.Range(0.0f, 0.3f);
        return randomDelay + timeWait;
    }

    #region INIT UNIT
    public virtual void InitUnit(UnitData unitData, SkillDataReader skillDataReader)
    {
        InitUnitData(unitData);
        SetupCurrentHp();
        SetDefaultSpeed();
        InitUnitSkill(unitData, skillDataReader);
    }
    private void InitUnitData(UnitData unitData)
    {
        UnitType            = unitData.unitType;
        UnitID              = unitData.id;
        MaxHP               = unitData.maxHP;
        MoveSpeed           = unitData.moveSpeed;
        AttackSpeed         = unitData.attackSpeed;
        Damage              = unitData.attackDamage;
        Gold                = unitData.goldReward;
        SkillType           = unitData.skillType;
    }
    
    private void InitUnitSkill(UnitData unitData, SkillDataReader skillDataReader)
    {
        string[] skillTypes = unitData.skillType.Split(";");
        // Debug.Log($"{gameObject.name} number of {skillTypes.Length}");
        foreach(string skillType in skillTypes)
        {
            SkillData skillData = skillDataReader.skillDataListSO.GetSkillData(skillType);
            if(skillData == null) continue;
            ISkill skill = SkillFactory.CreateSkill(skillData.skillType, skillData.skillValue, 
                                                        skillData.skillOccursTime, skillData.skillRange);
            if(skill == null) continue;
            skills.Add(skill);
        }
    }

    protected void ApplySkill()
    {
        unitSkillHandler.ApplySkill();
    }

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
        if(isDead) return;
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
        UpdateHealthBar();
    }

    public virtual void ApplyEffectFlashColor(Color effectColor)
    {
        unitAnimation.ApplyEffectFlashColor(effectColor);
    }

    public virtual void ApplyEffectColor(Color effectColor)
    {
        unitAnimation.ApplyEffectColor(effectColor);
    }

    public virtual void ResetColor()
    {
        unitAnimation.ResetColor();
    }

    public virtual void Healing(float healingValue)
    {
        if(isDead) return;
        CurrentHp = Mathf.Min(MaxHP, CurrentHp + healingValue);
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
        ResetColor();
        underEffect.Clear();
    }
}
