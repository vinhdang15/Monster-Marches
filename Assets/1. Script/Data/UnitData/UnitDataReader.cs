using UnityEngine;

public class UnitDataReader : MonoBehaviour
{
    public UnitDataListSO            unitDataListSO;

    public void PrepareGame()
    {
        unitDataListSO.unitDataList = JSONManager.unitDataList;
    }
}
