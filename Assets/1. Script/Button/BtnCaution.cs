using UnityEngine;
using System;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class BtnCaution : BtnBase
{
    [SerializeField] private CautionFill cautionFill;
    [SerializeField] private CautionGold cautionGold;
    public bool isFirstWave = false;
    private EnemySpawnerManager   enemySpawnerManager;

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

    public void SetSpawnEnemyManager(EnemySpawnerManager spawnEnemyManager)
    {
        this.enemySpawnerManager = spawnEnemyManager;
        this.enemySpawnerManager.OnAddGoldWhenCautionClick += HandleShowGoldAddWhenCautionClick;
        this.enemySpawnerManager.OnUpdateTimeWaitForNextWave += HandleOnUpdateTimeWaitForNextWave;
        // float fillDuration = this.spawnEnemyManager.GetTimeWaitForNextWave();
        // cautionFill.SetFillDuration(fillDuration);
    }

    protected override void OnButtonClick()
    {
        PlayCustomSound(soundEffectSO.comingWaveSound);
        enemySpawnerManager.HandleCautionButtonClicked();
    }

    public bool IsCautionFillActive()
    {
        return cautionFill.gameObject.activeSelf;
    }

    // Show gold add when caution click 
    private void HandleShowGoldAddWhenCautionClick(int gold)
    {
        if(gold == 0 || !IsCautionFillActive()) return;
        {
            PlayCustomSound(soundEffectSO.AddGoldSound);
            cautionGold.StarTween(gold);
        }
    }

    // Update caution sliding time
    private void HandleOnUpdateTimeWaitForNextWave(float timeWaitForNextWave)
    {
        // Debug.Log(timeWaitForNextWave);
        cautionFill.SetFillDuration(timeWaitForNextWave);
    }

    private void OnDestroy()
    {
        if (enemySpawnerManager != null)
        {
            enemySpawnerManager.OnAddGoldWhenCautionClick -= HandleShowGoldAddWhenCautionClick;
        }
    }

    public void Cleanup()
    {
        base.CleanupBtnListeners();
    }
}
