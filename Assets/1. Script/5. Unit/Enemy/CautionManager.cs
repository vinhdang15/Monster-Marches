using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionManager : MonoBehaviour
{
    [SerializeField] SpawnEnemyManager spawnEnemyManager;
    [SerializeField] BtnCautionSlider cautionSlider;
    [SerializeField] Transform cautionParent;
    private List<BtnCautionSlider> btnCautionSliders = new List<BtnCautionSlider>();

    private void Start()
    {
        InitCaution();
        // RegisterNumberEnemyInWaveHandler();
    }

    public void InitCaution()
    {
        foreach(var spawnEnemy in spawnEnemyManager.SpawnEnemies)
        {
            Vector2 pos = spawnEnemy.GetCautionPos();
            BtnCautionSlider btnCautionSlider = Instantiate(cautionSlider, pos, Quaternion.identity,cautionParent);
            btnCautionSlider.gameObject.SetActive(false);
            spawnEnemy.btnCautionSlider = btnCautionSlider;
            btnCautionSlider.GetSpawnEnemyManager(spawnEnemyManager);
            btnCautionSlider.OnCautionClick += HideAllCautionSlider;
            btnCautionSliders.Add(btnCautionSlider);
        }
    }

    // private void RegisterNumberEnemyInWaveHandler()
    // {
    //     foreach(var spawnEnemy in spawnEnemyManager.SpawnEnemies)
    //     {
    //         spawnEnemy.OnNumberEnemyInWaveIsNull += HideCautionSlider;
    //         spawnEnemy.OnNumberEnemyInWave += ShowCautionSlider;
    //     }
    // }

    private void ShowCautionSlider(BtnCautionSlider btnCautionSlider)
    {
        btnCautionSlider.gameObject.SetActive(true);
    }

    private void HideCautionSlider(BtnCautionSlider btnCautionSlider)
    {
        btnCautionSlider.gameObject.SetActive(false);
    }

    private void HideAllCautionSlider()
    {
        foreach( var btnCautionSlider in btnCautionSliders)
        {
            btnCautionSlider.gameObject.SetActive(false);
        }
    }
}
