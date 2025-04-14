using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    
    [SerializeField] MapBtn mapBtnPrefeb;
    private Transform mapBtnParent;

    private List<MapModel> mapBtnList = new();
    private List<MapPresenter> mapPresenterList = new();

    private MapPresenter selectedMapPresenter;
    public List<LoadSelectedMapBtn> loadSelectedMapBtnList;

    private int starPoint;

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
        foreach(var loadSelectedMapBtn in loadSelectedMapBtnList)
        {
            loadSelectedMapBtn.OnLoadMapBtnClick += HandleLoadSelectedMap;
        }
    }

    public void InitMapBtn()
    {
        foreach(MapData mapFullData in MapDataReader.Instance.mapDataListSO.mapDataList)
        {
            Vector2 initPos = mapFullData.initMapBtnPos;
            MapBtn mapBtn = Instantiate(mapBtnPrefeb,initPos, quaternion.identity, mapBtnParent);
            MapModel mapMode = MapModel.Create(mapBtn,mapFullData);
            MapPresenter mapPresenter = MapPresenter.Create(mapMode,mapBtn);

            mapPresenter.RegisterMapBtnEventClick();
            mapPresenter.OnMapBtnClickHanlder += HandleOnMapBtnClick;

            mapBtn.name = mapFullData.mapName;

            mapBtnList.Add(mapMode);
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

    private void HideMapBtn()
    {
        foreach(var mapPresenter in mapPresenterList)
        {
            mapPresenter.gameObject.SetActive(false);
        }
    }

    public void ShowMapBtn()
    {
        foreach(var mapPresenter in mapPresenterList)
        {
            if(mapPresenter.mapModel.Activate)
            {
                mapPresenter.gameObject.SetActive(true);
            }
            
        }
    }

    public void SetCurrentMapStarPoint(int starPoint)
    {
        this.starPoint = starPoint;
    }

    public void UpdateMapPresenterInfo()
    {
        MapDataReader.Instance.UpdateMapProgressDataList(selectedMapPresenter, starPoint);
        UpdateSelectedMapPresenterInfo(starPoint);
        UpdateNextMapPresenterInfo();
    }

    private void UpdateSelectedMapPresenterInfo(int mapStars)
    {
        selectedMapPresenter.UpdateMapModel(mapStars);
        selectedMapPresenter.UpdateMapBtnImage();
    }

    private void UpdateNextMapPresenterInfo()
    {
        int mapID = selectedMapPresenter.mapModel.MapID;
        int nextMapPresenterMapID = mapID + 1;

        foreach(MapPresenter mapPresenter in mapPresenterList)
        {
            if(mapPresenter.mapModel.MapID == nextMapPresenterMapID)
            {
                mapPresenter.mapModel.Activate = true;
                mapPresenter.gameObject.SetActive(true);
            }
        }

    }
}