using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataJsonCreater : MonoBehaviour
{
    [SerializeField] MapDataForJson mapdataForJson;

    private void Awake()
    {
        mapdataForJson = GetComponent<MapDataForJson>();
    }

    private void Start()
    {
        List<MapData> mapDatas = mapdataForJson.GetMapdataForJson();
        JSONManager.SaveMapDataToJson(mapDatas);
    }
}
