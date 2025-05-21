using UnityEngine;
using DG.Tweening;
using System;

public class MenuBase : MonoBehaviour
{
    [SerializeField] float timeDelay = 0.25f;

    [SerializeField]RectTransform rectTransform;

    private void Awake()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Show( Action action = null)
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = new Vector2(0f, 1500f);
        Sequence seqState1 = DOTween.Sequence();
        seqState1.Append(rectTransform.DOAnchorPos(new Vector2(0f, 50f), timeDelay, false).SetEase(Ease.OutBack, 0.5f).SetUpdate(true));
        seqState1.OnComplete(() =>
            {
                action?.Invoke();
            }
        );
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
