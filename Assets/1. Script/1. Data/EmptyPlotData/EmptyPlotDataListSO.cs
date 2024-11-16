using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyPlotDataList", menuName = "Data Config/EmptyPlotDataList", order = 2)]
public class EmptyPlotDataListSO : ScriptableObject
{
    public List<EmptyPlotData> emptyPlotDataList = new List<EmptyPlotData>();

    public EmptyPlotData GetEmptyPlotData(int index)
    {
        if(index < emptyPlotDataList.Count)
        {
            return emptyPlotDataList[index];
        }
        else
        {
            return null;
        }
        
    }
}
