using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DecorObjectPool : MonoBehaviour
{
    public static DecorObjectPool Instance { get; private set; }
    private Dictionary<string, GameObject> prefabDic = new();
    public int poolSize;
    public Dictionary<string, Queue<DecorObject>> decorObjectPool = new();

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
        _ = PreloadAllDecorObject();
    }

    private async Task PreloadAllDecorObject()
    {
        await DecorObjectPrefabManager.PreloadAllEnvironmentObjPrefalb();
        InitPrefabDic();
        InitPool();
    }

    private void InitPrefabDic()
    {
        prefabDic = new(DecorObjectPrefabManager.GetDecorObjectPrefabDic());
    }

    private void InitPool()
    {
        foreach(KeyValuePair<string, GameObject> pair in prefabDic)
        {
            Queue<DecorObject> queue = new();
            for(int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(pair.Value, transform);
                DecorObject EnvironmentObj = obj.GetComponent<DecorObject>();
                EnvironmentObj.decorID = pair.Key.ToString();
                obj.SetActive(false);
                queue.Enqueue(EnvironmentObj);
            }
            decorObjectPool.Add(pair.Key, queue);
        }
    }

    public DecorObject GetDecorObject(string decorID)
    {
        if(!decorObjectPool.ContainsKey(decorID))
        {
            Debug.Log("there is no " + decorID);
            return null;
        }
        
        if(decorObjectPool[decorID].Count > 0)
        {
            DecorObject obj = decorObjectPool[decorID].Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            DecorObject objPrefabScript = GetDecorObjectPrefabScript(decorID);
            DecorObject Obj = Instantiate(objPrefabScript, transform);
           return Obj;
        }
        
    }

    public void ReturnDecayObj(DecorObject decorObjectObj)
    {
        decorObjectObj.gameObject.SetActive(false);
        if(decorObjectPool.ContainsKey(decorObjectObj.decorID))
        {
            decorObjectPool[decorObjectObj.decorID].Enqueue(decorObjectObj);
        }
    }
    

    private DecorObject GetDecorObjectPrefabScript(string decorID)
    {
        foreach(KeyValuePair<string, GameObject> pair in prefabDic)
        {
            if(pair.Key == decorID)
            {
                return pair.Value.GetComponent<DecorObject>();
            }
        }
        return null;
    }
}
