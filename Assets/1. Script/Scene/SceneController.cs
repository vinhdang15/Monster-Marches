using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] SoundEffectSO soundEffectSO;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadIntroScene()
    {
        LoadScene("IntroScene");
    }

    public void LoadWorldMapScene()
    {
        LoadScene("WorldMapScene");
    }

    public void LoadSelectedMapScene()
    {
        LoadScene("SelectedMapScene");
    }

    private void LoadScene(string sceneName)
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

    public void ReLoadCurrentScene()
    {
        Time.timeScale = 1;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public bool IsWorldMapScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "WorldMapScene") return true;
        else return false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "SelectedMapScene":
            AudioManager.Instance.PlayBackgroundMusic(soundEffectSO.Theme[0]);
            break;
        }
    }
}
