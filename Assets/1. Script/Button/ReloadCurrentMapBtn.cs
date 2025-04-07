using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadCurrentMapBtn : BtnBase
{
    [SerializeField] CanvasManager canvasManager;
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        canvasManager.HandleReloadCurrentMapClick();
        base.OnButtonClick();
    }
}
