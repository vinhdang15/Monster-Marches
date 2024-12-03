using UnityEngine;

public class CSVUnitDataReader : MonoBehaviour
{
    [SerializeField] TextAsset          unitDataCSV;
    public           UnitDataListSO     unitDataList;
    public bool IsDataLoaded { get; private set; }

    private void Start()
    {
        if (unitDataList == null)
        {
            Debug.LogError("UnitDataCSV is not assigned.");
            return;
        }
        LoadTowerData();
    }

    private void LoadTowerData()
    {
        string[] Lines = unitDataCSV.text.Split('\n');
        // i = 1 to Skip the first row (it's the title row)
        for (int i = 1; i < Lines.Length; i++)
        {
            string[] values = Lines[i].Split(',');
            // make sure to only add rows that contain enough information (8 columns of information)
            // if it not, move to the next line
            if(values.Length < 7) continue;
            UnitData unitData = new UnitData
            {
                unitType                    = values[0],
                unitName               = values[1],
                maxHP                   = int.Parse(values[2]),
                speed                   = float.Parse(values[3]),
                damage                  = int.Parse(values[4]),
                gold                    = int.Parse(values[5]),
                specialAbility          = values[6],
            };
            unitDataList.enemyDataList.Add(unitData);
        }
        IsDataLoaded = true;
    }
}
