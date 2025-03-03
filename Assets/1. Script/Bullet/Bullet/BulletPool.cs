using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [System.Serializable]
    public class BulletPoolInfo
    {
        public string       BulletType => bulletPrefab.name.Trim().ToLower();
        public BulletBase   bulletPrefab;
        public int          poolSize;
    }

    public List<BulletPoolInfo> bulletPoolInfos;
    Dictionary<string, Queue<BulletBase>> bulletPools = new Dictionary<string, Queue<BulletBase>>();

    private void Start()
    {   
        // wait bullet data && bullet effect data been readed atfer that init bullet
        StartCoroutine(InitializePoolsCoroutine());
    }

    private IEnumerator InitializePoolsCoroutine()
    {
        yield return new WaitUntil(() => CSVBulletDataReader.Instance.IsDataLoaded && CSVBulletEffectDataReader.Instance.IsDataLoaded);
        InitializePools();
    }

    private void InitializePools()
    {
        foreach(var bulletPoolInfo in bulletPoolInfos)
        {
            Queue<BulletBase> bulletQueue = new Queue<BulletBase>();
            for(int i = 0; i < bulletPoolInfo.poolSize; i++)
            {
                BulletBase bullet = Instantiate(bulletPoolInfo.bulletPrefab, transform);
                BulletData bulletData = CSVBulletDataReader.Instance.bulletDataList.GetBulletData(bulletPoolInfo.BulletType);
                bullet.InitBullet(bulletData);

                bullet.gameObject.SetActive(false);
                bulletQueue.Enqueue(bullet);
            }
            bulletPools.Add(bulletPoolInfo.BulletType,bulletQueue);
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
            BulletData bulletData = CSVBulletDataReader.Instance.bulletDataList.GetBulletData(bulletPrefab.BulletType);
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
        if(bulletPools.ContainsKey(bullet.BulletType))
        {
            bulletPools[bullet.BulletType].Enqueue(bullet);
        }
    }

    private BulletBase GetBulletPrefab(string bulletType)
    {
        foreach(BulletPoolInfo bulletPoolInfo in bulletPoolInfos)
        {
            if(bulletPoolInfo.BulletType == bulletType)
            {
                return bulletPoolInfo.bulletPrefab;
            }
        }
        return null;
    }
}
