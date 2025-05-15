using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjData
{
    public int mapID;
    public List<DecorObjectInfo> decorObjectInforList;
}

[System.Serializable]
public class DecorObjectInfo
{
    public string decorID;
    public List<Vector2> decorObjectPosList;
}