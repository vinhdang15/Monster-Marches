using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    [System.Serializable]
    public class UnitPoolInfor
    {
        public UnitBase unitPrefab;
        public string unitType;
        public string UnitName => unitPrefab.name.Trim().ToLower();
        public int poolSize; 
    }
    public List<UnitPoolInfor> unitPoolInfors = new List<UnitPoolInfor>();
    public Dictionary<string, Queue<Enemy>> enemyPool = new Dictionary<string, Queue<Enemy>>();
    public Dictionary<string, Queue<Soldier>> soldierPool = new Dictionary<string, Queue<Soldier>>();
    

    private void Start()
    {
        InitializePoolsCoroutine();
    }

    private void UnitType()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            unitPoolInfor.unitType = CSVUnitDataReader.Instance.unitDataList.GetUnitType(unitPoolInfor.UnitName);
        }
    }

    private void InitializePoolsCoroutine()
    {
        // yield return new WaitUntil(() => CSVUnitDataReader.Instance.IsDataLoaded);
        UnitType();
        InitializeEnemyPools();
        InitializeSoldierPools();
    }

    private void InitializeEnemyPools()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            if(unitPoolInfor.unitType != "enemy") continue;
            Queue<Enemy> enemyQueue = new Queue<Enemy>();
            for( int i = 0; i < unitPoolInfor.poolSize; i++)
            {
                Enemy enemy = (Enemy)Instantiate(unitPoolInfor.unitPrefab, transform);
                UnitData unitData = CSVUnitDataReader.Instance.unitDataList.GetUnitData(unitPoolInfor.UnitName);
                enemy.InItUnit(unitData);
                enemy.GetAnimation();
                enemy.gameObject.SetActive(false);
                enemyQueue.Enqueue(enemy);
            }
            enemyPool.Add(unitPoolInfor.UnitName, enemyQueue);
        }
    }

    private void InitializeSoldierPools()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            if(unitPoolInfor.unitType != "soldier") continue;
            Queue<Soldier> soldierQueue = new Queue<Soldier>();
            for( int i = 0; i < unitPoolInfor.poolSize; i++)
            {
                Soldier soldier = (Soldier)Instantiate(unitPoolInfor.unitPrefab, transform);
                UnitData soldierData = CSVUnitDataReader.Instance.unitDataList.GetUnitData(unitPoolInfor.UnitName);
                soldier.InItUnit(soldierData);
                soldier.GetAnimation();
                soldier.gameObject.SetActive(false);
                soldierQueue.Enqueue(soldier);
            }
            soldierPool.Add(unitPoolInfor.UnitName, soldierQueue);
        }
    }

    // get enemy from pool
    public Enemy GetEnemy(string EnemyName)
    {
        if(!enemyPool.ContainsKey(EnemyName))
        {
            Debug.Log("there is no " + EnemyName);
            return null;
        }
        if(enemyPool[EnemyName].Count > 0)
        {
            Enemy enemy = enemyPool[EnemyName].Dequeue() as Enemy;
            enemy.GetAnimation();
            enemy.isdead = false;
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else // Init unit if out of unit in pool
        {
            Enemy unitPrefab = GetUnitPrefab(EnemyName) as Enemy;
            Enemy enemy = Instantiate(unitPrefab, transform);
            UnitData unitData = CSVUnitDataReader.Instance.unitDataList.GetUnitData(unitPrefab.name.Trim().ToLower());
            enemy.InItUnit(unitData);
            enemy.GetAnimation();
           return enemy;
        }
    }

    // get soldier from pool
    public Soldier GetSoldier(string unitName, Vector2 initPos)
    {
        if(!soldierPool.ContainsKey(unitName))
        {
            Debug.Log("there is no " + unitName);
            return null;
        }
        if(soldierPool[unitName].Count > 0)
        {
            Soldier soldier = soldierPool[unitName].Dequeue();
            soldier.transform.position = initPos;
            soldier.isdead = false;
            soldier.gameObject.SetActive(true);
            return soldier;
        }
        else // Init unit if out of unit in pool
        {
            Soldier unitPrefab = (Soldier)GetUnitPrefab(unitName);
            Soldier soldier = Instantiate(unitPrefab, initPos, Quaternion.identity, transform);
            UnitData unitData = CSVUnitDataReader.Instance.unitDataList.GetUnitData(unitPrefab.name.Trim().ToLower());
            soldier.InItUnit(unitData);
            soldier.GetAnimation();
           return soldier;
        }
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        if(enemyPool.ContainsKey(enemy.UnitName))
        {
            enemyPool[enemy.UnitName].Enqueue(enemy);
        }
        enemy.ResetUnit();
    }

    public void ReturnSoldier(Soldier soldier)
    {
        soldier.gameObject.SetActive(false);
        if(soldierPool.ContainsKey(soldier.UnitName))
        {
            soldierPool[soldier.UnitName].Enqueue(soldier);
        }
        soldier.ResetUnit();
    }

    private UnitBase GetUnitPrefab(string unitName)
    {
        foreach(UnitPoolInfor unitPoolInfo in unitPoolInfors)
        {
            if(unitPoolInfo.UnitName == unitName)
            {
                return unitPoolInfo.unitPrefab;
            }
        }
        return null;
    }

}
