using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SetNewGameBtn : BtnBase
{
    [SerializeField] CanvasManager canvasManager;
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        CanvasManager.Instance.HandleSetNewGameBtnCLcik();
        base.OnButtonClick();
    }
}
