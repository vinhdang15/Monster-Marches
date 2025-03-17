using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBtnBase : BtnBase
{
    [SerializeField] protected string sceneName;

    protected override void OnButtonClick()
    {
        base.OnButtonClick();
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

    protected void ReLoadCurrentScene()
    {
        Time.timeScale = 1;
        sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
