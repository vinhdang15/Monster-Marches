using UnityEngine;

public class ReloadWorldMapSceneBtn : BtnBase
{
    [SerializeField] CanvasManager canvasManager;
    protected override void Start() 
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        canvasManager.HandleReLoadWorldMapBtnClick();
        base.OnButtonClick();
    }
}
