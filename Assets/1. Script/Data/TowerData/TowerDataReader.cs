using UnityEngine;

public class TowerDataReader : MonoBehaviour
{
    public static TowerDataReader    Instance { get; private set; }
    public TowerDataListSO              towerDataListSO;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        towerDataListSO.towerDataList = JSONManager.LoadTowerDataFromJson();
        LoadTowerInit();
    }

    private void LoadTowerInit()
    {
        string archerTowerString =  TowerType.ArcherTower.ToString();
        string mageTowerString   =  TowerType.MageTower.ToString();
        string barackTowerString =  TowerType.Barrack.ToString();
        string cannonTowerString =  TowerType.CannonTower.ToString();

        int archerTowerInitCost = towerDataListSO.GetGoldInit(archerTowerString);
        int mageTowerInitCost   = towerDataListSO.GetGoldInit(mageTowerString);
        int barackTowerInitCost = towerDataListSO.GetGoldInit(barackTowerString);
        int cannonTowerInitCost = towerDataListSO.GetGoldInit(cannonTowerString);
        
        towerDataListSO.TowerInitGoldList.Add(archerTowerInitCost);
        towerDataListSO.TowerInitGoldList.Add(mageTowerInitCost);
        towerDataListSO.TowerInitGoldList.Add(barackTowerInitCost);
        towerDataListSO.TowerInitGoldList.Add(cannonTowerInitCost);
    }
}
