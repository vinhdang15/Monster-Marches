using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataForJon : MonoBehaviour
{
    private List<UnitData> unitDatas = new()
    {
        // Soldier
        new UnitData
        {
            unitType        = Unitype.Soldier.ToString(),
            id              = UnitID.Soldier_1.ToString(),
            maxHP           = 150,
            moveSpeed       = 1.2f,
            attackSpeed     = 1f,
            attackDamage    = 30,
            goldReward      = 0,
            skillType       = UnitSkill.none.ToString(),
        },

        new UnitData
        {
            unitType        = Unitype.Soldier.ToString(),
            id              = UnitID.Soldier_2.ToString(),
            maxHP           = 200,
            moveSpeed       = 1.2f,
            attackSpeed     = 1f,
            attackDamage    = 45,
            goldReward      = 0,
            skillType       = UnitSkill.SelfHealing.ToString(),
        },

        // Enemy
        new UnitData
        {
            unitType        = Unitype.Enemy.ToString(),
            id              = UnitID.Enemy_A_1.ToString(),
            maxHP           = 350,
            moveSpeed       = 0.5f,
            attackSpeed     = 1.5f,
            attackDamage    = 50,
            goldReward      = 5,
            skillType       = UnitSkill.none.ToString(),
        },

        new UnitData
        {
            unitType        = Unitype.Enemy.ToString(),
            id              = UnitID.Enemy_B_1.ToString(),
            maxHP           = 250,
            moveSpeed       = 0.6f,
            attackSpeed     = 1.5f,
            attackDamage    = 40,
            goldReward      = 20,
            skillType       = UnitSkill.none.ToString(),
        },

        new UnitData
        {
            unitType        = Unitype.Enemy.ToString(),
            id              = UnitID.Enemy_C_1.ToString(),
            maxHP           = 200,
            moveSpeed       = 0.7f,
            attackSpeed     = 1.5f,
            attackDamage    = 30,
            goldReward      = 20,
            skillType       = UnitSkill.none.ToString(),
        },

        new UnitData
        {
            unitType        = Unitype.Enemy.ToString(),
            id              = UnitID.Enemy_C_2.ToString(),
            maxHP           = 200,
            moveSpeed       = 0.7f,
            attackSpeed     = 1.5f,
            attackDamage    = 30,
            goldReward      = 20,
            skillType       = UnitSkill.none.ToString(),
        },
    };

    public List<UnitData> GetUnitDataForJson()
    {
        return unitDatas;
    }
}
