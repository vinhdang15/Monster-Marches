using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    public static UnitPool Instance { get; private set; }
    [System.Serializable]
    public class UnitPoolInfor
    {
        public UnitBase unitPrefab;
        public string unitType;
        public string UnitID => unitPrefab.name;
        public int poolSize; 
    }
    public List<UnitPoolInfor> unitPoolInfors = new List<UnitPoolInfor>();
    public Dictionary<string, Queue<Enemy>> enemyPool = new Dictionary<string, Queue<Enemy>>();
    public Dictionary<string, Queue<Soldier>> soldierPool = new Dictionary<string, Queue<Soldier>>();

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
        InitializeUnitPool();
    }

    private void InitializeUnitPool()
    {
        UnitType();
        InitializeEnemyPools();
        InitializeSoldierPools();
    }

    private void UnitType()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            unitPoolInfor.unitType = UnitDataReader.Instance.unitDataListSO.GetUnitType(unitPoolInfor.UnitID);
        }
    }

    private void InitializeEnemyPools()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            if(unitPoolInfor.unitType != "Enemy") continue;
            Queue<Enemy> enemyQueue = new Queue<Enemy>();
            for(int i = 0; i < unitPoolInfor.poolSize; i++)
            {
                Enemy enemy = (Enemy)Instantiate(unitPoolInfor.unitPrefab, transform);
                UnitData unitData = UnitDataReader.Instance.unitDataListSO.GetUnitData(unitPoolInfor.UnitID);
                enemy.InitUnit(unitData);
                enemy.GetAnimation();
                enemy.gameObject.SetActive(false);
                enemyQueue.Enqueue(enemy);
            }
            enemyPool.Add(unitPoolInfor.UnitID, enemyQueue);
        }
    }

    private void InitializeSoldierPools()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            if(unitPoolInfor.unitType != "Soldier") continue;
            Queue<Soldier> soldierQueue = new Queue<Soldier>();
            for( int i = 0; i < unitPoolInfor.poolSize; i++)
            {
                UnitData soldierData = UnitDataReader.Instance.unitDataListSO.GetUnitData(unitPoolInfor.UnitID);
                Soldier soldier = (Soldier)Instantiate(unitPoolInfor.unitPrefab, transform);
                soldier.InitUnit(soldierData);
                soldier.GetAnimation();
                soldier.gameObject.SetActive(false);
                soldierQueue.Enqueue(soldier);
            }
            soldierPool.Add(unitPoolInfor.UnitID, soldierQueue);
        }
    }

    // get enemy from pool
    public Enemy GetEnemy(string enemyID)
    {
        if(!enemyPool.ContainsKey(enemyID))
        {
            Debug.Log("there is no " + enemyID);
            return null;
        }
        
        if(enemyPool[enemyID].Count > 0)
        {
            Enemy enemy = enemyPool[enemyID].Dequeue() as Enemy;
            enemy.GetAnimation();
            enemy.isdead = false;
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else // Init unit if out of unit in pool
        {
            Enemy unitPrefab = GetUnitPrefab(enemyID) as Enemy;
            Enemy enemy = Instantiate(unitPrefab, transform);
            UnitData unitData = UnitDataReader.Instance.unitDataListSO.GetUnitData(unitPrefab.name.Trim().ToLower());
            enemy.InitUnit(unitData);
            enemy.GetAnimation();
           return enemy;
        }
    }

    // get soldier from pool
    public Soldier GetSoldier(string unitID, Vector2 initPos)
    {
        if(!soldierPool.ContainsKey(unitID))
        {
            Debug.Log("there is no " + unitID);
            return null;
        }
        if(soldierPool[unitID].Count > 0)
        {
            Soldier soldier = soldierPool[unitID].Dequeue();
            soldier.transform.position = initPos;
            soldier.isdead = false;
            soldier.gameObject.SetActive(true);
            return soldier;
        }
        else // Init unit if out of unit in pool
        {
            Soldier unitPrefab = (Soldier)GetUnitPrefab(unitID);
            Soldier soldier = Instantiate(unitPrefab, initPos, Quaternion.identity, transform);
            UnitData unitData = UnitDataReader.Instance.unitDataListSO.GetUnitData(unitPrefab.name);
            soldier.InitUnit(unitData);
            soldier.GetAnimation();
           return soldier;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        if(enemyPool.ContainsKey(enemy.UnitID))
        {
            enemyPool[enemy.UnitID].Enqueue(enemy);
        }
        enemy.ResetUnit();
    }

    public void ReturnSoldier(Soldier soldier)
    {
        soldier.gameObject.SetActive(false);
        if(soldierPool.ContainsKey(soldier.UnitID))
        {
            soldierPool[soldier.UnitID].Enqueue(soldier);
        }
        soldier.ResetUnit();
    }

    private UnitBase GetUnitPrefab(string unitID)
    {
        foreach(UnitPoolInfor unitPoolInfo in unitPoolInfors)
        {
            if(unitPoolInfo.UnitID == unitID)
            {
                return unitPoolInfo.unitPrefab;
            }
        }
        return null;
    }

}
