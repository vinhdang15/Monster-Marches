using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitCurrentMapBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        CanvasManager.Instance.HandleQuitCurrentMapBtnClick();
        base.OnButtonClick();
    }
}
