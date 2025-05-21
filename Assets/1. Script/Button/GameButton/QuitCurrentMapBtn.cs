public class QuitCurrentMapBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.HandleQuitCurrentMap();
        base.OnButtonClick();
    }
}
