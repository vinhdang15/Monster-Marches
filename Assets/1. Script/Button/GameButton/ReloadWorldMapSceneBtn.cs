public class ReloadWorldMapSceneBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.HandleFinishMap();
        base.OnButtonClick();
    }
}
