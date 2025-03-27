using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnLoadMainMenu : LoadSceneBtnBase
{
    protected override void Start() 
    {
        sceneName = "MainMenuScene";
        base.Start();
    }
}
