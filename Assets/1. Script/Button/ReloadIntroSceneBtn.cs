using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadIntroSceneBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        CanvasManager.Instance.HandleReloadIntroScene();
    }
}
