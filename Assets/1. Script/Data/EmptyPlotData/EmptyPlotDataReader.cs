using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPlotDataReader : MonoBehaviour
{
    public static EmptyPlotDataReader    Instance { get; private set; }
    public EmptyPlotDataListSO           emptyPlotDataListSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadEmptyPlotData();
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadEmptyPlotData()
    {
        emptyPlotDataListSO.emptyPlotDataList = JSONManager.LoadEmptyPlotDataFromJson();
    }

    public List<Vector2> GetEmptyPlotDataSelectedMapList(int mapID)
    {
        List<Vector2> emptyPlotDataList = new();
        EmptyPlotSerializableData emptyPlotDataHolder = EmptyPlotDataHolderSelectedMap(mapID);
        foreach(Vector2Serializable i in emptyPlotDataHolder.emptyPlotList)
        {
            Vector2 pos = i.ToVector2();
            emptyPlotDataList.Add(pos);
        }
        return emptyPlotDataList;
    }

    private EmptyPlotSerializableData EmptyPlotDataHolderSelectedMap(int mapID)
    {
        foreach(var emptyPlotDataHolder in emptyPlotDataListSO.emptyPlotDataList)
        {
            if(emptyPlotDataHolder.mapID == mapID)
            {
                return emptyPlotDataHolder;
            }
        }
        Debug.Log("There is no emptyPlot for mapID " + mapID);
        return null;
    }
}
