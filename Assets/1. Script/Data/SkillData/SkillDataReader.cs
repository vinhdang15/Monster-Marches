using System.Linq;
using UnityEngine;

public class SkillDataReader : MonoBehaviour
{
    public static SkillDataReader    Instance { get; private set; }
    public SkillDataListSO           skillDataListSO;

    private void Awake()
    {
        if (Instance == null)
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
        skillDataListSO.skillDataList = JSONManager.LoadSkillDataFromJson();
    }
}
