using UnityEngine;

public class BulletDataReader : MonoBehaviour
{
    public BulletDataListSO             bulletDataListSO;

    public void PrepareGame()
    {
        bulletDataListSO.bulletDataList = JSONDataLoader.bulletDataList;
    }
}
