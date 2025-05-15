public class CreateBulletEffectDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreator.Instance.CreateBulletEffectDataJson();
        base.OnButtonClick();
    }
}
