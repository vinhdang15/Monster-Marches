public class CreateSkillDataBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JsonCreater.Instance.CreateSkillDataJson();
        base.OnButtonClick();
    }
}
