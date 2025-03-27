using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBase : MonoBehaviour
{
    [SerializeField] protected SoundEffectSO    soundEffectSO;
    protected Button        thisButton;
    protected virtual void Start()
    {
        thisButton = GetComponent<Button>();
        if(thisButton != null)
        {
           thisButton.onClick.AddListener(OnButtonClick);
        }
    }

    protected virtual void OnButtonClick()
    {
        PlayClickSound();
    }

    protected void QuitGame()
    {
        Application.Quit();
    }

    protected virtual void PlayClickSound()
    {
        AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
    }
    protected void PlayCustomSound(AudioClip customAudioClip)
    {
        AudioManager.Instance.PlaySound(customAudioClip);
    }
}
