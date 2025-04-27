using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadWorldMapSceneBtn : BtnBase
{
    [SerializeField] CanvasManager canvasManager;

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        canvasManager.HandleLoadWorldMapBtnClick();
        base.OnButtonClick();
    }
}