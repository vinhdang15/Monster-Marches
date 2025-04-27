public class CreateTowerDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateTowerDataJson();
        base.OnButtonClick();
    }
}
