using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class LoadSelectedMapBtn : BtnBase
{
    public event Action OnLoadMapBtnClick;

    protected override void LoadComponents()
    {
        MapManager.Instance.loadSelectedMapBtnList.Add(this);
        base.LoadComponents();
    }

    protected override void OnButtonClick()
    {
        OnLoadMapBtnClick?.Invoke();
        base.OnButtonClick();
    }
}
