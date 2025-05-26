using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JSONDataExporter : MonoBehaviour
{
    #region  MAP DESIGNE DATA
    public static void ExportMapDesignData(List<MapDesignData> mapDataList)
    {
        string json = JsonConvert.SerializeObject(mapDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(AssetPathManager.mapDesignDataJsonPath, json);

        Debug.Log("Export completed MapData JSON file");
    }
    #endregion

    #region  MAP PROGRESS DATA
    public static void ExportMapProgressData(List<MapProgressData> mapProgressDataList)
    {
        string json = JsonConvert.SerializeObject(mapProgressDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(AssetPathManager.mapProgressDataJsonPath, json);
        Debug.Log("Create completed MapProgressData JSON file");
    }
    #endregion

    #region DECOR OBJECT DATA
    public static void ExportDecorObjData(List<DecorObjData> decorObjDataList)
    {
        string json = JsonConvert.SerializeObject(decorObjDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(AssetPathManager.decorObjDataJsonPath, json);
        Debug.Log("Create completed DecorObjData JSON file");
    }
    #endregion

    #region WAY POINT DATA
    public static void ExportWayPointData(List<WayPointData> wayPointDataList)
    {
        string json = JsonConvert.SerializeObject(wayPointDataList, Formatting.Indented, new Vector2Converter());
        File.WriteAllText(AssetPathManager.waypointDataJsonPath, json);
        Debug.Log("Create completed WayPointData JSON file");
    }
    #endregion

    #region TOWER DATA
    public static void ExportTowerData(List<TowerData> towerDataList)
    {
        string json = JsonConvert.SerializeObject(towerDataList, Formatting.Indented);
        File.WriteAllText(AssetPathManager.towerDataJsonPath, json);
        Debug.Log("Create completed towerData JSON file");
    }
    #endregion

    #region BULLET DATA
    public static void ExportBulletData(List<BulletData> bulletDataList)
    {
        string json = JsonConvert.SerializeObject(bulletDataList, Formatting.Indented);
        File.WriteAllText(AssetPathManager.bulletDataJsonPath, json);
        Debug.Log("Create completed bulletData JSON file");
    }
    #endregion

    #region BULLET EFFECT DATA
    public static void ExportBulletEffectData(List<BulletEffectData> bulletEffectDataList)
    {
        string json = JsonConvert.SerializeObject(bulletEffectDataList, Formatting.Indented);
        File.WriteAllText(AssetPathManager.bulletEffectDataJsonPath, json);
        Debug.Log("Create completed BulletEffectData JSON file");
    }
    #endregion

    #region UNIT DATA
    public static void ExportUnitData(List<UnitData> unitDataList)
    {
        string json = JsonConvert.SerializeObject(unitDataList, Formatting.Indented);
        File.WriteAllText(AssetPathManager.unitDataJsonPath, json);
        Debug.Log("Create completed UnitData JSON file");
    }
    #endregion

    #region SKILL DATA
    public static void ExportSkillData(List<SkillData> skillDataList)
    {
        string json = JsonConvert.SerializeObject(skillDataList, Formatting.Indented);
        File.WriteAllText(AssetPathManager.SkillDataJsonPath, json);
        Debug.Log("Create completed SkillData JSON file");
    }
    #endregion

    #region ENEMY WAVE DATA
    public static void ExportEnemyWayData(List<EnemyWaveData> EnemyWaveDataList)
    {
        string json = JsonConvert.SerializeObject(EnemyWaveDataList, Formatting.Indented);
        File.WriteAllText(AssetPathManager.enemyWaveDataJsonPath, json);
        Debug.Log("Create completed EnemyWayData JSON file");
    }
    #endregion
}
