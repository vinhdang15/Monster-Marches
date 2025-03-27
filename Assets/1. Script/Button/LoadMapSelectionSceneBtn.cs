using UnityEngine;

public class LoadMapSelectionSceneBtn : BtnBase
{
    [SerializeField] CanvasManager canvasManager;
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        canvasManager.HandleLoadMapSelectionClick();
        base.OnButtonClick();
    }
}
