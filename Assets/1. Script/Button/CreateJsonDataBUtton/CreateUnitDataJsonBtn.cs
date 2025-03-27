public class CreateUnitDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateUnitDataJson();
        base.OnButtonClick();
    }
}
