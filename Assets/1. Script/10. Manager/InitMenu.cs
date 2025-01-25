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

    [SerializeField] List<TextMeshProUGUI> TowerInitGOldText;
    private List<ButtonColor> buttonColorControllers = new List<ButtonColor>();
    [SerializeField] private List<int> towerInitGold = new List<int>();

    private void Awake()
    {
        LoadComponent();
    }

    private void Start()
    {
        towerInitGold = CSVTowerDataReader.Instance.towerDataList.TowerInitGoldList;
        UpdateTowerInitGoldText();
        base.Hide();
    }

    private void LoadComponent()
    {
        for(int i = 1; i < 5; i++)
        {
            ButtonColor initButton = transform.GetChild(i).GetComponent<ButtonColor>();
            buttonColorControllers.Add(initButton);
        }
    }

    private void UpdateTowerInitGoldText()
    {
        archerTowerInitGoldText.text = towerInitGold[0].ToString();
        mageTowerInitGoldText.text =towerInitGold[1].ToString();
        barrackTowerInitGoldText.text = towerInitGold[2].ToString();
        cannonTowerInitGoldText.text = towerInitGold[3].ToString();
    }

    public void ButtonCheckInitGoldRequire(int currentGold)
    {
        for(int i = 0; i < buttonColorControllers.Count; i++)
        {
            if(currentGold < towerInitGold[i])
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