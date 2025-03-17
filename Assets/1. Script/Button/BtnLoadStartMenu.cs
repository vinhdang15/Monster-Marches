using UnityEngine.UI;

public class BtnLoadStartMenu : LoadSceneBtnBase
{
    protected override void Start() 
    {
        sceneName = "StartMenuScene";
        base.Start();
    }
}