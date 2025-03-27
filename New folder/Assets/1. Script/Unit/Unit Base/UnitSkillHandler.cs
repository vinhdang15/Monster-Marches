using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillHandler : MonoBehaviour
{
    private UnitBase unit;
     public void Init(UnitBase unit)
    {
        this.unit = unit;
    }

    public void ApplySkill()
    {
        foreach(ISkill skill in unit.skills)
        {
            if(skill is SkillBase skillBase && skillBase.CanApply(unit))
            {
                skillBase.Execute(unit);
            }
        }
    }
}
