using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] RectTransform shieldRect;
    [SerializeField] RectTransform victoryRibbonRect;
    [SerializeField] StarController starController;
    [SerializeField] RectTransform reloadBtnRect;
    [SerializeField] RectTransform restartBtnRect;
    private Vector2 victoryRibbonRectEndPos;
    private RectTransform rectTransform;
    private Vector2 initPos = new Vector2(0, 170);
    private Vector2 reloadBtnPos = new Vector2(0,-230);
    private Vector2 reStartBtnEndPos = new Vector2(0,-470);
    private float duration = 0.5f;
    Sequence seqState2;

    private void Start()
    {
        VictoryMenuPrepareGame();
    }

    private void LoadComponents()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void ResetState()
    {
        rectTransform.anchoredPosition = initPos;
        shieldRect.anchoredPosition = new Vector2(0, 1000);

        victoryRibbonRect.localScale = new Vector2(0.8f, 0.8f);
        victoryRibbonRect.anchoredPosition = new Vector2(0, 1000);

        starController.SetAnchorPos(new Vector2(0, 1000));
        starController.StarControllerResetState();

        reloadBtnRect.anchoredPosition = new Vector2(0, 770);
        restartBtnRect.anchoredPosition = new Vector2(0, 530);
    }

    private void VictoryMenuPrepareGame()
    {
        LoadComponents();
        starController.StarControllerPrepareGame();
        ResetState();

        victoryRibbonRectEndPos = new Vector2(0, 100);
    }

    public void StartVictoryMenu()
    {   
        // StartCoroutine(ShowCoroutine());
        gameObject.SetActive(true);
        Sequence seqState1 = DOTween.Sequence();
        seqState1.AppendInterval(1f).SetUpdate(true);
        seqState1.Append(shieldRect.DOAnchorPos( Vector2.zero, duration).SetEase(Ease.OutBack,1f).SetUpdate(true));
        seqState1.AppendInterval(duration/2).SetUpdate(true);

        // start VictoryRibbonRect Tween
        seqState1.Append(victoryRibbonRect.DOAnchorPos(new Vector2(0, 100), 0).SetUpdate(true));
        seqState1.Join(victoryRibbonRect.DOAnchorPos( Vector2.zero, duration/4).SetEase(Ease.OutBack,1f).SetUpdate(true));
        seqState1.Join(victoryRibbonRect.DOScale(1, duration/6)).SetUpdate(true);
        seqState1.AppendInterval(0.5f).SetUpdate(true);
        

        seqState1.OnComplete(() =>
        {
            starController.SetAnchorPos(Vector2.zero);
            starController.ActiveStars(0.3f);
            seqState2 = DOTween.Sequence();
            seqState2.AppendInterval(0.3f).SetUpdate(true);
            seqState2.OnComplete(() =>
            {
                ButtonsTwwen();
            });
        });
        
    }

    private IEnumerator ShowCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        gameObject.SetActive(true);
        // ShieldRectTween();
        // yield return new WaitForSeconds(duration/2);
        // VictoryRibbonRectTween();
        // yield return new WaitForSeconds(0.5f);
        // starController.SetAnchorPos(Vector2.zero);
        // starController.ActiveStars(0.3f);
        // yield return new WaitForSeconds(0.3f);
        // ButtonsTwwen();

    }

    // private void ShieldRectTween()
    // {
    //     shieldRect.DOAnchorPos( Vector2.zero, duration).SetEase(Ease.OutBack,1f).SetUpdate(true);
    // }

    // private void VictoryRibbonRectTween()
    // {
    //     victoryRibbonRect.DOAnchorPos(new Vector2(0, 100),0).SetUpdate(true);
    //     victoryRibbonRect.DOAnchorPos( Vector2.zero, duration/4).SetEase(Ease.OutBack,1f).SetUpdate(true);
    //     victoryRibbonRect.DOScale(1, duration/6);
    // }

    private void ButtonsTwwen()
    {
        reloadBtnRect.anchoredPosition = Vector2.zero;
        restartBtnRect.anchoredPosition = Vector2.zero;
        ButtonsSequence();
    }

    private void ButtonsSequence()
    {
        var seq = DOTween.Sequence();
        seq.Append(reloadBtnRect.DOAnchorPos( reloadBtnPos, duration/3).SetUpdate(true));
        seq.Append(restartBtnRect.DOAnchorPos( reStartBtnEndPos, duration/3).SetUpdate(true));
    }

    public void SetStarScore(float lifePercentage)
    {
        starController.SetStarScore(lifePercentage);
    }
} 
