using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MapProgressManager
{
    private static List<MapProgressData> mapProgressDataList => JSONManager.mapProgressDataList;

    public static void UpdateProgress(int mapID, int newStarPoint)
    {
        var progress = mapProgressDataList.FirstOrDefault(p => p.mapID == mapID);
        if(progress == null) return;

        // Unlock next map
        int nextMapID = progress.mapID + 1;
        var nextMap = mapProgressDataList.FirstOrDefault(p => p.mapID == nextMapID);
        if(nextMap != null) nextMap.activate = true;

        // Update current map star
        if(progress.starsPoint < newStarPoint) progress.starsPoint = newStarPoint;

        // Save progress save-data
        JSONManager.SaveMapProgressDataToJson(mapProgressDataList);
        Debug.Log("Save progress save-data");
    }
}
