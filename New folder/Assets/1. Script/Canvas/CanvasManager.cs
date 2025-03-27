using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] List<GameObject> gamePlayIUList;
    [SerializeField] List<GameObject> ShowWhenInteractUIList;
    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // HideALLIU();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void HideALLIU()
    {
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
}
