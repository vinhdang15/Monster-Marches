public class InstructionNextBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GamePlayUIManager.Instance.InstructionNextBtnClick();
        base.OnButtonClick();
    }
}
