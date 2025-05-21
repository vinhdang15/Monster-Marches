
public class ReloadIntroSceneBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.HandleReloadIntroScene();
        base.OnButtonClick();
    }
}
