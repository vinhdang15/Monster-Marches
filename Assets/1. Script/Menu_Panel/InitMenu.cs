using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitMenu : UIElementBase
{
    [SerializeField] TextMeshProUGUI archerTowerInitGoldText;
    [SerializeField] TextMeshProUGUI mageTowerInitGoldText;
    [SerializeField] TextMeshProUGUI barrackTowerInitGoldText;
    [SerializeField] TextMeshProUGUI cannonTowerInitGoldText;

    private List<ButtonColor> buttonColorControllers = new();
    [SerializeField] private List<int> towerInitGoldList = new();

    public void PrepareGame(TowerDataReader towerDataReader)
    {
        LoadComponent(towerDataReader);
    }

    private void LoadComponent(TowerDataReader towerDataReader)
    {
        towerInitGoldList = towerDataReader.towerDataListSO.towerInitGoldList;
        UpdateTowerInitGoldText();
        
        for (int i = 1; i < 5; i++)
        {
            ButtonColor initButton = transform.GetChild(i).GetComponent<ButtonColor>();
            buttonColorControllers.Add(initButton);
        }
    }

    private void UpdateTowerInitGoldText()
    {
        archerTowerInitGoldText.text    = towerInitGoldList[0].ToString();
        mageTowerInitGoldText.text      = towerInitGoldList[1].ToString();
        barrackTowerInitGoldText.text   = towerInitGoldList[2].ToString();
        cannonTowerInitGoldText.text    = towerInitGoldList[3].ToString();
    }

    public void ButtonCheckInitGoldRequire(int currentGold)
    {
        for(int i = 0; i < buttonColorControllers.Count; i++)
        {
            if(currentGold < towerInitGoldList[i])
            {
                buttonColorControllers[i].GreyOutButton(true);
            }
            else
            {
                buttonColorControllers[i].GreyOutButton(false);
            }
        }
    }
}