using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPresenter : MonoBehaviour
{
    public TowerModel           towerModel;
    public TowerView            towerView;

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
        InitTowerRange();
    }

    public void InitTowerRange()
    {
        towerView.SetRangeDetect(towerModel.RangeDetect);
        towerView.SetRangeRaycat(towerModel.RangeRaycast);
    }
}
