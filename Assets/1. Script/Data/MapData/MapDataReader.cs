using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapDataReader : MonoBehaviour
{
    public static MapDataReader Instance { get; private set; }
    public MapDataListSO        mapDataListSO;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMapData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadMapData()
    {
        mapDataListSO.mapDataList = JSONManager.LoadMapDataFromJson();
    }
}
