using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentSttPanel : UIElementBase
{
    [SerializeField] TextMeshProUGUI TowerType;
    [SerializeField] TextMeshProUGUI towerDamageText;
    [SerializeField] TextMeshProUGUI spawnRateText;

    public void SetCurrentSttText(TowerPresenter towerPresenter)
    {
        TowerType.text = towerPresenter.towerModel.TowerType.ToString();
        spawnRateText.text = towerPresenter.towerModel.SpawnRate.ToString() + "s";
        towerDamageText.text = towerPresenter.CurentTowerDamage.ToString();
    }
}
