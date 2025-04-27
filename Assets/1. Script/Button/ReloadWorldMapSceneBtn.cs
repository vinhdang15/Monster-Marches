using UnityEngine;

public class ReloadWorldMapSceneBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        CanvasManager.Instance.HandleFinishGamepBtnClick();
        base.OnButtonClick();
    }
}
