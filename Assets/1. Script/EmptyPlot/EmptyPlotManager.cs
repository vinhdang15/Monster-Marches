using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EmptyPlotManager : MonoBehaviour
{
    [SerializeField] EmptyPlot emptyPlotPrefab;
    public List<EmptyPlot> emptyPlotList = new List<EmptyPlot>();

    public void InitializeEmptyPlot(int mapID)
    {
        InitEmptyPlot(mapID);
    }

    private void InitEmptyPlot(int mapID)
    {
        List<Vector2> emptyPlotDataList = EmptyPlotDataReader.Instance.GetEmptyPlotDataSelectedMapList(mapID);
        foreach(var emptyPlotData in emptyPlotDataList)
        {
            Vector2 pos = new Vector2(emptyPlotData.x,emptyPlotData.y);
            EmptyPlot emptyPlotScript = Instantiate(emptyPlotPrefab,pos, quaternion.identity, transform);
            emptyPlotList.Add(emptyPlotScript);
        }
    }

    public void ClearEmptyPlot()
    {
        foreach(EmptyPlot emptyPlot in emptyPlotList)
        {
            Destroy(emptyPlot.gameObject);
        }
        emptyPlotList.Clear();
    }
}
