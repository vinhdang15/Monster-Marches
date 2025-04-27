using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMapProgressDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateMapProgressDataJson();
        base.OnButtonClick();
    }
}
