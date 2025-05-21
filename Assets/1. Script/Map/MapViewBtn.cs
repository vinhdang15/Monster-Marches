using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapViewBtn : BtnBase
{
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] private Image image;

    [SerializeField] List<GameObject> starList;
    [SerializeField] GameObject starBackGround;
    public event Action<Button> OnMapBtnClick;


    protected override void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => OnMapBtnClick?.Invoke(thisButton));
    }

    public void PrepareGame()
    {
        GetComponents();
    }

    private void GetComponents()
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
        if(mapModel.Activate)
        {
            transform.gameObject.SetActive(true);
        }
        else transform.gameObject.SetActive(false);
    }

    public void UpdateMapBtnImage(MapModel mapModel)
    {
        if(mapModel.StarScore == 0) image.sprite = spriteList[0];
        else image.sprite = spriteList[1];
    }

    public IEnumerator TurnOnMapStar(int currentStarScore, int starScore)
    {
        if(currentStarScore >= starScore) yield break;
        
        for(int i = 0; i < starScore; i++)
        {
            yield return new WaitForSeconds(0.4f);
            starList[i].SetActive(true);
        }
    }

    public void SetDefaultTurnOnStar(int currentStarScore)
    {
        for(int i = 0; i < currentStarScore; i++)
        {
            starList[i].SetActive(true);
        }
        if(currentStarScore == 0)
        {
            starBackGround.SetActive(false);
        }
    }

    public void ShowStarBackGround()
    {
        starBackGround.SetActive(true);
    }
}
