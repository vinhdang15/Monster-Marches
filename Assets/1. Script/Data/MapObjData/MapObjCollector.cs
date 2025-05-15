
using System.Collections.Generic;
using UnityEngine;

public class MapObjCollector : MonoBehaviour
{
    public List<GameDecorObjList> gameDecorObjList;
}

[System.Serializable]
public class GameDecorObjList
{
    public int mapID;
    public List<MapDecorHolderObj> mapDecorObjHolderList;
}

[System.Serializable]
public class MapDecorHolderObj
{
    public string decorObjID;
    public Transform decorObjHolder;
}
