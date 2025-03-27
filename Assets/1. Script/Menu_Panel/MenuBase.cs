using UnityEngine;
using DG.Tweening;

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

    public virtual void Show()
    {
        gameObject.SetActive(true);
        rectTransform.anchoredPosition = new Vector2(0f, 1000f);
        rectTransform.DOAnchorPos(new Vector2(0f, 50f), timeDelay, false).SetEase(Ease.OutBack,0.5f).SetUpdate(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
