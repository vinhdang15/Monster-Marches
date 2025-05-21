public class LoadWorldMapSceneBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    
    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.HandleLoadWorldMapScene();
        base.OnButtonClick();
    }
}