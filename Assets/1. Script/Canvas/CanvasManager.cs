using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] List<GameObject> gamePlayIUList;
    [SerializeField] List<GameObject> xxx;
    public static CanvasManager Instance { get; private set; }

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

    public void HideALLIU()
    {
        HideXXX();
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

    public void HideXXX()
    {
        foreach(GameObject i in xxx)
        {
            i.SetActive(false);
        }
    }
}
