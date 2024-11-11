using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GamePlayManager gamePlayManager;
    [Header("For UI")]
    [SerializeField] TextMeshProUGUI goldText;
    [Header("Panel Gold Text")]
    [SerializeField] TextMeshProUGUI ArcherTowerInitGoldText;
    [SerializeField] TextMeshProUGUI mageTowerInitGoldText;
    [SerializeField] TextMeshProUGUI barrackTowerInitGoldText;
    [SerializeField] TextMeshProUGUI cannonTowerInitGoldText;
    [SerializeField] TextMeshProUGUI upgradeTowerGoldText;
    [SerializeField] TextMeshProUGUI sellTowerGoldText;

    private void Start()
    {
        gamePlayManager = GetComponent<GamePlayManager>();
        StartCoroutine(WaitForDataLoadAnhProcess());
        gamePlayManager.OnSelectedTower += HandeSelectedTower;
        gamePlayManager.OnGoldChange += HandleGoldChange;
        HandleGoldChange();
    }

    private void HandleGoldChange()
    {
        goldText.text = gamePlayManager.Gold.ToString();
    }
    private IEnumerator WaitForDataLoadAnhProcess()
    {
        yield return new WaitUntil(() => gamePlayManager.IsDataLoaded);
        SetupTowerInitGoldText();
    }
    private void SetupTowerInitGoldText()
    {
        ArcherTowerInitGoldText.text = gamePlayManager.archerTOwerInitGold.ToString();
        mageTowerInitGoldText.text = gamePlayManager.mageTowerInitGold.ToString();
        barrackTowerInitGoldText.text = gamePlayManager.barrackTowerInitGold.ToString();
        cannonTowerInitGoldText.text = gamePlayManager.cannonTowerInitGold.ToString();
    }

    private void HandeSelectedTower()
    {
        upgradeTowerGoldText.text = gamePlayManager.GetTowerGoldUpgrade().ToString();
        sellTowerGoldText.text = gamePlayManager.GetTowerGoldRefund().ToString();
    }
}
