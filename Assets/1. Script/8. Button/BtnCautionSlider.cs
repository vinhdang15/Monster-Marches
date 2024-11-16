using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCautionSlider : BtnBase
{
    private SpawnEnemyManager spawnEnemyManager;
    public event Action OnCautionClick;
    protected override void Start()
    {
        base.Start();
    }

    public void GetSpawnEnemyManager(SpawnEnemyManager _spawnEnemyManager)
    {
        spawnEnemyManager = _spawnEnemyManager;
    }

    protected override void OnButtonClick()
    {
        //PlayClickSound();
        spawnEnemyManager.CautionClick();
        OnCautionClick?.Invoke();
    }

    

}
