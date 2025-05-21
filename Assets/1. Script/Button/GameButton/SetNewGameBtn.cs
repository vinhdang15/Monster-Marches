public class SetNewGameBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.HandleSetNewGameBtnClick();
        base.OnButtonClick();
    }
}
