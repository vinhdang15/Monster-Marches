public class CreateMapDesignDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateMapDesignDataJson();
        base.OnButtonClick();
    }
}
