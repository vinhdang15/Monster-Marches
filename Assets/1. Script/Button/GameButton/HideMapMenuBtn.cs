public class HideMapMenuBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GamePlayUIManager.Instance.HideMapMenu();
        base.OnButtonClick();
    }
}
