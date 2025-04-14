using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDesignDataForJson : MonoBehaviour
{
    // using this class to contain data to create mapdata json original file
    // only use when want to add new map or remove existing map
    [SerializeField] Transform initMapBtnPosList;
    private List<MapDesignData> mapDesignDataList = new()
    {
        new MapDesignData
        {
            mapID = 1,
            mapName = "Forest_1",
            goldInit = 1000,
            lives = 18,
            description = "welcome to my dev joney.\nI think I gonna make it right.",
            initMapBtnPos = Vector2.zero,
        },
        new MapDesignData
        {
            mapID = 2,
            mapName = "Forest_2",
            goldInit = 500,
            lives = 10,
            description = "welcome to the Forest_2.\nLet's play",
            initMapBtnPos = Vector2.zero,
        },
    };

    private void GetInitMapBtnPos()
    {
        for(int i = 0; i < initMapBtnPosList.childCount; i++)
        {
            mapDesignDataList[i].initMapBtnPos = initMapBtnPosList.GetChild(i).position;
        }
    }

    public List<MapDesignData> GetMapDesignDataForJson()
    {
        GetInitMapBtnPos();
        return mapDesignDataList;
    }
}