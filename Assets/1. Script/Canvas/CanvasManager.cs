using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] List<GameObject> WorldMapSceneUI;
    [SerializeField] List<GameObject> gamePlaySceneUI;
    [SerializeField] List<GameObject> gamePlaySceneUIShowWhenInteract;
    [SerializeField] GameObject startGameMenu;
    [SerializeField] VictoryMenu victoryMenu;
    [SerializeField] GameObject saveGameMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PrepareGame()
    {
        ShowWorldMapUI();
        HideWorldMapSceneUI();
        ShowStartGameMenu();
        victoryMenu.PrepareGame();
    }

    private void ShowWorldMapUI()
    {
        HideStartGameMenu();
        HideSaveGameMenu();
        HideFPSText();
        HideInteractUIList();
        HideGamePlaySceneIUList();
        ShowWorldMapSceneUIList();
    }

    public void ShowSaveGameMenuHandler()
    {
        HideStartGameMenu();
        if(!JSONManager.HasSaveGameData())
        {
            saveGameMenu.transform.GetChild(1).gameObject.SetActive(true);
        }else
        {
            ShowSaveGameMenu();
        }
    }

    private void ShowSaveGameMenu()
    {
        foreach(Transform child in saveGameMenu.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void HideSaveGameMenu()
    {
        foreach(Transform child in saveGameMenu.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ShowStartGameMenu()
    {
        startGameMenu.SetActive(true);
    }

    public void HideStartGameMenu()
    {
        startGameMenu.SetActive(false);
    }

    public void HideGamePlaySceneIUList()
    {
        foreach(GameObject i in gamePlaySceneUI)
        {
            i.SetActive(false);
        }
    }

    public void ShowAllGamePlayIUList()
    {
        foreach(GameObject i in gamePlaySceneUI)
        {
            i.SetActive(true);
        }
    }

    public void HideInteractUIList()
    {
        foreach(GameObject i in gamePlaySceneUIShowWhenInteract)
        {
            i.SetActive(false);
        }
    }

    public void ShowWorldMapSceneUIList()
    {
        foreach(var i in WorldMapSceneUI)
        {
            i.SetActive(true);
        }
    }
    public void HideWorldMapSceneUI()
    {
        foreach(var i in WorldMapSceneUI)
        {
            i.SetActive(false);
        }
    }

    private void HideFPSText()
    {
        fpsText.gameObject.SetActive(false);
    }

    public void ShowFPSText()
    {
        fpsText.gameObject.SetActive(true);
    }

    public void HandleReloadIntroScene()
    {
        HideWorldMapSceneUI();
        ShowStartGameMenu();
        GameInitiator.Instance.HandleReloadIntroScene();
    }

    public void HandleLoadWorldMapBtnClick()
    {
        ShowWorldMapUI();
        GameInitiator.Instance.HandleLoadWorldMapScene();
    }

    public void HandleFinishGamepBtnClick()
    {
        ShowWorldMapUI();
        ResetVictoryMenuState();
        GameInitiator.Instance.HandleFinishMap();
    }

    public void HandleQuitCurrentMapBtnClick()
    {
        ShowWorldMapUI();
        GameInitiator.Instance.HandleQuitCurrentMap();
    }

    public void HandleReloadCurrentMapBtnClick()
    {
        GameInitiator.Instance.HandleReloadCurrentMap();
    }

    public void HandleSetNewGameBtnCLcik()
    {
        ShowWorldMapSceneUIList();
        GameInitiator.Instance.HandleSetNewGameBtnClick();
    }

    private void ResetVictoryMenuState()
    {
        victoryMenu.ResetState();
    }
}
