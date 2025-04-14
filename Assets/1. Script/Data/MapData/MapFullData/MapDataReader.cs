using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDataReader : MonoBehaviour
{
    public static MapDataReader         Instance { get; private set; }
    public MapDataListSO                mapDataListSO;
    private List<MapProgressData>       mapProgressDataList = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMapData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadMapData()
    {
        List<MapDesignData> designList  = JSONManager.LoadMapDesignDataFromJson();
        List<MapProgressData> progressList = JSONManager.LoadMapProgressDataFromJson();
        foreach(var design in designList)
        {
            MapProgressData progress = progressList.Find(p => p.mapID == design.mapID);
            progress ??= InitMapProgressData(design.mapID);
            mapProgressDataList.Add(progress);

            MapData mapData = new()
            {
                mapID = design.mapID,
                mapName = design.mapName,
                goldInit = design.goldInit,
                lives = design.lives,
                description = design.description,
                initMapBtnPos = design.initMapBtnPos,
                activate = progress.activate,
                starsPoint = progress.starsPoint,
            };
            mapDataListSO.mapDataList.Add(mapData);
        }
    }

    private MapProgressData InitMapProgressData(int DesignMapID)
    {
        return new MapProgressData
        {
            mapID = DesignMapID,
            activate = false,
            starsPoint = 0,
        };
    }

    public void UpdateMapProgressDataList(MapPresenter mapPresenter, int starPoint)
    {
        for(int i = 0; i < mapProgressDataList.Count; i++)
        {
            if(mapProgressDataList[i].mapID != mapPresenter.mapModel.MapID) continue;
            if(i + 1 < mapProgressDataList.Count) mapProgressDataList[i+1].activate = true;

            if(mapProgressDataList[i].starsPoint >= starPoint) break;
            else mapProgressDataList[i].starsPoint = starPoint;
        }
        // JSONManager.SaveMapProgressDataToJson(mapProgressDataList);
    }
}
