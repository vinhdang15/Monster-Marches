using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPlotDataHolder : MonoBehaviour
{
    public int mapID;
    public List<Vector2Serializable> emptyPlotList;

    public void UpdateEmptyPlotList()
    {
        if(emptyPlotList != null) emptyPlotList.Clear();

        for(int i = 0; i < transform.childCount; i++)
        {
            Vector2 pos = gameObject.transform.GetChild(i).position;
            Vector2Serializable vector2Serializable = new(pos.x,pos.y);
            emptyPlotList.Add(vector2Serializable);
        }
    }
}


// For EmptyPlotData
// EmptyPlotData is MonoBehaviour so can't serialize  it directly.  
// need to use EmptyPlotSerializableData
// [System.Serializable]
public class EmptyPlotSerializableData
{
    public int mapID;
    public List<Vector2Serializable> emptyPlotList = new();

    public EmptyPlotSerializableData(int mapID, List<Vector2Serializable> emptyPlotList)
    {
        this.mapID = mapID;
        this.emptyPlotList = emptyPlotList;
    }
}