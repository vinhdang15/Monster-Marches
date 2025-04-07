public class CreateBulletEffectDataJsonBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateBulletEffectDataJson();
        base.OnButtonClick();
    }
}
