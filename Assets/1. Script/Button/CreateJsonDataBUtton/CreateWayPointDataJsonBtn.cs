using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWayPointDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreator.Instance.CreateWayPointDataJson();
        base.OnButtonClick();
    }
}
