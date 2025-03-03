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

    private void OnDisable()
    {
        ResetState();
    }

    public void ResetState()
    {
        transform.DOKill();
        fill.DOKill();
        transform.DOScale(1, 0f);
        fill.fillAmount = 0;
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
        gameObject.SetActive(true);
        transform.DOScale(targetScale, scaleDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void StartFillImageTween()
    {
        fill.DoFillAmount(1f, fillDuration).SetEase(Ease.InOutQuad);
    }

    
}
