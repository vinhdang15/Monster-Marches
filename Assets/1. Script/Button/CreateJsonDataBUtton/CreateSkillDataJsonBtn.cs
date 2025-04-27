public class CreateSkillDataBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreater.Instance.CreateSkillDataJson();
        base.OnButtonClick();
    }
}
