using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] RectTransform shieldRect;
    [SerializeField] RectTransform victoryRibbonRect;
    [SerializeField] StarMovementController starMovementController;
    [SerializeField] RectTransform reloadBtnRect;
    [SerializeField] RectTransform restartBtnRect;
    private RectTransform rectTransform;
    private Vector2 initPos = new Vector2(0, 170);
    private Vector2 reloadBtnPos = new Vector2(0, -230);
    private Vector2 reStartBtnEndPos = new Vector2(0, -470);
    private float duration = 0.5f;
    private int starScore;
    public int StarScore() =>  starScore;

    public void PrepareGame()
    {
        LoadComponents();
        starMovementController.PrepareGame();
        ResetState();
    }

    private void LoadComponents()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ResetState()
    {
        rectTransform.anchoredPosition = initPos;
        shieldRect.anchoredPosition = new Vector2(0, 1000);

        victoryRibbonRect.localScale = new Vector2(0.8f, 0.8f);
        victoryRibbonRect.anchoredPosition = new Vector2(0, 1000);

        starMovementController.SetAnchorPos(new Vector2(0, 1000));
        starMovementController.StarControllerResetState();

        reloadBtnRect.anchoredPosition = new Vector2(0, 770);
        restartBtnRect.anchoredPosition = new Vector2(0, 530);
    }

    public void SetStarScore(int starScore)
    {
        this.starScore = starScore;
        starMovementController.GetStarScore(StarScore());
    }

    public void StartVictoryMenu()
    {
        gameObject.SetActive(true);
        Sequence seqState1 = DOTween.Sequence();
        seqState1.AppendInterval(3f).SetUpdate(true);
        seqState1.Append(shieldRect.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutBack, 1f));
        seqState1.AppendInterval(duration / 2).SetUpdate(true);

        // start VictoryRibbonRect Tween
        seqState1.Append(victoryRibbonRect.DOAnchorPos(new Vector2(0, 100), 0));
        seqState1.Join(victoryRibbonRect.DOAnchorPos(Vector2.zero, duration / 4).SetEase(Ease.OutBack, 1f));
        seqState1.Join(victoryRibbonRect.DOScale(1, duration / 6));
        seqState1.AppendInterval(0.5f);


        seqState1.OnComplete(() =>
        {
            starMovementController.SetAnchorPos(Vector2.zero);
            starMovementController.ActiveStars(0.3f);
            Sequence seqState2 = DOTween.Sequence();
            seqState2.AppendInterval(0.3f).SetUpdate(true);
            seqState2.OnComplete(() =>
            {
                ButtonsTween();
            });
        });
    }

    private void ButtonsTween()
    {
        reloadBtnRect.anchoredPosition = Vector2.zero;
        restartBtnRect.anchoredPosition = Vector2.zero;
        ButtonsSequence();
    }

    private void ButtonsSequence()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(0f).SetUpdate(true);
        seq.Append(reloadBtnRect.DOAnchorPos(reloadBtnPos, duration / 3));
        seq.Append(restartBtnRect.DOAnchorPos(reStartBtnEndPos, duration / 3));
        seq.OnComplete(() => PauseGame());
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }
} 
