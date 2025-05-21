public class BtnPauseGame : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GamePlayUIManager.Instance.ShowPauseMenu();
        base.OnButtonClick();
    }
}
