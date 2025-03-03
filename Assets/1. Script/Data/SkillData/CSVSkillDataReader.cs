using System.Linq;
using UnityEngine;

public class CSVSkillDataReader : MonoBehaviour
{
    public static CSVSkillDataReader    Instance { get; private set; }
    [SerializeField] TextAsset          skillDataCSV;
    public SkillDataListSO              skillDataListSO;

    private void Awake()
    {
        if (Instance == null)
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
        string[] Lines = skillDataCSV.text.Split('\n');
        // i = 1 to Skip the first row (it's the title row)
        for (int i = 1; i < Lines.Length; i++)
        {
            string[] values = Lines[i].Split(',');
            // make sure to only add rows that contain enough information (8 columns of information)
            // if it not, move to the next line
            if(values.Length < 4) continue;
            SkillData skillData = new SkillData
            {
                skillType              = values[0].Trim().ToLower(),
                skillValue             = int.Parse(values[1]),
                skillOccursTime        = float.Parse(values[2]),
                skillRange             = float.Parse(values[3]),
            };
            skillDataListSO.skillDataList.Add(skillData);
        }
    }
}
