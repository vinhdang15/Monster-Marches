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

    public override void Hide()
    {
        base.Hide();
        HideGuardPointBtn();
    }

    public void ShowGuardPointBtn()
    {
        guardPointBtn.SetActive(true);
    }

    public void HideGuardPointBtn()
    {
        guardPointBtn.SetActive(false);
    }
}
