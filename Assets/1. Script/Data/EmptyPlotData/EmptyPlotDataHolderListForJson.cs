using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPlotDataHolderListForJson : MonoBehaviour
{
    [SerializeField] List<EmptyPlotDataHolder> emptyPlotDataHolderList;

    public List<EmptyPlotDataHolder> GetEmptyPlotDataForJson()
    {
        foreach(EmptyPlotDataHolder emptyPlotDataHolder in emptyPlotDataHolderList)
        {
            emptyPlotDataHolder.UpdateEmptyPlotList();
        }
        return emptyPlotDataHolderList;
    }
}
