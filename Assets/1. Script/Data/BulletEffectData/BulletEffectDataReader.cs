using System;
using UnityEngine;

public class BulletEffectDataReader : MonoBehaviour
{
    public static BulletEffectDataReader     Instance { get; private set; }
    public BulletEffectDataListSO               bulletEffectDataSO;
    public bool IsDataLoaded { get; private set; }

    private void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        bulletEffectDataSO.bulletEffectDataList = JSONManager.bulletEffectDataList;
    }
}
