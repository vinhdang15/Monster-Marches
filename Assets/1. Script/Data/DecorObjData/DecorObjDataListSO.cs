using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorObjDataList", menuName = "DecorObj Config/DecorObjDataListSO", order = 1)]
public class DecorObjDataListSO : ScriptableObject
{
    public List<DecorObjData> decorObjDatas = new();
}
