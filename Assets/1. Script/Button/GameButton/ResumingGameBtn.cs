using UnityEngine;

public class ResumingGameBtn : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GamePlayUIManager.Instance.HidePauseMenu();
        ResumingGame();
        base.OnButtonClick();
    }

    private void ResumingGame()
    {
        Time.timeScale = 1;
    }
}
