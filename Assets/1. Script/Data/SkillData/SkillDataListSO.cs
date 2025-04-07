using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataList", menuName = "Data Config/SkillDataList", order = 1)]
public class SkillDataListSO : ScriptableObject
{
    public List<SkillData> skillDataList = new List<SkillData>();

    public SkillData GetSkillData(string type)
    {
        // type = type.Trim().ToLower();
        return skillDataList.Find(data => data.skillType == type);
    }

    public int GetSkillDamage(string type)
    {
        SkillData skillType = skillDataList.Find(data => data.skillType == type);
        return skillType.skillValue;
    }
}