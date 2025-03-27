public class CreateMapDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateMapDataJson();
        base.OnButtonClick();
    }
}
