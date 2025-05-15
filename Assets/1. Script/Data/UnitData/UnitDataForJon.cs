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
            unitType = "Soldier",
            id = "Swordsman_1",
            maxHP = 150,
            moveSpeed = 1f,
            attackSpeed = 1f,
            attackDamage = 30,
            goldReward = 0,
            skillType = "None",
        },

        new UnitData
        {
            unitType = "Soldier",
            id = "Swordsman_2",
            maxHP = 200,
            moveSpeed = 1.2f,
            attackSpeed = 1f,
            attackDamage = 45,
            goldReward = 0,
            skillType = "SelfHealing",
        },

        // Enemy
        new UnitData
        {
            unitType = "Enemy",
            id = "Enemy_A_1",
            maxHP = 350,
            moveSpeed = 0.8f,
            attackSpeed = 1.5f,
            attackDamage = 50,
            goldReward = 5,
            skillType = "None",
        },

        new UnitData
        {
            unitType = "Enemy",
            id = "Enemy_B_1",
            maxHP = 250,
            moveSpeed = 0.7f,
            attackSpeed = 1.5f,
            attackDamage = 40,
            goldReward = 20,
            skillType = "None",
        },

        new UnitData
        {
            unitType = "Enemy",
            id = "Enemy_C_1",
            maxHP = 200,
            moveSpeed = 0.7f,
            attackSpeed = 1.5f,
            attackDamage = 30,
            goldReward = 20,
            skillType = "None",
        },

        new UnitData
        {
            unitType = "Enemy",
            id = "Enemy_C_2",
            maxHP = 200,
            moveSpeed = 0.7f,
            attackSpeed = 1.5f,
            attackDamage = 30,
            goldReward = 20,
            skillType = "None",
        },
    };

    public List<UnitData> GetUnitDataForJson()
    {
        return unitDatas;
    }
}
