using UnityEngine;
using System;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class BtnCaution : BtnBase
{
    [SerializeField] private CautionFill cautionFill;
    [SerializeField] private CautionGold cautionGold;
    public bool isFirstWave = false;
    private EnemySpawnerManager   spawnEnemyManager;

    protected override void Start()
    {
        base.Start();
    }

    public void CautionBtnPrepareGame()
    {
        HideCautionFill();
        cautionGold.CautionGoldPrepare();
        HideCautionGoldAdd();
    }

    public void HideCautionFill()
    {
        cautionFill.gameObject.SetActive(false);
    }

    private void HideCautionGoldAdd()
    {
        cautionGold.gameObject.SetActive(false);
    }
    
    public void StartActiveCautionFill()
    {
        cautionFill.StartCautionFillScalingTween();
        if(isFirstWave)
        {
            cautionFill.SetFillCompleteState();
            isFirstWave = false;
        }
        else
        {
            cautionFill.StartFillImageTween();
        }
    }

    public void SetSpawnEnemyManager(EnemySpawnerManager _spawnEnemyManager)
    {
        spawnEnemyManager = _spawnEnemyManager;
        spawnEnemyManager.OnAddGoldWhenCautionClick += HandleShowGoldAddWhenCautionClick;
        float fillDuration = spawnEnemyManager.GetTimeWaitForNextWave();
        cautionFill.SetFillDuration(fillDuration);
    }

    protected override void OnButtonClick()
    {
        PlayCustomSound(soundEffectSO.comingWaveSound);
        spawnEnemyManager.CautionClick();
    }

    public bool isCautionFillActive()
    {
        return cautionFill.gameObject.activeSelf;
    }

    // show gold add when caution click 
    private void HandleShowGoldAddWhenCautionClick(int gold)
    {
        if(gold == 0 || !isCautionFillActive()) return;
        {
            PlayCustomSound(soundEffectSO.AddGoldSound);
            cautionGold.StarTween(gold);
        }
    }
}
