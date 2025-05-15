public class CreateMapObjDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreator.Instance.CreateMapObjDataJson();
        base.OnButtonClick();
    }
}
