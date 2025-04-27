using UnityEngine;

public class ShowSaveGameMenuBtn : BtnBase
{
    [SerializeField] CanvasManager canvasManager;
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        canvasManager.ShowSaveGameMenuHandler();
        base.OnButtonClick();
    }
}
