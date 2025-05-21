using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EmptyPlotManager : MonoBehaviour
{
    [SerializeField] EmptyPlot  emptyPlotPrefab;
    public List<EmptyPlot>      emptyPlotList = new List<EmptyPlot>();
    private WayPointDataReader  wayPointDataReader;

    public void PrepareGame(WayPointDataReader wayPointDataReader)
    {
        this.wayPointDataReader = wayPointDataReader;
    }

    public void InitializeEmptyPlot(MapData mapData)
    {
        InitEmptyPlot(mapData);
    }

    private void InitEmptyPlot(MapData mapData)
    {
        List<Vector2> posList = wayPointDataReader.GetSelectedMapEmptyPlotPos(mapData);
        foreach(var emptyPlotData in posList)
        {
            Vector2 pos = new Vector2(emptyPlotData.x,emptyPlotData.y);
            EmptyPlot emptyPlotScript = Instantiate(emptyPlotPrefab,pos, quaternion.identity, transform);
            emptyPlotScript.PrepareGame();
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

    public void ShowAllEmptyPlot()
    {
        foreach(EmptyPlot emptyPlot in emptyPlotList)
        {
            emptyPlot.EnableCollider();
        }
    }
}
