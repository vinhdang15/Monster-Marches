using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitMenu : UIElementBase
{
    [SerializeField] TextMeshProUGUI ArcherTowerInitGoldText;
    [SerializeField] TextMeshProUGUI mageTowerInitGoldText;
    [SerializeField] TextMeshProUGUI barrackTowerInitGoldText;
    [SerializeField] TextMeshProUGUI cannonTowerInitGoldText;

    private void Start()
    {
        GetInitTowerGold();
    }

    private void GetInitTowerGold()
    {
        string archerTowerString =  TowerType.ArcherTower.ToString().Trim().ToLower();
        string mageTowerString =  TowerType.MageTower.ToString().Trim().ToLower();
        string barackTowerString =  TowerType.Barrack.ToString().Trim().ToLower();
        string cannonTowerString =  TowerType.CannonTower.ToString().Trim().ToLower();
        ArcherTowerInitGoldText.text = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(archerTowerString).ToString();
        mageTowerInitGoldText.text = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(mageTowerString).ToString();
        barrackTowerInitGoldText.text = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(barackTowerString).ToString();
        cannonTowerInitGoldText.text = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(cannonTowerString).ToString();
    }
}
