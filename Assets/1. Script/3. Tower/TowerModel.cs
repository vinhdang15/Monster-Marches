using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerModel : TowerBase
{
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
    
    public static TowerModel Craete(TowerView towerView, TowerData _data)
    {
        TowerModel towerMode = towerView.gameObject.AddComponent<TowerModel>();
        towerMode.TowerModeInit(_data);
        return towerMode;
    }

    private void TowerModeInit(TowerData _data)
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
    public override void UpgradeTowerModel(TowerData _data)
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
}
