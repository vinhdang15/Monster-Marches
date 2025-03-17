using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasWorldSpace : MonoBehaviour
{
    public static CanvasWorldSpace Instance { get; private set; }

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
}
