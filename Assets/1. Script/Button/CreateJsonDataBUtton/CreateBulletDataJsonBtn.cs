using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBulletDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateBulletDataJson();
        base.OnButtonClick();
    }
}
