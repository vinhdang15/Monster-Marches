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
    public virtual void ShowInPos(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}
