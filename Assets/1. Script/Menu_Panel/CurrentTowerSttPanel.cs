using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentTowerSttPanel : UIElementBase
{
    [SerializeField] TextMeshProUGUI TowerType;
    [SerializeField] TextMeshProUGUI towerDamageText;
    [SerializeField] TextMeshProUGUI spawnRateText;

    public void SetCurrentSttText(TowerPresenter towerPresenter)
    {
        TowerType.text = towerPresenter.towerModel.TowerType.ToString();
        spawnRateText.text = "Rate: " + towerPresenter.towerModel.SpawnRate.ToString() + "s";
        towerDamageText.text = "Damage: " + towerPresenter.CurrentTowerDamage.ToString();
    }
}
