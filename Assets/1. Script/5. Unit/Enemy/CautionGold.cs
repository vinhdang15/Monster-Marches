using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CautionGold : MonoBehaviour
{
    [SerializeField] private RectTransform goldImageRect;
    [SerializeField] private RectTransform goldTextRect;
    [SerializeField] private float endPosY = 0.4f;
    [SerializeField] private float initTextScale = 0.4f;
    [SerializeField] private float popUpDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.3f;
    [SerializeField] private Image goldImage;
    private Vector2 goldImageStartPos;
    private Vector2 goldImageEndPos;
    private TextMeshProUGUI cautionGoldText;

    private void OnDisable()
    {
        ResetState();
    }

    public void CautionGoldPrepare()
    {
        SetGoldImageStartPos();
        LoadComponents();
        SetEndPos();
        ResetState();
    }
    private void SetGoldImageStartPos()
    {
        goldImageEndPos = goldImageRect.anchoredPosition;
    }

    private void ResetState()
    {
        goldImageRect.anchoredPosition = goldImageStartPos;
        goldTextRect.localScale = Vector2.one * initTextScale;
        goldImageRect.DOScaleX(0.5f,0);
        goldImage.DOFade(1,0);
        cautionGoldText.DOFade(1,0);
    }

    private void LoadComponents()
    {
        goldImage = goldImageRect.GetComponent<Image>();
        cautionGoldText = goldTextRect.GetComponent<TextMeshProUGUI>();
    }

    private void SetEndPos()
    {
        goldImageStartPos =  new Vector2(goldImageEndPos.x, goldImageEndPos.y - endPosY);
    }

    private void SetCautionGoldText(int gold)
    {
        cautionGoldText.text = "+" + gold.ToString();
    }

    public void StarTween(int gold)
    {
        gameObject.SetActive(true);
        SetCautionGoldText(gold);
        StartCautionGoldTween();
    }

    private void StartCautionGoldTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(goldImageRect.DOAnchorPos(goldImageEndPos, popUpDuration).SetEase(Ease.OutQuad));
        seq.Join(goldImageRect.DOScaleX(1,popUpDuration).SetEase(Ease.OutQuad));
        seq.Join(goldTextRect.DOScale(Vector2.one, popUpDuration).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.5f);
        seq.Append(goldImage.DOFade(0, fadeOutDuration).SetEase(Ease.OutQuad));
        seq.Join(cautionGoldText.DOFade(0, fadeOutDuration).SetEase(Ease.OutQuad));
        seq.OnComplete(() =>
        {
            ResetState();
            gameObject.SetActive(false);
        });
    }
}
