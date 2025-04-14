using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBtn : BtnBase
{
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] private Image image;
    public event Action<Button> OnMapBtnClick;


    protected override void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => OnMapBtnClick?.Invoke(thisButton));
    }

    public void PrepareGame()
    {
        GetImage();
    }

    private void GetImage()
    {
        image = transform.gameObject.GetComponent<Image>();
    }

    public void UpdateMapInfo(MapModel mapModel)
    {
        CheckActiveMapBtn(mapModel);
        UpdateMapBtnImage(mapModel);
    }

    private void CheckActiveMapBtn(MapModel mapModel)
    {
        if(mapModel.Activate) transform.gameObject.SetActive(true);
        else transform.gameObject.SetActive(false);   
    }

    public void UpdateMapBtnImage(MapModel mapModel)
    {
        if(mapModel.StarPoint == 0) image.sprite = spriteList[0];
        else image.sprite = spriteList[1];
    }
}
