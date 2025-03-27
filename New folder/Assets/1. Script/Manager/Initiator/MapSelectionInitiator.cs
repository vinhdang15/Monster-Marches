using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectionInitiator : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField] private MapManager mapManager;
    private void Start()
    {
        StartCoroutine(BindObjects());
    }

    private IEnumerator BindObjects()
    {
        mapManager = Instantiate(mapManager);
        yield return null;
    }
}
