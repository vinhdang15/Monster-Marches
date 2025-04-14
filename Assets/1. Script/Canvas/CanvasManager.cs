using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] GameObject loadingImage;
    [SerializeField] List<GameObject> IntroUIList;
    [SerializeField] List<GameObject> gamePlayIUList;
    [SerializeField] List<GameObject> ShowWhenInteractUIList;
    public event Action OnLoadWorldMapBtnClick;
    public event Action OnReloadWorldMapBtnClick;
    public event Action OnReloadCurrentMapBtnClick;

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

    public void HideLoadingImage()
    {
        loadingImage.SetActive(false);
    }

    public void HideAllUI()
    {
        HideIntroUIList();
        HideFPSText();
        HideInteractUIList();
        HideAllGamePlayIUList();
    }

    public void ShowIntroBtn()
    {
        foreach(GameObject i in IntroUIList)
        {
            i.SetActive(true);
        }
    }

    public void HideIntroUIList()
    {
        foreach(GameObject i in IntroUIList)
        {
            i.SetActive(false);
        }
    }

    public void HideAllGamePlayIUList()
    {
        foreach(GameObject i in gamePlayIUList)
        {
            i.SetActive(false);
        }
    }

    public void ShowAllGamePlayIUList()
    {
        foreach(GameObject i in gamePlayIUList)
        {
            i.SetActive(true);
        }
    }

    public void HideInteractUIList()
    {
        foreach(GameObject i in ShowWhenInteractUIList)
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

    public void HandleLoadMapSelectionBtnClick()
    {
        OnLoadWorldMapBtnClick?.Invoke();
    }

    public void HandleReLoadWorldMapBtnClick()
    {
        OnReloadWorldMapBtnClick?.Invoke();
    }

    public void HandleReloadCurrentMapBtnClick()
    {
        OnReloadCurrentMapBtnClick?.Invoke();
    }
}
