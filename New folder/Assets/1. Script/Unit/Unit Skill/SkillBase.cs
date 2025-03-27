using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SkillBase : ISkill
{
    public string   skillType;
    public int      skillValue;
    public float    skillOccursTime;
    public float    skillRange;

    public void Init(string skillType, int skillValue, float skillOccursTime, float skillRange)
    {
        this.skillType           = skillType;
        this.skillValue          = skillValue;
        this.skillOccursTime     = skillOccursTime;
        this.skillRange          = skillRange;
    }

    public abstract void Execute(UnitBase unit);

    public abstract bool CanApply(UnitBase unit);
}
