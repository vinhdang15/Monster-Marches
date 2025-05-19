public class InstructionNextBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        PlayClickSound();
        PanelManager.Instance.InstructionNextBtnClick();
    }
}
