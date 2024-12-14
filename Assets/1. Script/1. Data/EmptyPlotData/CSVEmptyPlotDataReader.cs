using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVEmptyPlotDataReader : MonoBehaviour
{
    public static CSVEmptyPlotDataReader Instance { get; private set; }
    [SerializeField] TextAsset emptyPlotCSV;
    public EmptyPlotDataListSO emptyPlotDataList;
    public bool IsDataLoaded { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadEmptyPlotData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadEmptyPlotData()
    {
        string[] lines = emptyPlotCSV.text.Split('\n');
        for(int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if(values.Length < 2) continue;
            EmptyPlotData emptyPlotData = new EmptyPlotData
            {
                x = float.Parse(values[0]),
                y = float.Parse(values[1]),
            };
            emptyPlotDataList.emptyPlotDataList.Add(emptyPlotData);
        }
        IsDataLoaded = true;
    }
}
