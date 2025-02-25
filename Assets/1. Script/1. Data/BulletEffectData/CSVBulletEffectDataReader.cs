using System;
using UnityEngine;

public class CSVBulletEffectDataReader : MonoBehaviour
{
    public static CSVBulletEffectDataReader     Instance { get; private set; }
    [SerializeField] TextAsset                  bulletEffectDataCSV;
    public BulletEffectDataListSO               bulletEffectDataList;
    public bool IsDataLoaded { get; private set; }

    private void Awake()
    {
        if( Instance == null)
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

    private void Start()
    {
        if (bulletEffectDataList == null)
        {
            Debug.LogError("BulletDataCSV is not assigned.");
            return;
        }
        LoadTowerData();
    }

    private void LoadTowerData()
    {
        string[] Lines = bulletEffectDataCSV.text.Split('\n');
        // i = 1 to Skip the first row (it's the title row)
        for (int i = 1; i < Lines.Length; i++)
        {
            string[] values = Lines[i].Split(',');
            // make sure to only add rows that contain enough information (8 columns of information)
            // if it not, move to the next line
            //if(values.Length < 5) continue;
            BulletEffectData effectData = new BulletEffectData
            {
                effectType              = values[0].Trim().ToLower(),
                effectValue             = int.Parse(values[1]),
                effectDuration          = float.Parse(values[2]),
                effectOccursTime        = int.Parse(values[3]),
                effectRange             = float.Parse(values[4]),
            };
            bulletEffectDataList.bulletEffectDataList.Add(effectData);
        }
        IsDataLoaded = true;
    }
}
