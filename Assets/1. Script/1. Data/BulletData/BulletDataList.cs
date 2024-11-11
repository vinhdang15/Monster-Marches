using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataList", menuName = "ScriptableObjects/BulletDataList", order = 1)]
public class BulletDataList : ScriptableObject
{
    public List<BulletData> bulletDataList = new List<BulletData>();

    public BulletData GetBulletData(string type)
    {
        Debug.Log(type);
        return bulletDataList.Find(data => data.type == type);
    }
}