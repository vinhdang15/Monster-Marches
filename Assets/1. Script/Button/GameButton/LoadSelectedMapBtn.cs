using System;


public class LoadSelectedMapBtn : BtnBase
{
    public MapManager mapManager;
    public event Action OnLoadMapBtnClick;

    protected override void OnButtonClick()
    {
        OnLoadMapBtnClick?.Invoke();
        base.OnButtonClick();
    }
}
