using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapBtnManager : MonoBehaviour
{
    public static MapBtnManager Instance { get; private set; }

    [SerializeField] MapBtn mapBtnPrefeb;
    private Transform mapBtnParent;
    private List<MapPresenter> mapPresenterList = new();

    private MapPresenter selectedMapPresenter;
    public List<LoadSelectedMapBtn> loadSelectedMapBtnList;

    private int currentMapStarPoint;

    public event Action<MapPresenter> OnLoadSelectedMap;

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PrepareGame()
    {
        GetMapParent();
        RegisterButtonEvent();
    }

    private void GetMapParent()
    {
        mapBtnParent = CanvasManager.Instance.transform.Find("CanvasWorldSpace");
    }

    private void RegisterButtonEvent()
    {
        foreach (var loadSelectedMapBtn in loadSelectedMapBtnList)
        {
            loadSelectedMapBtn.OnLoadMapBtnClick += HandleLoadSelectedMap;
        }
    }

    public void InitMapBtn()
    {
        foreach (MapData mapFullData in MapDataReader.Instance.mapDataListSO.mapDataList)
        {
            Vector2 initPos = mapFullData.initMapBtnPos;
            MapBtn mapBtn = Instantiate(mapBtnPrefeb, initPos, quaternion.identity, mapBtnParent);
            MapModel mapMode = MapModel.Create(mapBtn, mapFullData);
            MapPresenter mapPresenter = MapPresenter.Create(mapMode, mapBtn);

            mapPresenter.SetDefaultTurnOnStar(mapMode.StarScore);

            mapPresenter.RegisterMapBtnEventClick();
            mapPresenter.OnMapBtnClickHanlder += HandleOnMapBtnClick;

            mapBtn.name = mapFullData.mapName;

            mapPresenterList.Add(mapPresenter);

            mapBtn.PrepareGame();
            mapBtn.UpdateMapInfo(mapMode);
        }
    }

    public void HandleOnMapBtnClick(MapPresenter mapPresenter)
    {
        selectedMapPresenter = mapPresenter;
        PanelManager.Instance.ShowMapMenu(mapPresenter.mapModel);
    }

    private void HandleLoadSelectedMap()
    {
        HideMapBtn();
        PanelManager.Instance.HideMapMenu();
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

    public void UpdateMapPresenterInfo()
    {
        // Update progress save-data
        MapProgressManager.UpdateProgress(selectedMapPresenter.mapModel.MapID, currentMapStarPoint);

        // Update mapBtn UI
        UpdateSelectedMapPresenterInfo(currentMapStarPoint);
        UpdateNextMapPresenterInfo();

        // Update progress runtime-data
        MapDataReader.Instance.ResetFullMapData();
    }

    private void UpdateSelectedMapPresenterInfo(int starSocre)
    {
        if (starSocre != -0) selectedMapPresenter.ShowStarBackGround();

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