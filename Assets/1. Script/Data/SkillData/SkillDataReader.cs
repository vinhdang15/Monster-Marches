using System.Linq;
using UnityEngine;

public class SkillDataReader : MonoBehaviour
{
    public static SkillDataReader    Instance { get; private set; }
    public SkillDataListSO           skillDataListSO;

    public void PrepareGame()
    {
        skillDataListSO.skillDataList = JSONDataLoader.skillDataList;
    }
}
