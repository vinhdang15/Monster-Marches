using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    
    [SerializeField] MapBtn mapBtnPrefeb;
    private Transform mapParent;

    private List<MapModel> mapBtnList = new();
    private List<MapPresenter> mapPresenterList = new();

    private MapPresenter SelectedMapPresenter;
    public List<LoadSelectedMapBtn> loadSelectedMapBtnList;

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
        mapParent = CanvasManager.Instance.transform.Find("CanvasWorldSpace");
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
        foreach(MapData mapData in MapDataReader.Instance.mapDataListSO.mapDataList)
        {
            Vector2 initPos = mapData.initPos.ToVector2();
            MapBtn mapBtn = Instantiate(mapBtnPrefeb,initPos, quaternion.identity, mapParent);
            MapModel mapMode = MapModel.Create(mapBtn,mapData);
            MapPresenter mapPresenter = MapPresenter.Create(mapMode,mapBtn);

            mapPresenter.RegisterMapBtnEventClick();
            mapPresenter.OnMapBtnClickHanlder += HandleOnMapBtnClick;

            mapBtn.name = mapData.mapName;
            //if(!mapData.active) mapBtn.GetComponent<Button>().enabled = false;

            mapBtnList.Add(mapMode);
            mapPresenterList.Add(mapPresenter);
        }
    }

    public void HandleOnMapBtnClick(MapPresenter mapPresenter)
    {
        SelectedMapPresenter = mapPresenter;
        PanelManager.Instance.ShowMapMenu(mapPresenter.mapModel);
    }

    private void HandleLoadSelectedMap()
    {
        HideMapBtn();
        PanelManager.Instance.HideMapMenu();
        OnLoadSelectedMap?.Invoke(SelectedMapPresenter);
    }

    private void HideMapBtn()
    {
        foreach(var mapBtn in mapBtnList)
        {
            mapBtn.gameObject.SetActive(false);
        }
    }

    public void ShowMapBtn()
    {
        foreach(var mapBtn in mapBtnList)
        {
            mapBtn.gameObject.SetActive(true);
        }
    }
}
