using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerModel : MonoBehaviour
{
    public string  TowerType { get ; set ; }
    public int     Level { get ; set ; }
    public string  SpawnObject { get ; set ; }
    public float   SpawnRate { get ; set ; }
    public float   FireAnimDelay  { get ; set ; }
    public float   RangeDetect { get ; set ; }
    public float   RangeRaycast { get ; set ; }
    public int     GoldRequired { get ; set ; }
    public string  Descriptions { get ; set ; }
    
    public static TowerModel Craete(TowerViewBase TowerView, TowerData _data)
    {
        TowerModel TowerMode = TowerView.gameObject.AddComponent<TowerModel>();
        TowerMode.InitBuildingMode(_data);
        return TowerMode;
    }

    private void InitBuildingMode(TowerData _data)
    {
        TowerType           = _data.towerType;
        Level               = _data.level;
        SpawnObject         = _data.SpawnObject;
        SpawnRate           = _data.spawnRate;
        FireAnimDelay       = _data.fireAnimDelay;
        RangeDetect         = _data.rangeDetect;
        RangeRaycast        = _data.rangeRaycast;
        GoldRequired        = _data.goldRequired;
        Descriptions        = _data.descriptions;
    }
    public void UpgradeTowerModel(TowerData _data)
    {
        TowerType           = _data.towerType;
        Level               = _data.level;
        SpawnObject          = _data.SpawnObject;
        SpawnRate            = _data.spawnRate;
        RangeDetect         = _data.rangeDetect;
        RangeRaycast        = _data.rangeRaycast;
        GoldRequired        = _data.goldRequired;
        Descriptions        = _data.descriptions;
    }
}
