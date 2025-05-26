using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AssetPathManager
{
    public static string mapProgressDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/MapProgressData.json");
    public static string mapDesignDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/MapDesignData.json");
    public static string decorObjDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/DecorObjData.json");
    public static string waypointDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/WayPointData.json");
    public static string towerDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/TowerData.json");
    public static string bulletDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/BulletData.json");
    public static string unitDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/UnitData.json");
    public static string SkillDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/SkillData.json");
    public static string bulletEffectDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/BulletEffectData.json");
    public static string enemyWaveDataJsonPath = Path.Combine(Application.streamingAssetsPath, "JSON/EnemyWaveData.json");
}
