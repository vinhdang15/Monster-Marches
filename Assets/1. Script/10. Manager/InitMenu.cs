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
    // private ButtonColorController archerTowerButton;
    // private ButtonColorController mageTowerButton;
    // private ButtonColorController barrackTowerButton;
    // private ButtonColorController cannonTowerButton;
    private List<ButtonColorController> buttonColorControllers = new List<ButtonColorController>();
    private List<int> towerInitCosts = new List<int>();

    private void Awake()
    {
        LoadComponent();
    }
    private void Start()
    {
        GetInitTowerGold();
        gameObject.SetActive(false);
    }

    private void LoadComponent()
    {
        // archerTowerButton       = transform.GetChild(1).GetComponent<ButtonColorController>();
        // mageTowerButton         = transform.GetChild(2).GetComponent<ButtonColorController>();
        // barrackTowerButton      = transform.GetChild(3).GetComponent<ButtonColorController>();
        // cannonTowerButton       = transform.GetChild(4).GetComponent<ButtonColorController>();

        for(int i = 1; i < 5; i++)
        {
            ButtonColorController initButton = transform.GetChild(i).GetComponent<ButtonColorController>();
            buttonColorControllers.Add(initButton);
        }
    }

    private void GetInitTowerGold()
    {
        string archerTowerString =  TowerType.ArcherTower.ToString().Trim().ToLower();
        string mageTowerString =  TowerType.MageTower.ToString().Trim().ToLower();
        string barackTowerString =  TowerType.Barrack.ToString().Trim().ToLower();
        string cannonTowerString =  TowerType.CannonTower.ToString().Trim().ToLower();

        int archerTowerInitCost = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(archerTowerString);
        int mageTowerInitCost = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(mageTowerString);
        int barackTowerInitCost = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(barackTowerString);
        int cannonTowerInitCost = CSVTowerDataReader.Instance.towerDataList.GetGoldInit(cannonTowerString);
        
        towerInitCosts.Add(archerTowerInitCost);
        towerInitCosts.Add(mageTowerInitCost);
        towerInitCosts.Add(barackTowerInitCost);
        towerInitCosts.Add(cannonTowerInitCost);

        archerTowerInitGoldText.text = archerTowerInitCost.ToString();
        mageTowerInitGoldText.text = mageTowerInitCost.ToString();
        barrackTowerInitGoldText.text = barackTowerInitCost.ToString();
        cannonTowerInitGoldText.text = cannonTowerInitCost.ToString();

    }
    
    public override void CheckAndShowInPos(Vector2 pos, int currentGold)
    {
        for(int i = 0; i < buttonColorControllers.Count; i++)
        {
            if(currentGold < towerInitCosts[i])
            {
                buttonColorControllers[i].GreyOutButtonImage(true);
            }
            else
            {
                buttonColorControllers[i].GreyOutButtonImage(false);
            }
        }
        base.CheckAndShowInPos(pos, currentGold);
    }
}
