using System.Linq;
using UnityEngine;

public class CSVBulletDataReader : MonoBehaviour
{
    [SerializeField] TextAsset              bulletDataCSV;
    public           BulletDataListSO       bulletDataList;
    public bool IsDataLoaded { get; private set; }

    private void Start()
    {
        if (bulletDataList == null)
        {
            Debug.LogError("BulletDataCSV is not assigned.");
            return;
        }
        LoadTowerData();
    }

    private void LoadTowerData()
    {
        string[] Lines = bulletDataCSV.text.Split('\n');
        // i = 1 to Skip the first row (it's the title row)
        for (int i = 1; i < Lines.Length; i++)
        {
            string[] values = Lines[i].Split(',');
            // make sure to only add rows that contain enough information (8 columns of information)
            // if it not, move to the next line
            if(values.Length < 5) continue;
            BulletData bulletData = new BulletData
            {
                type                    = values[0].Trim().ToLower(),
                damage                  = int.Parse(values[1]),
                speed                   = float.Parse(values[2]),
                effectTyes              = values[3].Trim().ToLower(),
                dealDamageDelay         = float.Parse(values[4])
            };
            bulletDataList.bulletDataList.Add(bulletData);
        }
        IsDataLoaded = true;
    }
}
