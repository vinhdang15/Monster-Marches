using System;
using TMPro;
using UnityEngine;

public class UpgradeMenu : UIElementBase
{
    [SerializeField] GameObject guardPointBtn;
    [SerializeField] TextMeshProUGUI upgradeTowerGoldText;
    [SerializeField] TextMeshProUGUI sellTowerGoldText;
    [SerializeField] ButtonColor UpgradeButtonColor;

    private void Awake()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        UpgradeButtonColor = transform.GetChild(1).GetComponent<ButtonColor>();
    }

    public void UpdateText(TowerPresenter towerPresenter)
    {
        upgradeTowerGoldText.text = towerPresenter.GoldUpgrade.ToString();
        sellTowerGoldText.text = towerPresenter.GoldRefund.ToString();
    }

    public void UpdateButtonColor(TowerPresenter towerPresenter, int currentGold)
    {
        if(currentGold < towerPresenter.GoldUpgrade)
        {
            UpgradeButtonColor.GreyOutButton(true);
        }
        else
        {
            UpgradeButtonColor.GreyOutButton(false);
        }
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
