using System.Collections.Generic;
using UnityEngine;

public class MapModel : MonoBehaviour
{
    public int              MapID { get ; set ; }
    public string           MapName { get ; set ; }
    public int              GoldInit { get ; set ; }
    public int              MapLives { get ; set ; }
    public string           Description { get ; set ; }
    public bool             Activate { get ; set ; }
    public bool             MapPassed { get ; set ; }
    public int              StarScore { get ; set ; }
    public List<string>     PathID { get ; set ; }
    public Vector2          InitMapBtnPos { get ; set ; }
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
        this.GoldInit = mapData.goldInit;
        this.MapLives = mapData.lives;
        this.Description = mapData.description;
        this.Activate = mapData.activate;
        this.StarScore = mapData.starsPoint;
        this.InitMapBtnPos = mapData.initMapBtnPos;
    }
}
