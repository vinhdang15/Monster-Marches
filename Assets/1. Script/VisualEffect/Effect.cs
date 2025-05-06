using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public string effectName;
    [SerializeField] List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    public void PlayEffect(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector2.zero;
        foreach(ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
        StartCoroutine(StopEffectAfterCompletion());
    }

    private IEnumerator StopEffectAfterCompletion()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            while (ps.isPlaying)
            {
                yield return null;
            }
        }
        ReturnEffectToPool();
    }

    public void ReturnEffectToPool()
    {
        foreach(ParticleSystem ps in particleSystems)
        {
            VisualEffectPool.Instance.ReturnEffect(this);
        }
    }
}
