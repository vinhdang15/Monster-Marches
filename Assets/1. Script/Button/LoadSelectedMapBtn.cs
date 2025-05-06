using System;


public class LoadSelectedMapBtn : BtnBase
{
    public event Action OnLoadMapBtnClick;

    protected override void LoadComponents()
    {
        MapBtnManager.Instance.loadSelectedMapBtnList.Add(this);
        base.LoadComponents();
    }

    protected override void OnButtonClick()
    {
        OnLoadMapBtnClick?.Invoke();
        base.OnButtonClick();
    }
}
