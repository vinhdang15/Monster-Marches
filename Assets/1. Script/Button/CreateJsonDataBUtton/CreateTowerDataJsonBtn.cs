public class CreateTowerDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateTowerDataJson();
        base.OnButtonClick();
    }
}
