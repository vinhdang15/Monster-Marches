using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlaySoundTurnPitch(AudioSource audioSource ,AudioClip audioClip)
    {
        audioSource.pitch = Random.Range(0.95f,1.05f);
        audioSource.volume = Random.Range(0.8f,1.0f);
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }

    public IEnumerator PlaySoundAndWait(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
    }
}
