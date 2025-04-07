using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletEffectDataList", menuName = "Data Config/BulletEffectDataList", order = 1)]
public class BulletEffectDataListSO : ScriptableObject
{
    public List<BulletEffectData> bulletEffectDataList = new List<BulletEffectData>();
    public BulletEffectData GetBulletEffectData(string bulletEffectType)
    {  
        string normalizedEffectTypee = bulletEffectType;
        return bulletEffectDataList.Find(data => data.effectType == bulletEffectType);
    }
}
