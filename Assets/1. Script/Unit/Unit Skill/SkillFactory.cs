using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    public static ISkill CreateSkill(string skillType, int skillValue, float skillOccursTime, float skillRange)
    {
        switch (skillType)
        {
            case string t when t.Contains(UnitSkill.SelfHealing.ToString()):
                return new SelfHealSkill(skillType, skillValue, skillOccursTime, skillRange);
            default :
                return null;
        }
    }
}
