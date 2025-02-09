using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnArcherTower : MonoBehaviour
{
    [SerializeField] private bool initArcherTower;
    [SerializeField] private bool initMageTower;
    [SerializeField] private bool initBarrackTower;
    [SerializeField] private bool initCannonTower;

    [SerializeField] private InputControllerxxx inputController;
    [SerializeField] private Button button;

    private void LoadComponents()
    {
        inputController = FindObjectOfType<InputControllerxxx>();
        button = GetComponent<Button>();
    }

    private void AddBtnListener()
    {
        if(initArcherTower)
        {
            button.onClick.AddListener(() => inputController.InitArcherTowerBtn(button));
        }
        else if(initMageTower)
        {
            button.onClick.AddListener(() => inputController.InitMageTowerBtn(button));
        }
        else if(initBarrackTower)
        {
            button.onClick.AddListener(() => inputController.InitBarrackTowerBtn(button));
        }
        else if(initCannonTower)
        {
            button.onClick.AddListener(() => inputController.InitCannonTowerBtn(button));
        }
    }


}
