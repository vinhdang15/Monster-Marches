using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDataReader : MonoBehaviour
{
    public static MapDataReader         Instance { get; private set; }
    public MapDataListSO                mapDataListSO;

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
        InitFullMapData();
    }

    private void InitFullMapData()
    {
        List<MapDesignData> designList  = JSONManager.mapDesignDataList;
        List<MapProgressData> progressList = JSONManager.mapProgressDataList;
        List<MapProgressData> fullProgressList = new();
        foreach(var design in designList)
        {
            MapProgressData progress = progressList.Find(p => p.mapID == design.mapID);
            if(progress == null)
            {
                progress = InitMapProgressData(design.mapID); 
            }
            fullProgressList.Add(progress);

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
        JSONManager.SaveMapProgressDataToJson(fullProgressList);
        JSONManager.mapProgressDataList = fullProgressList;
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

    public void ResetFullMapData()
    {
        ClearMapDataListSO();
        InitFullMapData();
    }
    
    public void ClearMapDataListSO()
    {
        mapDataListSO.mapDataList.Clear();
    }
}