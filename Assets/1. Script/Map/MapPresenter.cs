using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapPresenter : MonoBehaviour
{
    public MapBtn mapBtn;
    public MapModel mapModel;
    public event Action<MapPresenter> OnMapBtnClickHanlder;

    public static MapPresenter Create(MapModel mapMode, MapBtn mapBtn)
    {
        MapPresenter mapPresenter = mapBtn.gameObject.AddComponent<MapPresenter>();
        mapPresenter.MapPresenterInit(mapMode, mapBtn);
        return mapPresenter;
    }

    private void MapPresenterInit(MapModel mapMode, MapBtn mapBtn)
    {
        this.mapModel = mapMode;
        this.mapBtn = mapBtn;
    }

    public void RegisterMapBtnEventClick()
    {
        mapBtn.OnMapBtnClick += HandleMapBtnClick;
    } 

    private void HandleMapBtnClick(Button button)
    {
        OnMapBtnClickHanlder?.Invoke(this);
    }

    public void UpdateMapModel(int starPoint)
    {
        if(mapModel.StarPoint < starPoint)
        {
            mapModel.StarPoint = starPoint;
        }
    }

    public void UpdateMapBtnImage()
    {
        Sequence sequence = DOTween.Sequence();
        mapBtn.UpdateMapBtnImage(mapModel);
    }
}
