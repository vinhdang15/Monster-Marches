using UnityEngine;

public class TowerDataReader : MonoBehaviour
{
    public          TowerDataListSO    towerDataListSO;

    public void PrepareGame()
    {
        towerDataListSO.towerDataList = JSONDataLoader.towerDataList;
        LoadTowerInitGold();
    }

    private void LoadTowerInitGold()
    {
        string archerTowerString = TowerType.ArcherTower.ToString();
        string mageTowerString = TowerType.MageTower.ToString();
        string barackTowerString = TowerType.Barrack.ToString();
        string cannonTowerString = TowerType.CannonTower.ToString();

        int archerTowerInitGold = towerDataListSO.GetGoldInit(archerTowerString);
        int mageTowerInitGold = towerDataListSO.GetGoldInit(mageTowerString);
        int barackTowerInitGold = towerDataListSO.GetGoldInit(barackTowerString);
        int cannonTowerInitGold = towerDataListSO.GetGoldInit(cannonTowerString);

        towerDataListSO.towerInitGoldList.Clear();
        towerDataListSO.towerInitGoldList.Add(archerTowerInitGold);
        towerDataListSO.towerInitGoldList.Add(mageTowerInitGold);
        towerDataListSO.towerInitGoldList.Add(barackTowerInitGold);
        towerDataListSO.towerInitGoldList.Add(cannonTowerInitGold);
    }
}
