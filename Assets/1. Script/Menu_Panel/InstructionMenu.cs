using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionMenu : MenuBase
{
    [SerializeField] Image InstructionImage;
    [SerializeField] TextMeshProUGUI nextText;
    [SerializeField] List<GameObject> InstructionTextList = new();
    [SerializeField] GameObject skipButton;
    private int nextButtonClickCount = 0;

    public void ShowInstruction()
    {
        nextText.text = "NEXT";
        skipButton.SetActive(true);
        ShowInstructionPage1();
        Show();
    }

    // Button event
    public void HideInstruction()
    {
        nextText.text = "NEXT";
        nextButtonClickCount = 0;
        gameObject.SetActive(false);
    }

    // Button event
    public void NextButtonClick()
    {
        nextButtonClickCount++;
        if (nextButtonClickCount == 1)
        {
            ShowInstructionPage2();
            skipButton.SetActive(false);
            nextText.text = "GOT IT!";
        }
        else if (nextButtonClickCount == 2)
        {
            HideInstruction();
        }
    }

    private void ShowInstructionPage1()
    {
        GetImageCover(ImageName.Instruction_1.ToString());
        InstructionTextList[0].SetActive(true);
        InstructionTextList[1].SetActive(false);
    }

    private void ShowInstructionPage2()
    {
        GetImageCover(ImageName.Instruction_2.ToString());
        InstructionTextList[0].SetActive(false);
        InstructionTextList[1].SetActive(true);
    }

    private void GetImageCover(string image)
    {
        InstructionImage.sprite = SpriteManager.GetMapSprite(image);
    }
}
