using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EmptyPlotManager : MonoBehaviour
{
    [SerializeField] EmptyPlot emptyPlotPrefab;
    public List<EmptyPlot> emptyPlotList = new List<EmptyPlot>();

    public void InitializeEmptyPlot(MapData mapData)
    {
        InitEmptyPlot(mapData);
    }

    private void InitEmptyPlot(MapData mapData)
    {
        List<Vector2> posList = WayPointDataReader.Instance.GetSelectedMapEmptyPlotPos(mapData);
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
