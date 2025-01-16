using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorController : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] sprites;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.sprite = sprites[0];
    }

    public void GreyOutButtonImage(bool change)
    {
        if(change)
        {
            buttonImage.sprite = sprites[1];
        }
        else
        {
            buttonImage.sprite = sprites[0];
        }
    }
}
