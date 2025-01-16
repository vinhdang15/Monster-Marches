using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementBase : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public virtual void CheckAndShowInPos(Vector2 pos, int currentGold)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
