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
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadData()
    {
        unitDataListSO.unitDataList = JSONManager.LoadUnitDataFromJson();
    }
}
