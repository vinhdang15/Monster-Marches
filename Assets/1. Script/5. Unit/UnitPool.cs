using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    [SerializeField] CSVUnitDataReader unitDataReader;
    
    [System.Serializable]
    public class UnitPoolInfor
    {
        public UnitBase unitPrefab;
        public string unitName => unitPrefab.unitName;
        public int poolSize; 
    }
    public List<UnitPoolInfor> unitPoolInfors = new List<UnitPoolInfor>();
    public Dictionary<string, Queue<UnitBase>> unitPool = new Dictionary<string, Queue<UnitBase>>();
    

    private void Awake()
    {
        StartCoroutine(InitializePoolsCoroutine());
    }

    private IEnumerator InitializePoolsCoroutine()
    {
        yield return new WaitUntil(() => unitDataReader.IsDataLoaded);
        InitializePools();
    }

    private void InitializePools()
    {
        foreach(var unitPoolInfor in unitPoolInfors)
        {
            Queue<UnitBase> units = new Queue<UnitBase>();
            for( int i = 0; i < unitPoolInfor.poolSize; i++)
            {
                UnitBase unit = Instantiate(unitPoolInfor.unitPrefab, transform);
                UnitData unitData = unitDataReader.unitDataList.GetUnitData(unitPoolInfor.unitName);
                unit.InItUnit(unitData);
                unit.GetAnimation();
                unit.gameObject.SetActive(false);
                units.Enqueue(unit);
            }
            unitPool.Add(unitPoolInfor.unitName, units);
        }
    }

    // get unit from pool
    public Enemy GetEnemy(string unitName)
    {
        if(!unitPool.ContainsKey(unitName))
        {
            Debug.Log("there is no " + unitName);
            return null;
        }
        if(unitPool[unitName].Count > 0)
        {
            // Debug.Log("DEQUEUE STATE: enemy in pool count: " + unitPool[unitName].Count + " READY TO pooling enemy");
            Enemy enemy = unitPool[unitName].Dequeue() as Enemy;
            enemy.GetAnimation();
            // Debug.Log("DEQUEUE STATE: enemy in pool count: " + unitPool[unitName].Count + " POOLED enemy");
            enemy.gameObject.SetActive(true);
            return enemy;
        }
        else // Init unit if out of unit in pool
        {
            Debug.Log("init new enemy");
            Enemy unitPrefab = GetUnitPrefab(unitName) as Enemy;
            Enemy enemy = Instantiate(unitPrefab, transform);
            UnitData unitData = unitDataReader.unitDataList.GetUnitData(unitPrefab.unitName);
            enemy.InItUnit(unitData);
            enemy.GetAnimation();
           return enemy;
        }
    }

    public UnitBase GetSoldier(string unitName, Vector2 initPos)
    {
        if(!unitPool.ContainsKey(unitName))
        {
            Debug.Log("there is no " + unitName);
            return null;
        }
        if(unitPool[unitName].Count > 0)
        {
            UnitBase unit = unitPool[unitName].Dequeue();
            unit.transform.position = initPos;
            unit.gameObject.SetActive(true);
            return unit;
        }
        else // Init unit if out of unit in pool
        {
            UnitBase unitPrefab = GetUnitPrefab(unitName);
            UnitBase unit = Instantiate(unitPrefab, initPos, Quaternion.identity, transform);
            UnitData unitData = unitDataReader.unitDataList.GetUnitData(unitPrefab.unitName);
            unit.InItUnit(unitData);
            unit.GetAnimation();
           return unit;
        }
    }

    public void ReturnUnit(UnitBase unitBase)
    {
        unitBase.gameObject.SetActive(false);
        if(unitPool.ContainsKey(unitBase.unitName))
        {
            // Debug.Log("RELOAD STATE: enemy in pool count: " + unitPool[unitBase.unitName].Count + " READY TO reload");
            unitPool[unitBase.unitName].Enqueue(unitBase);
            // Debug.Log("RELOAD STATE: enemy in pool count: " + unitPool[unitBase.unitName].Count + " RELOADED");
        }
        unitBase.ResetUnit();
    }

    private UnitBase GetUnitPrefab(string unitName)
    {
        foreach(UnitPoolInfor unitPoolInfo in unitPoolInfors)
        {
            if(unitPoolInfo.unitName == unitName)
            {
                return unitPoolInfo.unitPrefab;
            }
        }
        return null;
    }

}
