public class CreateMapDesignDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateMapDesignDataJson();
        base.OnButtonClick();
    }
}
