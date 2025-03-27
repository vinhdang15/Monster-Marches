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
            LoadBulletData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadBulletData()
    {
        bulletDataListSO.bulletDataList = JSONManager.LoadBulletDataFromJson();
    }
}
