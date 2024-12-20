using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Sound Effect", menuName = "Audio Config/Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    [Header("Next Wave Alarm")]
    public AudioClip comingWaveSound;
    [Header("Theme")]
    public List<AudioClip> Theme = new();

    [Header("Click Event")]
    public AudioClip clickSound;
    public AudioClip BuildSound;
    public AudioClip AddGoldSound;

    [Header("Weapons")]
    public AudioClip arrowSound;
    public AudioClip bomExplosionSound;
    public AudioClip bomWhistleSound;
    public AudioClip MagicBallHitSound;
    public AudioClip MagicBallWhistleSound;
    public List<AudioClip> Sword = new();

    [Header("Soldier")]
    public List<AudioClip> soldierDie;

    [Header("Enemy")]
    public List<AudioClip> monsterDie;
    public AudioClip endPointSound;

    

    public AudioClip GetRandomSoldierDie()
    {
        int index = Random.Range(0, soldierDie.Count);
        return soldierDie[index];
    }

    public AudioClip GetRandomMonsterDie()
    {
        int index = Random.Range(0, monsterDie.Count);
        return monsterDie[index];
    }

    public AudioClip GetRandomSwordSound()
    {
        int index = Random.Range(0, Sword.Count);
        return Sword[index];
    }

    public AudioClip GetThemeSound(int index)
    {
        return Theme[index];
    }
}
