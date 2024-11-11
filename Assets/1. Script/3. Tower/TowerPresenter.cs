using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPresenter : MonoBehaviour
{
    public TowerModel           towerModel;
    public TowerView            towerView;
    public GameObject           spawnObject;
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

    public IEnumerator SpawnBulletCoroutine(List<Enemy> a)
    {
        while(true)
        {
            if(a.Count > 0)
            {
                GameObject bullet = Instantiate(spawnObject, spawnBulletTrans);
                //bullet.GetEnemyPos();
            }
            
        }
    }
}
