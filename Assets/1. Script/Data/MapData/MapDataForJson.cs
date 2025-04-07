using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataForJson : MonoBehaviour
{
    // using this class to contain data to create mapdata json original file
    // only use when want to add new map or remove existing map
    private List<MapData> mapDataList = new()
    {
        new MapData
        {
            mapID = 1,
            mapName = "Forest_1",
            goldInit = 1000,
            lives = 18,
            description = "welcome to my dev joney.\nI think I gonna make it right.",
            activate = true,
            mapPassed = false,
            mapStars = 0,
            initMapBtnPos = new Vector2(-3, 0),
        },
        new MapData
        {
            mapID = 2,
            mapName = "Forest_2",
            goldInit = 500,
            lives = 10,
            description = "welcome to the Forest_2.\nLet's play",
            activate = false,
            mapPassed = false,
            mapStars = 0,
            initMapBtnPos = new Vector2(3, 0),
        },
    };

    public List<MapData> GetMapDataForJson()
    {
        return mapDataList;
    }
}