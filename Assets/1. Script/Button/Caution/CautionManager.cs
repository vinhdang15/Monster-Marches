using System.Collections.Generic;
using UnityEngine;

public class CautionManager : MonoBehaviour
{
    [SerializeField] BtnCaution cautionBtnPref;
    private EnemySpawnerManager enemySpawnManager;
    private Transform cautionParent;
    [SerializeField] private List<BtnCaution> cautionBtnList = new List<BtnCaution>();

    public void PrepareGame()
    {
        LoadComponents();
        RegisterSpawnEnemyManagerEvent();
    }

    public void ClearCautionBtnManager()
    {
        CleanupCautionBtn();
    }

    private void CleanupCautionBtn()
    {
        foreach(BtnCaution button in cautionBtnList)
        {
            button.Cleanup();
            Destroy(button.gameObject);
        }
        cautionBtnList.Clear();
    }

    private void LoadComponents()
    {
        cautionParent = GameObject.Find(InitNameObject.CanvasWorldSpace.ToString()).transform;
        enemySpawnManager = FindObjectOfType<EnemySpawnerManager>();
    }

    private void RegisterSpawnEnemyManagerEvent()
    {
        enemySpawnManager.OnCautionBtnClicked += HandleHideAllCautionSlider;
    }

    public void InitializeCautionBtn()
    {   
        foreach(var enemyspawner in enemySpawnManager.enemySpawnerList)
        {
            // init and set pos btnCautionSlider
            Vector2 pos = enemyspawner.GetCautionBtnPos();
            BtnCaution cautionBtn = Instantiate(cautionBtnPref, pos, Quaternion.identity, cautionParent);
            cautionBtn.CautionBtnPrepareGame();
            enemyspawner.SetCautionBtm(cautionBtn);
            cautionBtn.SetSpawnEnemyManager(enemySpawnManager);

            CheckShowFristWaveCautionBtn(enemyspawner, cautionBtn);
            cautionBtnList.Add(cautionBtn);
        }
    }

    private void CheckShowFristWaveCautionBtn(EnemySpawner enemySpawner, BtnCaution cautionBtn)
    {
        if(enemySpawner.HasEnemyInWave(0))
         {
            cautionBtn.isFirstWave = true;
            cautionBtn.StartActiveCautionFill();
        }
        else cautionBtn.HideCautionFill();
    }

    private void HandleHideAllCautionSlider()
    {
        foreach( var cautionBtn in cautionBtnList)
        {
            if(cautionBtn.IsCautionFillActive())
            {
                cautionBtn.HideCautionFill();
            }
        }
    }
}
