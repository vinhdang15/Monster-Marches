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
        JSONCreator.Instance.CreateMapProgressDataJson();
        base.OnButtonClick();
    }
}
