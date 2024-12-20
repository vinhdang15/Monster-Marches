using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] SoundEffectSO soundEffectSO;

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.Instance.PlaySound(soundEffectSO.endPointSound);
        IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
        if(enemy == null) return;
        enemy.OnReachEndPoint();
        Debug.Log("check");
    }
}
