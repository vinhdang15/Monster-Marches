using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBase : MonoBehaviour
{
    [SerializeField] protected string           sceneName;
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
        LoadScene();     
    }

    protected virtual void LoadScene()
    {
        if(!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set.");
        }
    }

    protected void QuitGame()
    {
        Application.Quit();
    }

    protected void ReLoadCurrentScene()
    {
        sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
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
