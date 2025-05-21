using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenUIManager : MonoBehaviour
{
    public static ScreenUIManager Instance { get; private set; }
    [SerializeField] FPSCounter fPSCounter;
    [SerializeField] List<GameObject> WorldMapSceneUI;
    [SerializeField] List<GameObject> gamePlaySceneUI;
    [SerializeField] List<GameObject> gamePlaySceneUIShowWhenInteract;
    [SerializeField] GameObject startGameMenu;
    [SerializeField] GameObject newGameMenu;
    [SerializeField] VictoryMenu victoryMenu;

    public void PrepareGame()
    {
        ShowWorldMapUI();
        HideWorldMapSceneUI();
        ShowStartGameMenu();
        victoryMenu.PrepareGame();
        fPSCounter.PrepareGame();
    }

    public void ShowWorldMapUI()
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
        if (!JSONManager.HasSaveGameData())
        {
            newGameMenu.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            ShowSaveGameMenu();
        }
    }

    private void ShowSaveGameMenu()
    {
        foreach(Transform child in newGameMenu.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void HideSaveGameMenu()
    {
        foreach(Transform child in newGameMenu.transform)
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
        fPSCounter.gameObject.SetActive(false);
    }

    public void ShowFPSText()
    {
        fPSCounter.gameObject.SetActive(true);
    }

    public void ResetVictoryMenuState()
    {
        victoryMenu.ResetState();
    }
}
