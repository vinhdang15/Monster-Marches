using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataForJson : MonoBehaviour
{
    private List<SkillData> skillDatas = new()
    {
        new SkillData
        {
            skillType = UnitSkill.SelfHealing.ToString(),
            skillValue = 10,
            skillOccursTime = 1f,
            skillRange = 0f
        },

        new SkillData
        {
            skillType = UnitSkill.Archery.ToString(),
            skillValue = 15,
            skillOccursTime = 2f,
            skillRange = 3f
        }
    };

    public List<SkillData> GetSkillDataForJson()
    {
        return skillDatas;
    }
}
