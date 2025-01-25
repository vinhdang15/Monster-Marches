using UnityEngine;
using System;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class BtnCautionSlider : BtnBase
{
    [SerializeField] private float targetScale = 0.8f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] Image cautionFill;
    [SerializeField] public float fillDuration;
    private SpawnEnemyManager   spawnEnemyManager;
    public event Action         OnCautionClick;
    private void OnEnable()
    {
        StartScaling();
    }

    private void OnDisable()
    {
        StopScaling();
    }
    protected override void Start()
    {
        base.Start();
    }

    public void GetSpawnEnemyManager(SpawnEnemyManager _spawnEnemyManager)
    {
        spawnEnemyManager = _spawnEnemyManager;
        fillDuration = spawnEnemyManager.GetTimeWaitForNextWave();
    }

    protected override void OnButtonClick()
    {
        PlayClickSound();
        spawnEnemyManager.CautionClick();
        OnCautionClick?.Invoke();
    }

    protected override void PlayClickSound()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.comingWaveSound);
    }

    private void StartScaling()
    {
        transform.DOScale(targetScale, scaleDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopScaling()
    {
        transform.DOKill();
        transform.DOScale(1, 0f);
    }

    public void StartFillCautionImage()
    {
        cautionFill.fillAmount = 0;
        cautionFill.DOFillAmount(1f, fillDuration).SetEase(Ease.InOutQuad);
    }

}
