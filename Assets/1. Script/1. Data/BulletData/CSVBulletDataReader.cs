using UnityEngine;

public class CSVBulletDataReader : MonoBehaviour
{
    [SerializeField] TextAsset        bulletDataCSV;
    public           BulletDataListSO    bulletDataList;
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
            if(values.Length < 7) continue;
            BulletData bulletData = new BulletData
            {
                type                    = values[0],
                speed                   = float.Parse(values[1]),
                damage                  = int.Parse(values[2]),
                damageOverTimeCount     = int.Parse(values[3]),
                damageTarget            = values[4],
                damageRange             = float.Parse(values[5]),
                damageEffect            = values[6],
            };
            bulletDataList.bulletDataList.Add(bulletData);
        }
        IsDataLoaded = true;
    }
}
