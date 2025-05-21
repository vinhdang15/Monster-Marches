using UnityEngine;

public class ShowSaveGameMenuBtn : BtnBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        GameFlowManager.Instance.ShowSaveGameMenu();
        base.OnButtonClick();
    }
}