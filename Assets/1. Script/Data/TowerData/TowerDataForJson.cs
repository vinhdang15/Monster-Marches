using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerDataForJson : MonoBehaviour
{
    private List<TowerData> towerDataList = new()
    {
        // ArcherTower
        new TowerData
        {
            towerType       = TowerType.ArcherTower.ToString(),
            level           = 1,
            SpawnObject     = BulletID.Arrow_1.ToString(),
            spawnRate       = 1f,
            fireAnimDelay   = 0.25f,
            rangeDetect     = 2.5f,
            rangeRaycast    = 0.7f,
            goldRequired    = 50,
            descriptions    = "<color=#FFD900><size=130%>Archer Tower:</size></color>\n  Shoots at a high speed. Allowing for rapid attacks.",
        },

        new TowerData
        {
            towerType       = TowerType.ArcherTower.ToString(),
            level           = 2,
            SpawnObject     = BulletID.Arrow_2.ToString(),
            spawnRate       = 1f,
            fireAnimDelay   = 0.25f,
            rangeDetect     = 3f,
            rangeRaycast    = 0.7f,
            goldRequired    = 70,
            descriptions    = "<color=#FFD900><size=130%>Archer Tower:</size></color>\n  Arrows apply a damage-over-time effect, steadily reducing the enemy's health."
        },

        // MageTower
        new TowerData
        {
            towerType       = TowerType.MageTower.ToString(),
            level           = 1,
            SpawnObject     = BulletID.MagicBall_1.ToString(),
            spawnRate       = 1f,
            fireAnimDelay   = 0.4f,
            rangeDetect     = 2.3f,
            rangeRaycast    = 0.7f,
            goldRequired    = 70,
            descriptions    = "<color=#FFD900><size=130%>Mage tower:</size></color>\n  The Magic Ball hits the target and continues to deal damage over time.",
        },

        new TowerData
        {
            towerType       = TowerType.MageTower.ToString(),
            level           = 2,
            SpawnObject     = BulletID.MagicBall_2.ToString(),
            spawnRate       = 1f,
            fireAnimDelay   = 0.4f,
            rangeDetect     = 2.8f,
            rangeRaycast    = 0.7f,
            goldRequired    = 100,
            descriptions    = "<color=#FFD900><size=130%>Mage tower:</size></color>\n  The Magic Ball deals damage over time and slows the enemy's movement.",
        },

        // CannonTower
        new TowerData
        {
            towerType       = TowerType.CannonTower.ToString(),
            level           = 1,
            SpawnObject     = BulletID.Bomb_1.ToString(),
            spawnRate       = 1.5f,
            fireAnimDelay   = 0.5f,
            rangeDetect     = 2f,
            rangeRaycast    = 0.7f,
            goldRequired    = 80,
            descriptions    = "<color=#FFD900><size=130%>Cannon Tower:</size></color>\n  Bombs explode on impact. Dealing damage to all enemies in the area.",
        },

        new TowerData
        {
            towerType       = TowerType.CannonTower.ToString(),
            level           = 2,
            SpawnObject     = BulletID.Bomb_2.ToString(),
            spawnRate       = 1.3f,
            fireAnimDelay   = 0.5f,
            rangeDetect     = 2.5f,
            rangeRaycast    = 0.7f,
            goldRequired    = 120,
            descriptions    = "<color=#FFD900><size=130%>Cannon Tower:</size></color>\n  Bombs deal greater damage, making them more effective against groups of enemies.",
        },

        // Barrack
        new TowerData
        {
            towerType       = TowerType.Barrack.ToString(),
            level           = 1,
            SpawnObject     = UnitID.Soldier_1.ToString(),
            spawnRate       = 5f,
            fireAnimDelay   = 0f,
            rangeDetect     = 2f,
            rangeRaycast    = 0.7f,
            goldRequired    = 70,
            descriptions    = "<color=#FFD900><size=130%>Barrack:</size></color>\n   Summons three swordsmen to fight against incoming enemies.",
        },

        new TowerData
        {
            towerType       = TowerType.Barrack.ToString(),
            level           = 2,
            SpawnObject     = UnitID.Soldier_2.ToString(),
            spawnRate       = 0.3f,
            fireAnimDelay   = 5f,
            rangeDetect     = 2.5f,
            rangeRaycast    = 0.7f,
            goldRequired    = 100,
            descriptions    = "<color=#FFD900><size=130%>Barrack:</size></color>\n   Summons three knights who heal themselves and allies when not engaged in combat.",
        }
    };

    public List<TowerData> GetTowerDataForJson()
    {
        return towerDataList;
    }
}
