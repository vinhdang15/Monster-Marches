using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] MapViewBtn mapViewBtnPrefeb;
    private MapDataReader mapDataReader;
    private Transform mapViewBtnParent;
    private List<MapPresenter> mapPresenterList = new();

    private MapPresenter selectedMapPresenter;
    private LoadSelectedMapBtn loadSelectedMapBtn;

    private int currentMapStarPoint;

    public event Action<MapPresenter> OnLoadSelectedMap;

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;

    public void PrepareGame(MapDataReader mapDataReader)
    {
        GetComponents(mapDataReader);
        GetMapParent();
        RegisterButtonEvent();
    }

    private void GetComponents(MapDataReader mapDataReader)
    {
        this.mapDataReader = mapDataReader;
        loadSelectedMapBtn = FindObjectOfType<LoadSelectedMapBtn>();
    }

    private void GetMapParent()
    {
        mapViewBtnParent = FindObjectOfType<ScreenUIManager>().transform.Find("CanvasWorldSpace");
    }

    private void RegisterButtonEvent()
    {
        loadSelectedMapBtn.OnLoadMapBtnClick += HandleLoadSelectedMap;
    }

    public void InitMapBtn()
    {
        foreach (MapData mapFullData in mapDataReader.mapDataListSO.mapDataList)
        {
            Vector2 initPos = mapFullData.initMapBtnPos;
            MapViewBtn mapViewBtn = Instantiate(mapViewBtnPrefeb, initPos, quaternion.identity, mapViewBtnParent);
            MapModel mapMode = MapModel.Create(mapViewBtn, mapFullData);
            MapPresenter mapPresenter = MapPresenter.Create(mapMode, mapViewBtn);

            mapPresenter.SetDefaultTurnOnStar(mapMode.StarScore);

            mapPresenter.RegisterMapBtnEventClick();
            mapPresenter.OnMapBtnClickHanlder += HandleOnMapBtnClick;

            mapViewBtn.name = mapFullData.mapName;

            mapPresenterList.Add(mapPresenter);

            mapViewBtn.PrepareGame();
            mapViewBtn.UpdateMapInfo(mapMode);
        }
    }

    public void HandleOnMapBtnClick(MapPresenter mapPresenter)
    {
        selectedMapPresenter = mapPresenter;
        GamePlayUIManager.Instance.ShowMapMenu(mapPresenter.mapModel);
    }

    private void HandleLoadSelectedMap()
    {
        HideMapBtn();
        GamePlayUIManager.Instance.HideMapMenu();
        OnLoadSelectedMap?.Invoke(selectedMapPresenter);
    }

    public void HideMapBtn()
    {
        foreach (var mapPresenter in mapPresenterList)
        {
            mapPresenter.gameObject.SetActive(false);
        }
    }

    public void CLearAllMapBtn()
    {
        foreach (var mapPresenter in mapPresenterList)
        {
            Destroy(mapPresenter.gameObject);
        }
        mapPresenterList.Clear();
    }

    public void ShowMapBtn()
    {
        foreach (var mapPresenter in mapPresenterList)
        {
            if (mapPresenter.mapModel.Activate)
            {
                mapPresenter.gameObject.SetActive(true);
            }
        }
    }

    public void SetCurrentMapStarPoint(int starPoint)
    {
        this.currentMapStarPoint = starPoint;
    }

    public void UpdateMapDataJson()
    {
        // Update progress save-data
        MapProgressManager.UpdateProgress(selectedMapPresenter.mapModel.MapID, currentMapStarPoint);

        // Update progress runtime-data
        mapDataReader.ResetFullMapData();
    }

    public void UpdateMapPresenterInfo()
    {
        // Update mapBtn UI
        UpdateSelectedMapPresenterInfo(currentMapStarPoint);
        UpdateNextMapPresenterInfo();
    }

    private void UpdateSelectedMapPresenterInfo(int starSocre)
    {
        if (starSocre != 0) selectedMapPresenter.ShowStarBackGround();

        selectedMapPresenter.LightUpMapStar(selectedMapPresenter.mapModel.StarScore, starSocre);
        selectedMapPresenter.UpdateMapModel(starSocre);
        selectedMapPresenter.UpdateMapBtnImage();
    }

    private void UpdateNextMapPresenterInfo()
    {
        int mapID = selectedMapPresenter.mapModel.MapID;
        int nextMapPresenterMapID = mapID + 1;

        foreach (MapPresenter mapPresenter in mapPresenterList)
        {
            if (selectedMapPresenter.mapModel.StarScore == 0) return;
            if (mapPresenter.mapModel.MapID == nextMapPresenterMapID)
            {
                mapPresenter.mapModel.Activate = true;
                mapPresenter.gameObject.SetActive(true);
            }
        }
    }

    public bool HasActiveMapID2()
    {
        foreach (MapPresenter mapPresenter in mapPresenterList)
        {
            if (mapPresenter.mapModel.MapID == 2)
            {
                if (mapPresenter.mapModel.Activate == true) return true;
            }
        }
            return false;
    }
}