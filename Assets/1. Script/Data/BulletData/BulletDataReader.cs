using System.Linq;
using UnityEngine;

public class BulletDataReader : MonoBehaviour
{
    public static BulletDataReader      Instance { get; private set; }
    public BulletDataListSO             bulletDataListSO;

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
        bulletDataListSO.bulletDataList = JSONManager.LoadBulletDataFromJson();
    }
}
