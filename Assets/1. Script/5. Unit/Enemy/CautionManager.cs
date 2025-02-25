using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionManager : MonoBehaviour
{
    [SerializeField] BtnCaution cautionBtnPref;
    private SpawnEnemyManager spawnEnemyManager;
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
        spawnEnemyManager = FindObjectOfType<SpawnEnemyManager>();
    }

    private void RegisterSpawnEnemyManagerEvent()
    {
        spawnEnemyManager.OnCautionClick += HandleHideAllCautionSlider;
    }

    private void UnregisterSpawnEnemyManagerEvent()
    {
        spawnEnemyManager.OnCautionClick -= HandleHideAllCautionSlider;
    }

    public void InitCaution()
    {
        foreach(var spawnEnemy in spawnEnemyManager.SpawnEnemies)
        {
            // init and set pos btnCautionSlider
            Vector2 pos = spawnEnemy.GetCautionPos();
            BtnCaution cautionBtn = Instantiate(cautionBtnPref, pos, Quaternion.identity, cautionParent);
            cautionBtn.CautionBtnPrepareGame();
            spawnEnemy.cautionBtn = cautionBtn;

            cautionBtn.SetSpawnEnemyManager(spawnEnemyManager);
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
