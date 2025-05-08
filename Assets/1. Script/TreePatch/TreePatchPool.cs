using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TreePatchPool : MonoBehaviour
{
    public static TreePatchPool Instance { get; private set; }
    private Dictionary<string, GameObject> prefabDic = new();
    [SerializeField] TreePatch treePatchPrefab;
    public int poolSize;
    public Dictionary<string, Queue<TreePatch>> treePatchPool = new();

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
        _ = PreloadAllTreePatch();
    }

    private async Task PreloadAllTreePatch()
    {
        await TreePatchPrefabManager.PreloadAllTreePrefab();
        InitPrefabDic();
        InitPool();
    }

    private void InitPrefabDic()
    {
        prefabDic = new(TreePatchPrefabManager.GetTreePatchPrefabDic());
    }

    private void InitPool()
    {
        foreach(KeyValuePair<string, GameObject> pair in prefabDic)
        {
            Queue<TreePatch> treePatchQueue = new();
            for(int i = 0; i < poolSize; i++)
            {
                GameObject treePatchObj = Instantiate(pair.Value, transform);
                TreePatch treePatch = treePatchObj.GetComponent<TreePatch>();
                treePatch.treePatchID = pair.Key.ToString();
                treePatchObj.SetActive(false);
                treePatchQueue.Enqueue(treePatch);
            }
            treePatchPool.Add(pair.Key, treePatchQueue);
        }
    }

    public TreePatch GetTreePatch(string treePatchID)
    {
        if(!treePatchPool.ContainsKey(treePatchID))
        {
            Debug.Log("there is no " + treePatchID);
            return null;
        }
        
        if(treePatchPool[treePatchID].Count > 0)
        {
            TreePatch treePatch = treePatchPool[treePatchID].Dequeue();
            treePatch.gameObject.SetActive(true);
            return treePatch;
        }
        else
        {
            TreePatch treePatchObj = GetTreePatchPrefab(treePatchID);
            TreePatch treePatch = Instantiate(treePatchObj, transform);
           return treePatch;
        }
    }

    public void ReturnTreePatch(TreePatch treePatch)
    {
        treePatch.gameObject.SetActive(false);
        treePatch.SetDefaultSprite();
        if(treePatchPool.ContainsKey(treePatch.treePatchID))
        {
            treePatchPool[treePatch.treePatchID].Enqueue(treePatch);
        }
    }
    

    private TreePatch GetTreePatchPrefab(string treePatchID)
    {
        foreach(KeyValuePair<string, GameObject> pair in prefabDic)
        {
            if(pair.Key == treePatchID)
            {
                return pair.Value.GetComponent<TreePatch>();
            }
        }
        return null;
    }
}
