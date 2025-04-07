using UnityEngine;

public class UnitDataReader : MonoBehaviour
{
    public static UnitDataReader     Instance { get; private set; }
    public UnitDataListSO            unitDataListSO;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTowerData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadTowerData()
    {
        unitDataListSO.unitDataList = JSONManager.LoadUnitDataFromJson();
    }
}
