public class CreateTowerDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreator.Instance.CreateTowerDataJson();
        base.OnButtonClick();
    }
}
