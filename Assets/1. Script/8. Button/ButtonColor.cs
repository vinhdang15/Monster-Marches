using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] sprites;
    [SerializeField] TextMeshProUGUI goldText;
    public bool isGreyOut = false;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        if(transform.childCount > 0)
        {
            goldText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        buttonImage.sprite = sprites[0];
    }

    public void GreyOutButton(bool change)
    {
        if(change)
        {
            buttonImage.sprite = sprites[1];
            if(goldText == null) return;
            goldText.color = Color.black;
            isGreyOut = true;
        }
        else
        {
            buttonImage.sprite = sprites[0];
            if(goldText == null) return;
            goldText.color = Color.yellow;
            isGreyOut = false;
        }
    }
}
