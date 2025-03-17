using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CautionManager : MonoBehaviour
{
    [SerializeField] BtnCaution cautionBtnPref;
    private EnemySpawnerManager enemySpawnManager;
    private Transform cautionParent;
    private List<BtnCaution> cautionBtnList = new List<BtnCaution>();

    public void CautionManagerPrepareGame()
    {
        LoadComponents();
        InitCaution();
        RegisterSpawnEnemyManagerEvent();
    }

    private void OnDisable()
    {
        UnregisterSpawnEnemyManagerEvent();
    }

    private void LoadComponents()
    {
        cautionParent = GameObject.Find(InitNameObject.CanvasWorldSpace.ToString()).transform;
        enemySpawnManager = FindObjectOfType<EnemySpawnerManager>();
    }

    private void RegisterSpawnEnemyManagerEvent()
    {
        enemySpawnManager.OnCautionClick += HandleHideAllCautionSlider;
    }

    private void UnregisterSpawnEnemyManagerEvent()
    {
        enemySpawnManager.OnCautionClick -= HandleHideAllCautionSlider;
    }

    public void InitCaution()
    {   
        int i = 1;
        foreach(var spawnEnemy in enemySpawnManager.SpawnEnemies)
        {
            // init and set pos btnCautionSlider
            Vector2 pos = spawnEnemy.GetCautionPos();
            BtnCaution cautionBtn = Instantiate(cautionBtnPref, pos, Quaternion.identity, cautionParent);
            cautionBtn.gameObject.name = i.ToString();
            i++;
            cautionBtn.CautionBtnPrepareGame();
            spawnEnemy.cautionBtn = cautionBtn;

            cautionBtn.SetSpawnEnemyManager(enemySpawnManager);
            this.cautionBtnList.Add(cautionBtn);
        }
    }

    private void HandleHideAllCautionSlider()
    {
        foreach( var cautionBtn in cautionBtnList)
        {
            if(cautionBtn.isCautionFillActive())
            {
                cautionBtn.HideCautionFill();
            }
        }
    }
}
