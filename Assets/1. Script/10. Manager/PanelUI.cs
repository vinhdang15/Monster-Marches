using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PanelUI : MonoBehaviour
{
    [SerializeField] float timeDelay = 1f;
    [SerializeField]CanvasGroup canvasGroup;
    [SerializeField]RectTransform rectTransform;

    private void Awake()
    {
        LoadComponents();
    }

    private void Start()
    {

    }

    private void LoadComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = new Vector2(0f, 1000f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), timeDelay, false).SetEase(Ease.OutBack,0.5f).SetUpdate(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
