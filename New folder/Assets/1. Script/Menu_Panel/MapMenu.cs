using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MapMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mapName;
    [SerializeField] TextMeshProUGUI mapDescription;

    public void Show(MapModel mapModel)
    {
        MapValue(mapModel);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void MapValue(MapModel mapModel)
    {
        gameObject.SetActive(true);
        mapName.text = mapModel.name.ToString();
        mapDescription.text = mapModel.Description.ToString();
    }

}
