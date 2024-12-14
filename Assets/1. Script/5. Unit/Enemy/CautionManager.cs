using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautionManager : MonoBehaviour
{
    [SerializeField] BtnCautionSlider cautionSlider;
    [SerializeField] Transform cautionParent;
    private List<BtnCautionSlider> btnCautionSliders = new List<BtnCautionSlider>();

    private void OnDisable()
    {
        foreach(var btnCautionSlider in btnCautionSliders)
        {
            UnregisterBtnCautionSliderEvent(btnCautionSlider);
        }
    }

    public void InitCaution(SpawnEnemyManager spawnEnemyManager)
    {
        foreach(var spawnEnemy in spawnEnemyManager.SpawnEnemies)
        {
            // init and set po btnCautionSlider
            Vector2 pos = spawnEnemy.GetCautionPos();
            BtnCautionSlider btnCautionSlider = Instantiate(cautionSlider, pos, Quaternion.identity,cautionParent);
            btnCautionSlider.gameObject.SetActive(false);
            spawnEnemy.btnCautionSlider = btnCautionSlider;

            // adsigne click event for btnCautionSlider
            btnCautionSlider.GetSpawnEnemyManager(spawnEnemyManager);
            RegisterBtnCautionSliderEvent(btnCautionSlider);
            btnCautionSliders.Add(btnCautionSlider);
        }
    }

    private void RegisterBtnCautionSliderEvent(BtnCautionSlider btnCautionSlider)
    {
        btnCautionSlider.OnCautionClick += HandleHideAllCautionSlider;
    }

    private void UnregisterBtnCautionSliderEvent(BtnCautionSlider btnCautionSlider)
    {
        btnCautionSlider.OnCautionClick -= HandleHideAllCautionSlider;
    }

    private void ShowCautionSlider(BtnCautionSlider btnCautionSlider)
    {
        btnCautionSlider.gameObject.SetActive(true);
    }

    private void HideCautionSlider(BtnCautionSlider btnCautionSlider)
    {
        btnCautionSlider.gameObject.SetActive(false);
    }

    private void HandleHideAllCautionSlider()
    {
        foreach( var btnCautionSlider in btnCautionSliders)
        {
            btnCautionSlider.gameObject.SetActive(false);
        }
    }
}
