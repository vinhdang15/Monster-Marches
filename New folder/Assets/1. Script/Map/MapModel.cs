using System.Collections.Generic;
using UnityEngine;

public class MapModel : MonoBehaviour
{
    public int              MapID { get ; set ; }
    public string           MapName { get ; set ; }
    public string           ImagePath { get ; set ; }
    public string           Description { get ; set ; }
    public bool             Activate { get ; set ; }
    public bool             MapPassed { get ; set ; }
    public int              StarPoint { get ; set ; }
    public List<string>     PathID { get ; set ; }
    public Vector2          InitPos { get ; set ; }
    public List<Vector2>    EndPointPos { get ; set ; }
    public MapData mapData;

    public static MapModel Create(MapBtn mapBtn, MapData mapData)
    {
        MapModel mapMode = mapBtn.gameObject.AddComponent<MapModel>();
        mapMode.InitBuildingMap(mapData);
        return mapMode;
    }

    private void InitBuildingMap(MapData mapData)
    {
        this.mapData = mapData;
        this.MapID = mapData.mapID;
        this.MapName = mapData.mapName;
        this.ImagePath = mapData.imagePath;
        this.Description = mapData.description;
        this.Activate = mapData.activate;
        this.MapPassed = mapData.mapPassed;
        this.StarPoint = mapData.starPoint;
        this.PathID = mapData.pathID;
        this.InitPos = mapData.initPos.ToVector2();
        this.EndPointPos = mapData.GetEndPointsPos();
    }
}
