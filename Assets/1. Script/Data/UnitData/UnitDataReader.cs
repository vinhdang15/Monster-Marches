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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (unitDataListSO == null)
        {
            Debug.LogError("UnitDataCSV is not assigned.");
            return;
        }
        LoadTowerData();
    }

    private void LoadTowerData()
    {
        unitDataListSO.unitDataList = JSONManager.LoadUnitDataFromJson();
    }
}
