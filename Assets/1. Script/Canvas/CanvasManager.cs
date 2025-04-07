using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] List<GameObject> gamePlayIUList;
    [SerializeField] List<GameObject> ShowWhenInteractUIList;

    public event Action OnLoadMapSelectionClick;
    public event Action OnReloadCurrentMapClick;

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

    public void HideAllUI()
    {
        HideFPSText();
        HideInteractUIList();
        HideAllGamePlayIUList();
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

    public void HandleLoadMapSelectionClick()
    {
        OnLoadMapSelectionClick?.Invoke();
    }

    public void HandleReloadCurrentMapClick()
    {
        OnReloadCurrentMapClick?.Invoke();
    }
}
