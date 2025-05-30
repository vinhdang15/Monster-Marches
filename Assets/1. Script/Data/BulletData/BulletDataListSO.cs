using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataList", menuName = "Data Config/BulletDataList", order = 1)]
public class BulletDataListSO : ScriptableObject
{
    public List<BulletData> bulletDataList = new List<BulletData>();

    public BulletData GetBulletData(string type)
    {
        return bulletDataList.Find(data => data.bulletID == type);
    }

    public int GetBulletDamage(string type)
    {
        BulletData bulletData = bulletDataList.Find(data => data.bulletID == type);
        return bulletData.damage;
    }
}