using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] InputController inputController;
    [Header("GameStatus")]
    [SerializeField] GameSttPanel gameSttPanel;
    [Header("TowerBuildingStatus")]
    [SerializeField] InitMenu initMenu;
    [SerializeField] UpgradeMenu upgradeMenu;

    [Header("TowerStatus")]
    [SerializeField] CurrentSttPanel currentSttPanel;
    [SerializeField] UpgradeSttPanel upgradeSttPanel;

    #region MENU, PANEL VISIBLE
    public void ShowInitMenu(Vector2 pos)
    {
        initMenu.ShowInPos(pos);
    }

    public void HideInitPanel()
    {
        initMenu.Hide();
    }

    public void ShowUpgradeMenu(Vector2 pos)
    {
        upgradeMenu.ShowInPos(pos);
    }
    
    public void HideUpgradePanel()
    {
        upgradeMenu.Hide();
    }
    
    public void ShowCurrentSttPanel()
    {
        currentSttPanel.Show();
    }

    public void HideCurrentSttPanel()
    {
        currentSttPanel.Hide();
    }

    public void ShowUpgradeSttMenu(Vector2 pos)
    {
        upgradeSttPanel.ShowInPos(pos);
    }
    #endregion

    #region PANEL, MENU VISIBLE
    private void RegisterInputControllerEvent()
    {
        
    }
    #endregion
}
