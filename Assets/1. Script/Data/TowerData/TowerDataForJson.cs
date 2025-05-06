using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDataForJson : MonoBehaviour
{
    private List<TowerData> towerDataList = new()
    {
        // ArcherTower
        new TowerData
        {
            towerType = "ArcherTower",
            level           = 1,
            SpawnObject     = "Arrow_1",
            spawnRate       = 1f,
            fireAnimDelay   = 0.25f,
            rangeDetect     = 2.5f,
            rangeRaycast    = 0.7f,
            goldRequired    = 50,
            descriptions    = "<color=#FFD900><size=130%>Archer Tower:</size></color>\n  Shoots at a high speed. Allowing for rapid attacks",
        },

        new TowerData
        {
            towerType = "ArcherTower",
            level           = 2,
            SpawnObject     = "Arrow_2",
            spawnRate       = 1f,
            fireAnimDelay   = 0.25f,
            rangeDetect     = 3f,
            rangeRaycast    = 0.7f,
            goldRequired    = 70,
            descriptions    = "<color=#FFD900><size=130%>Archer Tower:</size></color>\n  The arrow applies a damage-over-time effect. Slowly reducing the enemy's health"
        },

        // MageTower
        new TowerData
        {
            towerType = "MageTower",
            level           = 1,
            SpawnObject     = "MagicBall_1",
            spawnRate       = 1f,
            fireAnimDelay   = 0.4f,
            rangeDetect     = 2.3f,
            rangeRaycast    = 0.7f,
            goldRequired    = 70,
            descriptions    = "<color=#FFD900><size=130%>Mage tower:</size></color>\n  The Magic Ball hits the target and continues to deal damage over time.",
        },

        new TowerData
        {
            towerType = "MageTower",
            level           = 2,
            SpawnObject     = "MagicBall_2",
            spawnRate       = 1f,
            fireAnimDelay   = 0.4f,
            rangeDetect     = 2.8f,
            rangeRaycast    = 0.7f,
            goldRequired    = 100,
            descriptions    = "<color=#FFD900><size=130%>Mage tower:</size></color>\n  The Magic Ball deal damages to enemy over time and also slows their movement.",
        },

        // CannonTower
        new TowerData
        {
            towerType       = "CannonTower",
            level           = 1,
            SpawnObject     = "Bomb_1",
            spawnRate       = 1.5f,
            fireAnimDelay   = 0.5f,
            rangeDetect     = 2f,
            rangeRaycast    = 0.7f,
            goldRequired    = 80,
            descriptions    = "<color=#FFD900><size=130%>Cannon Tower:</size></color>\n  Bombs explode on impact. Dealing damage to all enemies in the area.",
        },

        new TowerData
        {
            towerType       = "CannonTower",
            level           = 2,
            SpawnObject     = "Bomb_2",
            spawnRate       = 1.3f,
            fireAnimDelay   = 0.5f,
            rangeDetect     = 2.5f,
            rangeRaycast    = 0.7f,
            goldRequired    = 120,
            descriptions    = "<color=#FFD900><size=130%>Cannon Tower:</size></color>\n  Increases the damage attack. Making it more effective against enemies.",
        },

        // Barrack
        new TowerData
        {
            towerType       = "Barrack",
            level           = 1,
            SpawnObject     = "Swordsman_1",
            spawnRate       = 5f,
            fireAnimDelay   = 0f,
            rangeDetect     = 2f,
            rangeRaycast    = 0.7f,
            goldRequired    = 70,
            descriptions    = "<color=#FFD900><size=130%>Barrack:</size></color>\n   Summons three swordsmen to fight against incoming enemies.",
        },

        new TowerData
        {
            towerType       = "Barrack",
            level           = 2,
            SpawnObject     = "Swordsman_2",
            spawnRate       = 0.3f,
            fireAnimDelay   = 5f,
            rangeDetect     = 2.5f,
            rangeRaycast    = 0.7f,
            goldRequired    = 100,
            descriptions    = "<color=#FFD900><size=130%>Barrack:</size></color>\n   Summons three knights who heal themselves and allies. But only when they are not engaged in combat.",
        }
    };

    public List<TowerData> GetTowerDataForJson()
    {
        return towerDataList;
    }
}
