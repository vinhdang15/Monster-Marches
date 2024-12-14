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

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPauseMenu();
            UIManager.Instance.PauseGame();
        }
        else
        {
            Debug.LogError("UIManager.Instance is null. Make sure it is initialized.");
        }
    }
}
