public class CreateUnitDataBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateUnitDataJson();
        base.OnButtonClick();
    }
}
