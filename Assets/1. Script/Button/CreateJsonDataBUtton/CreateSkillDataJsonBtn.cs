public class CreateSkillDataBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        JSONCreator.Instance.CreateSkillDataJson();
        base.OnButtonClick();
    }
}
