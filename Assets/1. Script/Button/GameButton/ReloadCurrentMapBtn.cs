public class ReloadCurrentMapBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.HandleReloadCurrentMap();
        base.OnButtonClick();
    }
}
