using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectPool : MonoBehaviour
{
    public static VisualEffectPool Instance { get; private set; }
    [System.Serializable]
    public class VisualEffectInfor
    {
        public Effect effectPrefab;
        public string EffectName => effectPrefab.name;
        public int poolSize;
    }

    [SerializeField] List<VisualEffectInfor> effectPoolInfors = new List<VisualEffectInfor>();
    [SerializeField] Dictionary<string, Queue<Effect>> effectPool = new Dictionary<string, Queue<Effect>>();

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
        InitializeEffectPool();
    }

    private void InitializeEffectPool()
    {
        foreach( var effectPoolInfor in effectPoolInfors)
        {
            Queue<Effect> effectQueue = new Queue<Effect>();
            for(int i = 0; i< effectPoolInfor.poolSize; i++)
            {
                Effect effect = Instantiate(effectPoolInfor.effectPrefab, transform);
                effect.effectName = effectPoolInfor.EffectName;
                effectQueue.Enqueue(effect);
            }
            effectPool.Add(effectPoolInfor.EffectName, effectQueue);
        }
    }

    public Effect GetEffect(string effectName)
    {
        if(!effectPool.ContainsKey(effectName))
        {
            Debug.Log("there is no " + effectName);
            return null;
        }
        
        if(effectPool[effectName].Count > 0)
        {
            Effect effect = effectPool[effectName].Dequeue();
            effect.gameObject.SetActive(true);
            return effect;
        }
        else
        {
            return GetEffectPrefab(effectName);
        }
    }

    public void ReturnEffect(Effect effect)
    {
        effect.gameObject.SetActive(false);
        effect.transform.SetParent(transform);
        effectPool[effect.effectName].Enqueue(effect);
    }
    

    private Effect GetEffectPrefab(string effectName)
    {
        foreach(VisualEffectInfor effectPoolInfor in effectPoolInfors)
        {
            if(effectPoolInfor.EffectName == effectName)
            {
                Effect effect = Instantiate(effectPoolInfor.effectPrefab,transform);
                effect.effectName = effectPoolInfor.EffectName;
                return effect;
            }
        }
        return null;
    }
}
