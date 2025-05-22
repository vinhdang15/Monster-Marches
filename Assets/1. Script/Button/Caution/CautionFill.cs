using UnityEngine;
using System;
using DG.Tweening;
using Image = UnityEngine.UI.Image;

public class CautionFill : MonoBehaviour
{
    [SerializeField] private float targetScale = 0.8f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private Image fill;
    [SerializeField] private float fillDuration;
    private Tween scaleTween;
    private Tween fillTween;

    private void OnDisable()
    {
        ResetState();
    }

    public void ResetState()
    {
        if (scaleTween != null && scaleTween.IsActive()) scaleTween.Kill();
        if (fillTween != null && fillTween.IsActive()) fillTween.Kill();

        scaleTween = null;
        fillTween = null;
    }

    public void SetFillCompleteState()
    {
        fill.fillAmount = 1;
    }

    public void SetFillDuration(float time)
    {
        fillDuration = time;
    }

    public void StartCautionFillScalingTween()
    {
        transform.DOScale(1, 0f);
        gameObject.SetActive(true);
        scaleTween = transform.DOScale(targetScale, scaleDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StartFillImageTween()
    {
        fill.fillAmount = 0;
        fillTween = fill.DoFillAmount(1f, fillDuration).SetEase(Ease.InOutQuad);
    }
}
