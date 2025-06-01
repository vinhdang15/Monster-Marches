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
            goldInit = 155,
            lives = 5,
            description = "A dark force is marching through the forest.\nYour mission: stop the corrupted monster before they wither the land.\n\nBeware - Thier leader comes last, and it poisons everything it passes.",
            initMapBtnPos = Vector2.zero,
        },
        new MapDesignData
        {
            mapID = 2,
            mapName = "Forest_2",
            goldInit = 225,
            lives = 7,
            description = "Two paths wind through the forest, but all lead to ruin if you fail. Intercept the dark creatures before they taint the crops and skies.\n\nA devastating monster will arrive late - be ready",
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