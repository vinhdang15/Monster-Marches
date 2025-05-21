using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapPresenter : MonoBehaviour
{
    public MapViewBtn mapView;
    public MapModel mapModel;
    public event Action<MapPresenter> OnMapBtnClickHanlder;

    public static MapPresenter Create(MapModel mapMode, MapViewBtn mapViewBtn)
    {
        MapPresenter mapPresenter = mapViewBtn.gameObject.AddComponent<MapPresenter>();
        mapPresenter.MapPresenterInit(mapMode, mapViewBtn);
        return mapPresenter;
    }

    private void MapPresenterInit(MapModel mapMode, MapViewBtn mapViewBtn)
    {
        this.mapModel = mapMode;
        this.mapView = mapViewBtn;
    }

    public void RegisterMapBtnEventClick()
    {
        mapView.OnMapBtnClick += HandleMapBtnClick;
    } 

    private void HandleMapBtnClick(Button button)
    {
        OnMapBtnClickHanlder?.Invoke(this);
    }

    public void UpdateMapModel(int starPoint)
    {
        if(mapModel.StarScore < starPoint)
        {
            mapModel.StarScore = starPoint;
        }
    }

    public void UpdateMapBtnImage()
    {
        Sequence sequence = DOTween.Sequence();
        mapView.UpdateMapBtnImage(mapModel);
    }

    public void ShowStarBackGround()
    {
        mapView.ShowStarBackGround();
    }

    public void LightUpMapStar(int currentStarScore, int starScore)
    {
        StartCoroutine(mapView.TurnOnMapStar(currentStarScore, starScore));
    }

    public void SetDefaultTurnOnStar(int currentStarScore)
    {
        mapView.SetDefaultTurnOnStar(currentStarScore);
    }
}
