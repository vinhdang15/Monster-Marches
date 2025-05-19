public class InstructionSkipBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        PlayClickSound();
        InstructionMenu.Instance.HideInstruction();
    }
}
