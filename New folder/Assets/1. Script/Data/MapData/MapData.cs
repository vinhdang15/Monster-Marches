using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int mapID;
    public string mapName;
    public string imagePath;
    public string description;
    public bool activate;
    public bool mapPassed;
    public int starPoint;
    public List<string> pathID;
    public Vector2Serializable initPos;
    public List<Vector2Serializable> endPointsPos;
    

    public List<Vector2> GetEndPointsPos()
    {
        List<Vector2> posList = new List<Vector2>();
        foreach( Vector2Serializable vector2Serializable in endPointsPos)
        {
            Vector2 pos = vector2Serializable.ToVector2();
            posList.Add(pos);
        }
        return posList;
    }
}

[System.Serializable]
public struct Vector2Serializable
{
    public float x;
    public float y;

    public Vector2Serializable(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
