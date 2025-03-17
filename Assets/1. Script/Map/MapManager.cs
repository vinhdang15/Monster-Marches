using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public MapManager Instance { get; private set; }
    
    [SerializeField] MapMenu mapMenu;
    [SerializeField] MapBtn mapBtnPrefeb;
    private Transform mapParent;

    private List<MapModel> mapBtnList = new();
    private List<MapPresenter> mapPresenterList = new();

    private MapPresenter SelectedMapPresenter;

    [Header("Audio")]
    [SerializeField] SoundEffectSO soundEffectSO;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            mapMenu = FindObjectOfType<MapMenu>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PrepareGame()
    {
        GetMapParent();
    }

    private void GetMapParent()
    {
        // mapParent = GameObject.Find(InitNameObject.CanvasWorldSpace.ToString()).transform;
        mapParent = CanvasManager.Instance.transform.Find("CanvasWorldSpace");
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
        // mapMenu.gameObject.SetActive(true);
        // mapMenu.Show(mapPresenter.mapModel);
    }


}
