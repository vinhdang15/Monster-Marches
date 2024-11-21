using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPresenter : MonoBehaviour
{
    public TowerModel           towerModel;
    public TowerView            towerView;
    private Transform           spawnBulletTrans;

    public static TowerPresenter Create(TowerModel towerModel, TowerView towerView)
    {
        TowerPresenter towerPresenter = towerView.gameObject.AddComponent<TowerPresenter>();
        towerPresenter.TowerPresenterInit(towerModel, towerView);
        return towerPresenter;
    } 
    
    public void TowerPresenterInit(TowerModel towerModel, TowerView towerView)
    {
        this.towerModel = towerModel;
        this.towerView  = towerView;
        spawnBulletTrans = towerView.GetSpawnBulletTrans();
        InitTowerRange();
    }

    public void InitTowerRange()
    {
        towerView.SetRangeDetect(towerModel.RangeDetect);
        towerView.SetRangeRaycat(towerModel.RangeRaycast);
    }

    public BulletBase GetBullet()
    {
        return towerView.BulletList.Find(bullet => bullet.type == towerModel.BulletType);
    }
}
