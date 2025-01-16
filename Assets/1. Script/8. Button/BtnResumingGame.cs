using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnResumingGame : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        PlayClickSound();
        PanelManager.Instance.HidePauseMenu();
        ResumingGame();
    }

    private void ResumingGame()
    {
        Time.timeScale = 1;
    }
}
