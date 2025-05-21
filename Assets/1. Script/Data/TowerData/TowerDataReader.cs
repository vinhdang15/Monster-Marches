using UnityEngine;

public class TowerDataReader : MonoBehaviour
{
    public          TowerDataListSO    towerDataListSO;

    public void PrepareGame()
    {
        towerDataListSO.towerDataList = JSONManager.towerDataList;
        LoadTowerInitGold();
    }

    private void LoadTowerInitGold()
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
