using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEmptyPlotDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateEmptyPlotDataJson();
        base.OnButtonClick();
    }
}
