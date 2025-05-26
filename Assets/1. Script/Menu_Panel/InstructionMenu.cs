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
    private int nextButtonClickCount = 1;

    public void ShowInstruction()
    {
        nextText.text = "NEXT!";
        skipButton.SetActive(true);
        ShowInstructionPage1();
        Show();
    }

    // Button event
    public void HideInstruction()
    {
        nextButtonClickCount = 0;
        gameObject.SetActive(false);
    }

    // Button event
    public void NextButtonClick()
    {
        nextButtonClickCount++;
        if (nextButtonClickCount == 2)
        {
            ShowInstructionPage2();
        }
        else if (nextButtonClickCount == 3)
        {
            ShowInstructionPage3();
        }
        else if (nextButtonClickCount == 4)
        {
            ShowInstructionPage4();
        }
        else if (nextButtonClickCount == 5)
        {
            ShowInstructionPage5();
            skipButton.SetActive(false);
            nextText.text = "GOT IT!";
        }
        else if (nextButtonClickCount == 6)
        {
            HideInstruction();
        }
    }

    private void ShowInstructionPage1()
    {
        GetImageCover(ImageName.Instruction_1.ToString());
        SetActiveTextPage(1);
        InstructionTextList[1].SetActive(false);
    }

    private void ShowInstructionPage2()
    {
        SetActiveTextPage(2);
    }

    private void ShowInstructionPage3()
    {
        GetImageCover(ImageName.Instruction_3.ToString());
        SetActiveTextPage(3);
    }

    private void ShowInstructionPage4()
    {
        GetImageCover(ImageName.Instruction_4.ToString());
        SetActiveTextPage(4);
    }

    private void ShowInstructionPage5()
    {
        GetImageCover(ImageName.Instruction_5.ToString());
        SetActiveTextPage(5);
    }

    private void GetImageCover(string image)
    {
        InstructionImage.sprite = SpriteManager.GetMapSprite(image);
    }

    private void SetActiveTextPage(int pageNumber)
    {
        for (int i = 0; i < InstructionTextList.Count; i++)
        {
            if (i + 1 == pageNumber) InstructionTextList[i].SetActive(true);
            else InstructionTextList[i].SetActive(false);
        }
    }
}
