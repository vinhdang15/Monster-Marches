using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelfHealSkill : SkillBase
{
    private float healCoolDown = 0f;

    public SelfHealSkill(string skillType, int skillValue, float skillOccursTime, float skillRange)
    {
        base.Init(skillType, skillValue, skillOccursTime, skillRange);
    }

    public override void Execute(UnitBase unit)
    {
        if(healCoolDown <= 0)
        {
            unit.Healing(skillValue);
            healCoolDown = skillOccursTime;
        }
        else
        {
            healCoolDown -= Time.deltaTime;
        }
    }

    public override bool CanApply(UnitBase unit)
    {
        bool soldierCanApply = unit is Soldier soldier 
                                && soldier.IsInGuardPos()
                                && soldier.CurrentHp < soldier.MaxHP;
        return soldierCanApply;
    }
}
