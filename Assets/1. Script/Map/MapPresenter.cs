using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
