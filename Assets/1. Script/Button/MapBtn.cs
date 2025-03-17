using System;
using UnityEngine;
using UnityEngine.UI;

public class MapBtn : BtnBase
{
    public event Action<Button> OnMapBtnClick;
    protected override void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => OnMapBtnClick?.Invoke(thisButton));
    }
}
