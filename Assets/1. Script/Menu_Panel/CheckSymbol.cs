using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckSymbol : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] sprites;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void GreyOutCheckSymbol(Button button)
    {
        ButtonColor buttonColor = button.gameObject.GetComponent<ButtonColor>();
        if(buttonColor == null)
        {
            buttonImage.sprite = sprites[0];
            return;
        }
        else if (buttonColor.isGreyOut)
        {
            buttonImage.sprite = sprites[1];
        }
        else
        {
            buttonImage.sprite = sprites[0];
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
