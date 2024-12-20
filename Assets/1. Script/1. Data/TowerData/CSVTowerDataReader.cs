using System.Collections.Generic;
using UnityEngine;

public class CSVTowerDataReader : MonoBehaviour
{
    public static CSVTowerDataReader    Instance { get; private set; }
    [SerializeField] TextAsset          towerDataCSV;
    public TowerDataListSO              towerDataList;
    public bool IsDataLoaded { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTowerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadTowerData()
    {
        string[] Lines = towerDataCSV.text.Split('\n');
        // i = 1 to Skip the first row (it's the title row)
        for (int i = 1; i < Lines.Length; i++)
        {
            string[] values = Lines[i].Split(',');
            // make sure to only add rows that contain enough information (8 columns of information)
            // if it not, move to the next line
            if(values.Length < 8) continue;
            TowerData towerData = new TowerData
            {
                towerType       = values[0].Trim().ToLower(),
                level           = int.Parse(values[1]),
                SpawnObject      = values[2].Trim().ToLower(),
                fireRate        = float.Parse(values[3]),
                rangeDetect     = float.Parse(values[4]),
                rangeRaycast    = float.Parse(values[5]),
                goldRequired    = int.Parse(values[6]),
                descriptions    = values[7],
            };
            towerDataList.towerDataList.Add(towerData);
        }
        IsDataLoaded = true;
    }
}
