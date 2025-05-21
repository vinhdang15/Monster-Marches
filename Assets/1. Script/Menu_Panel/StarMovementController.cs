using System.Collections.Generic;
using UnityEngine;

public class StarMovementController : MonoBehaviour
{
    // handle starScore
    // execute StarMovement
    // execute reset star state
    private int starScore = 0;
    [SerializeField] List<EndGameStarMovement> stars = new List<EndGameStarMovement>();
    private Vector2 oneStarPos;
    private List<Vector2> twoStarPos = new();
    private List<Vector2> threeStarPos = new();
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
            new(-50, -80),
            new(50, -80)
        };

        threeStarPos = new List<Vector2>()
        {
            new(-85, -80),
            new(0, -100),
            new(85, -80),
        };
    }

    public void PrepareGame()
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

    public void GetStarScore(int starScore)
    {
        this.starScore = starScore;
    }
}
