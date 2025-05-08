using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    [System.Serializable]
    public class BulletPoolInfo
    {
        public string       BulletID => bulletPrefab.name;
        public BulletBase   bulletPrefab;
        public int          poolSize;
    }

    public List<BulletPoolInfo> bulletPoolInfos;
    Dictionary<string, Queue<BulletBase>> bulletPools = new Dictionary<string, Queue<BulletBase>>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
        public void Initialize()
    {
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {
        foreach(var bulletPoolInfo in bulletPoolInfos)
        {
            Queue<BulletBase> bulletQueue = new Queue<BulletBase>();
            for(int i = 0; i < bulletPoolInfo.poolSize; i++)
            {
                BulletBase bullet = Instantiate(bulletPoolInfo.bulletPrefab, transform);
                BulletData bulletData = BulletDataReader.Instance.bulletDataListSO.GetBulletData(bulletPoolInfo.BulletID);
                bullet.InitBullet(bulletData);

                bullet.gameObject.SetActive(false);
                bulletQueue.Enqueue(bullet);
            }
            bulletPools.Add(bulletPoolInfo.BulletID,bulletQueue);
        }
    }

    // Get bullet from pool
    public BulletBase GetBullet(string bulletType, Vector2 initPos)
    {
        if(!bulletPools.ContainsKey(bulletType))
        {
            Debug.LogWarning("there is no pool for " + bulletType);
            return null;
        }
        if(bulletPools[bulletType].Count > 0)
        {
            BulletBase bullet = bulletPools[bulletType].Dequeue();
            bullet.transform.position = initPos;
            bullet.startPos = initPos;
            bullet.isSetUpStartPos = true;
            bullet.gameObject.SetActive(true);
            bullet.BulletWhistleSound();
            return bullet;
        }
        else // Init bullet if out of bullet in pool
        {
            BulletBase bulletPrefab = GetBulletPrefab(bulletType);
            BulletBase bullet = Instantiate(bulletPrefab, initPos, Quaternion.identity, transform);
            BulletData bulletData = BulletDataReader.Instance.bulletDataListSO.GetBulletData(bulletPrefab.BulletID);
            bullet.InitBullet(bulletData);
            bullet.transform.position = initPos;
            bullet.startPos = initPos;
            bullet.isSetUpStartPos = true;
            return bullet;
        }
    }

    // Return bullet to pool
    public void ReturnBullet(BulletBase bullet)
    {
        bullet.ResetBullet();
        bullet.gameObject.SetActive(false);
        if(bulletPools.ContainsKey(bullet.BulletID))
        {
            bulletPools[bullet.BulletID].Enqueue(bullet);
        }
    }

    private BulletBase GetBulletPrefab(string bulletType)
    {
        foreach(BulletPoolInfo bulletPoolInfo in bulletPoolInfos)
        {
            if(bulletPoolInfo.BulletID == bulletType)
            {
                return bulletPoolInfo.bulletPrefab;
            }
        }
        return null;
    }
}
