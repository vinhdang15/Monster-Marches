using System;
using UnityEngine;

public class BulletEffectDataReader : MonoBehaviour
{
    public BulletEffectDataListSO               bulletEffectDataSO;
    public bool IsDataLoaded { get; private set; }

    public void PrepareGame()
    {
        bulletEffectDataSO.bulletEffectDataList = JSONDataLoader.bulletEffectDataList;
    }
}
