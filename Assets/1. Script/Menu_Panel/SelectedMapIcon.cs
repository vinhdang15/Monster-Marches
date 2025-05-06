using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedMapIcon : MonoBehaviour
{
    [SerializeField] Image image;

    public void LoadSelectedMapIcon(string mapID)
    {
        image.sprite = MapSpriteManager.GetMapSprite(mapID);
    }
}
