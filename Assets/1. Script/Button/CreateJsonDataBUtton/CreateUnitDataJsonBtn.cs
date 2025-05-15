public class CreateUnitDataBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreator.Instance.CreateUnitDataJson();
        base.OnButtonClick();
    }
}
