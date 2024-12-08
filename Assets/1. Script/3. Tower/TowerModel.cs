using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerModel : MonoBehaviour
{
    public string  TowerType { get ; set ; }
    public int     Level { get ; set ; }
    public string  BulletType { get ; set ; }
    public float   FireRate { get ; set ; }
    public float   RangeDetect { get ; set ; }
    public float   RangeRaycast { get ; set ; }
    public int     GoldRequired { get ; set ; }
    public string  Descriptions { get ; set ; }
    public TowerModel(TowerData _data)
    {
        TowerType           = _data.towerType;
        Level               = _data.level;
        BulletType          = _data.BulletType;
        FireRate            = _data.fireRate;
        RangeDetect         = _data.rangeDetect;
        RangeRaycast        = _data.rangeRaycast;
        GoldRequired        = _data.goldRequired;
        Descriptions        = _data.descriptions;
    }
    
    public static TowerModel Craete(TowerView TowerView, TowerData _data)
    {
        TowerModel TowerMode = TowerView.gameObject.AddComponent<TowerModel>();
        TowerMode.BuildingModeInit(_data);
        return TowerMode;
    }

    private void BuildingModeInit(TowerData _data)
    {
        TowerType           = _data.towerType;
        Level               = _data.level;
        BulletType          = _data.BulletType;
        FireRate            = _data.fireRate;
        RangeDetect         = _data.rangeDetect;
        RangeRaycast        = _data.rangeRaycast;
        GoldRequired        = _data.goldRequired;
        Descriptions        = _data.descriptions;
    }
    public void UpgradeTowerModel(TowerData _data)
    {
        TowerType        = _data.towerType;
        Level               = _data.level;
        BulletType          = _data.BulletType;
        FireRate            = _data.fireRate;
        RangeDetect         = _data.rangeDetect;
        RangeRaycast        = _data.rangeRaycast;
        GoldRequired        = _data.goldRequired;
        Descriptions        = _data.descriptions;
    }
}
