using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarController : MonoBehaviour
{
    // handle starScore
    // execute StarMovement
    // execute reset star state
    public int starScore = 0;
    [SerializeField] List<StarMovement> stars = new List<StarMovement>();
    private Vector2 oneStarPos;
    private List<Vector2> twoStarPos = new List<Vector2>();
    private List<Vector2> threeStarPos = new List<Vector2>();
    private RectTransform rectTransform;

    private void LoadComponents()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void InitStarsEndPos()
    {
        oneStarPos = new Vector2(0, -80);
        twoStarPos = new List<Vector2>()
        {
            new Vector2(-50, -80),
            new Vector2(50, -80)
        };

        threeStarPos = new List<Vector2>()
        {
            new Vector2(-85, -80),
            new Vector2(0, -100),
            new Vector2(85, -80),
        };
    }

    public void StarControllerPrepareGame()
    {
        LoadComponents();
        InitStarsEndPos();
        foreach(var star in stars)
        {
            star.StarMovementPrepareGame();
        }
    }

    public void StarControllerResetState()
    {
        foreach(var star in stars)
        {
            star.ResetState();
        }
    }

    public void ActiveStars(float duration)
    {
        if(starScore == 0) return;
        switch (starScore)
        {
            case 1:
                stars[0].StarMove(oneStarPos,duration);
                break;
            case 2:
                HandleActiveStars(twoStarPos, duration);
                break;
            case 3:
                HandleActiveStars(threeStarPos, duration);
                break;
        }
    }

    private void HandleActiveStars(List<Vector2> posList, float duration)
    {
        for(int i = 0; i < starScore; i++)
        {
            stars[i].StarMove(posList[i],duration);
        }
    }

    public void SetAnchorPos(Vector2 anchorPos)
    {
        rectTransform.anchoredPosition = anchorPos;
    }

    public void SetStarScore(float lifePercentage)
    {
        if (lifePercentage < 60)
        {
            starScore = 1;
        }
        else if (lifePercentage < 95)
        {
            starScore = 2;
        }
        else
        {
            starScore = 3;
        }
    }
}
