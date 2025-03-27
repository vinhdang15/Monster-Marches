using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIElementBase : MonoBehaviour
{
    [SerializeField] protected float timeDelay = 1f;
    [SerializeField]CanvasGroup canvasGroup;
    [SerializeField]RectTransform rectTransform;

    // private void Awake()
    // {
    //     LoadComponents();
    // }

    // private void LoadComponents()
    // {
    //     canvasGroup = GetComponent<CanvasGroup>();
    //     rectTransform = GetComponent<RectTransform>();
    // }

    public virtual void Show()
    {
        transform.DOScale(1, 0f);
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        transform.DOScale(0, 0).SetEase(Ease.OutBounce);
        gameObject.SetActive(false);
    }

    public virtual void ShowInPos(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
        transform.DOScale(1, timeDelay);
    }
}
