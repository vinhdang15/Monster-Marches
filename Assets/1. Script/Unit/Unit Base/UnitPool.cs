using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    public static UnitPool Instance { get; private set; }
    private UnitDataReader  unitDataReader;
    private SkillDataReader skillDataReader;
    private Dictionary<string, GameObject> prefabDic = new();
    public Dictionary<string, Queue<UnitBase>> unitPool = new();
    private int poolSize = 20;

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

    public void PrepareGame(UnitDataReader unitDataReader, SkillDataReader skillDataReader)
    {
        this.unitDataReader = unitDataReader;
        this.skillDataReader = skillDataReader;
    }

    public void Initialize()
    {
        _ = PreloadAllUnit();
    }

    private async Task PreloadAllUnit()
    {
        await UnitPrefabManager.PreloadAllUnit();
        InitPrefabDic();
        InitPool();
    }

    private void InitPrefabDic()
    {
        prefabDic = new(UnitPrefabManager.GetUnitPrefabDic());
    }

    private void InitPool()
    {
        foreach(KeyValuePair<string, GameObject> pair in prefabDic)
        {
            Queue<UnitBase> unitQueue = new();
            for(int i = 0; i < poolSize; i++)
            {
                GameObject unitObj = Instantiate(pair.Value, transform);
                UnitBase unit = unitObj.GetComponent<UnitBase>();
                UnitData unitData = unitDataReader.unitDataListSO.GetUnitData(pair.Key);
                unit.InitUnit(unitData, skillDataReader);
                unit.GetAnimation();
                unit.gameObject.SetActive(false);
                unitQueue.Enqueue(unit);
            }
            unitPool.Add(pair.Key, unitQueue);
        }
    }

    // get unit from pool
    public UnitBase GetUnitBase(string unitID, Vector2 initPos = default)
    {
        if (unitID == UnitID.none.ToString()) return null;

        if (!unitPool.ContainsKey(unitID))
        {
            Debug.Log("there is no " + unitID);
            return null;
        }
        
        if(unitPool[unitID].Count > 0)
        {
            UnitBase unit = unitPool[unitID].Dequeue();
            unit.transform.position = initPos;
            unit.isDead = false;
            unit.gameObject.SetActive(true);
            return unit;
        }
        else // Init unit if out of unit in pool
        {
            UnitBase unitPrefabScript = GetUnitPrefabScript(unitID);
            UnitBase unit = Instantiate(unitPrefabScript, initPos, Quaternion.identity, transform);
            UnitData unitData = unitDataReader.unitDataListSO.GetUnitData(unit.UnitID);
            unit.InitUnit(unitData, skillDataReader);
            unit.GetAnimation();
           return unit;
        }
    }

    public void ReturnToUnitPool(UnitBase unit)
    {
        unit.ResetUnit();
        unit.gameObject.SetActive(false);
        if(unitPool.ContainsKey(unit.UnitID))
        {
            unitPool[unit.UnitID].Enqueue(unit);
        }
        
    }

    private UnitBase GetUnitPrefabScript(string unitID)
    {
        foreach(KeyValuePair<string, GameObject> pair in prefabDic)
        {
            if(pair.Key == unitID)
            {
                return pair.Value.GetComponent<UnitBase>();
            }
        }
        return null;
    }
}
