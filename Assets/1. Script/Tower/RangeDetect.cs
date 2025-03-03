using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetect : MonoBehaviour
{
    private float spriteBoundInX;

    private void Awake()
    {
        spriteBoundInX = GetComponent<SpriteRenderer>().bounds.size.x/2;
        gameObject.SetActive(false);
    }

    public void SetSprtieIndicator(float targetRange)
    {   
        float rangeIndicatorScale = targetRange / spriteBoundInX;
        transform.localScale = new Vector3(rangeIndicatorScale, rangeIndicatorScale, 1);
    }
}
