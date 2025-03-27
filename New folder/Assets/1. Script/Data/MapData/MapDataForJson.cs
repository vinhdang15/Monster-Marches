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
            imagePath = "xx",
            description = "welcom to my dev joney.\nI think I gonna make it right.",
            activate = true,
            mapPassed = false,
            starPoint = 0,
            pathID = new List<string>(){"1_1","1_2"},
            initPos = new Vector2Serializable(-3, 0),
            endPointsPos = new List<Vector2Serializable>
            {
                new Vector2Serializable(-1f, -2f),
                new Vector2Serializable(-10.35f, 0f),
            }
        },
        new MapData
        {
            mapID = 2,
            mapName = "Forest_2",
            imagePath = "xxx",
            description = "welcom to the Forest_2.\nLet's play",
            activate = false,
            mapPassed = false,
            starPoint = 0,
            pathID = new List<string>(){"1_1","1_2"},
            initPos = new Vector2Serializable(3, 0),
            endPointsPos = new List<Vector2Serializable>
            {
                new Vector2Serializable(-1f, -2f),
                new Vector2Serializable(-10.35f, 0f),
            }
        },
    };

    public List<MapData> GetMapdataForJson()
    {
        return mapDataList;
    }
}
