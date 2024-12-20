using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeMenu : UIElementBase
{
    [SerializeField] GameObject guardPointBtn;
    [SerializeField] TextMeshProUGUI upgradeTowerGoldText;
    [SerializeField] TextMeshProUGUI sellTowerGoldText;

    public void UpdateText(TowerPresenter towerPresenter)
    {
        upgradeTowerGoldText.text = towerPresenter.GoldUpdrade.ToString();
        sellTowerGoldText.text = towerPresenter.GoldRefund.ToString();
    }

    public void ShowGuardPointBtn(bool show)
    {
        if(show) guardPointBtn.SetActive(true);
        else guardPointBtn.SetActive(false);
    }
}
