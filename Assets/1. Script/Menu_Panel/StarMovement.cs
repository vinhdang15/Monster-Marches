using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarMovement : MonoBehaviour
{
    // DoTween
    // reset state
    [SerializeField] RectTransform rectTransform;
    private Vector2 initPos = new Vector2(0, -115);

    public void StarMovementPrepareGame()
    {
        LoadComponents();
        ResetState();
    }
    private void LoadComponents()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ResetState()
    {
        rectTransform.anchoredPosition = initPos;
        rectTransform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void StarMove(Vector2 endPos, float duration)
    {
        gameObject.SetActive(true);
        rectTransform.DOScale(Vector3.one, duration);
        rectTransform.DOAnchorPos(endPos, duration);
    }
}
