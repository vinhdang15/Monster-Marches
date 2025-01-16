using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnPauseGame : BtnBase
{
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        PlayClickSound();

        PanelManager.Instance.ShowPauseMenu();
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

}
