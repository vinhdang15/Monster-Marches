using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class TowerManager : MonoBehaviour
{
    public BulletManager bulletManager;
    [SerializeField] CSVTowerDataReader towerDataReader;
    [SerializeField] CSVBulletDataReader bulletDataReader;
    [SerializeField] List<TowerView> towerPrefabList = new List<TowerView>();
    public List<TowerPresenter> towerList = new List<TowerPresenter>();
    public Dictionary<TowerPresenter, PresenterData> towerExtraData = new Dictionary<TowerPresenter, PresenterData>();
    public EmptyPlot       selectedEmptyPlot;
    public TowerPresenter selectedTower;
    public delegate void SelectedTowerPersenterHandler(TowerPresenter towerPresenter);
    public event SelectedTowerPersenterHandler OnSelectedTowerPersenter;
    bool check;
    private void Awake()
    {
        
    }
    void Start()
    {

    }
    void Update()
    {

    }

    #region INIT TOWER
    public void InitTower(Vector3 pos, TowerType towerType)
    {
        TowerData towerData = towerDataReader.towerDataList.GetTowerData(towerType.ToString(), 1);
        TowerView towerView = Instantiate(towerPrefabList[(int)towerType], pos, Quaternion.identity, transform);
        TowerModel towerModel = TowerModel.Craete(towerView,towerData);
        TowerPresenter towerPresenter = TowerPresenter.Create(towerModel, towerView);

        towerPresenter.towerView.OnSelectedTowerView += (TowerView) => HandleSelectedTowerView(towerPresenter);
        towerPresenter.towerView.OnEnemyEnter += (enmey, view) => HanldeEnemyEnter(enmey, towerPresenter);

        towerExtraData[towerPresenter] = new PresenterData();
        towerExtraData[towerPresenter].emptyPlot = selectedEmptyPlot;
        
        towerExtraData[towerPresenter].RangeDetectUpgrade = towerDataReader.towerDataList.GetRangeDetect(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldUpdrade = towerDataReader.towerDataList.GetGoldRequired(towerType.ToString(), 2);
        towerExtraData[towerPresenter].GoldRefund += towerModel.GoldRequired;

        selectedEmptyPlot.HideEmptyPlot();

        StartCoroutine(SpawnBulletCorountine(towerPresenter));

        this.towerList.Add(towerPresenter);    
    }

    public void InitArcherTower(Vector3 pos)
    {
        InitTower(pos,TowerType.ArcherTower);
    }

    public void InitMageTower(Vector3 pos)
    {
        InitTower(pos,TowerType.MageTower);
    }

    public void InitBarackTower(Vector3 pos)
    {
        InitTower(pos,TowerType.BarrackTower);
    }

    public void InitCannonTower(Vector3 pos)
    {
        InitTower(pos,TowerType.CannonTower);
    }
    #endregion

    #region SENT SELECTED TOWER TO GAMEPLAY MANAGER
    private void HandleSelectedTowerView(TowerPresenter towerPresenter)
    {
        OnSelectedTowerPersenter?.Invoke(towerPresenter);
    }
    #endregion

    public void UpdateTowerExtraData(TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter].GoldUpdrade = UpdateGoldUpgrade(towerPresenter);
        towerExtraData[towerPresenter].RangeDetectUpgrade = UpdateRangeDetectUpgradeData(towerPresenter);
    }
    private int UpdateGoldUpgrade(TowerPresenter towerPresenter)
    {
        string towerType = towerPresenter.towerModel.TowerType;
        int TowerLevel = towerPresenter.towerModel.Level;
        return towerDataReader.towerDataList.GetGoldRequired(towerType, TowerLevel + 1);
    }

    private float UpdateRangeDetectUpgradeData(TowerPresenter towerPresenter)
    {
        string towerType = towerPresenter.towerModel.TowerType;
        int TowerLevel = towerPresenter.towerModel.Level;
        return towerDataReader.towerDataList.GetRangeDetect(towerType, TowerLevel + 1);
    }

    public void UpdateRangeDetection(TowerPresenter towerPresenter)
    {
        float rangeDetect = towerPresenter.towerModel.RangeDetect;
        towerPresenter.towerView.SetRangeDetect(rangeDetect);
    }

    public void UpdateRangeDetectionUpgrade(TowerPresenter towerPresenter)
    {
        float rangeDetectUpgrade = towerExtraData[towerPresenter].RangeDetectUpgrade;
        towerPresenter.towerView.SetRangeDetectUpgrade(rangeDetectUpgrade);
    }

    public void AddGoldRefund(TowerPresenter towerPresenter, int gold)
    {
        towerExtraData[towerPresenter].GoldRefund += gold;
    }

    #region PROCESS ENEMY
    private void HanldeEnemyEnter(Enemy enemy, TowerPresenter towerPresenter)
    {
        towerExtraData[towerPresenter].enemies.Add(enemy);
    }

    private IEnumerator SpawnBulletCorountine(TowerPresenter towerPresenter)
    {
        Debug.Log("init corountine");
        List<Enemy> towerPresentEnemiesList = towerExtraData[towerPresenter].enemies;
        while(true)
        {
            if(towerPresentEnemiesList.Count > 0)
            {
                BulletBase bullet = Instantiate(towerPresenter.GetBullet(), towerPresenter.towerView.GetSpawnBulletTrans().position,Quaternion.identity, bulletManager.transform);
                Debug.Log(bullet.type);
                BulletData bulletData = bulletDataReader.bulletDataList.GetBulletData(bullet.type);
                bullet.Init(bulletData, towerPresentEnemiesList[0]);
                bulletManager.Bullets.Add(bullet);
            }
            yield return new WaitForSeconds(towerPresenter.towerModel.FireRate);
        }
    }
    #endregion

}

public enum TowerType
{
    ArcherTower = 0,
    MageTower = 1,
    BarrackTower = 2,
    CannonTower = 3
}

[System.Serializable]
public class PresenterData 
{
    public List<Enemy>  enemies = new List<Enemy>();
    public EmptyPlot    emptyPlot;
    public float        RangeDetectUpgrade { set ; get ; }
    public int          GoldUpdrade { set ; get ; }
    public int          GoldRefund { set ; get ; } = 0;
}
