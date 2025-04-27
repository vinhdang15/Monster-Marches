public class CreateBulletEffectDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateBulletEffectDataJson();
        base.OnButtonClick();
    }
}
