using System;
using UnityEngine;

public class TowerViewBase : MonoBehaviour
{
    [SerializeField] protected RangeDetect        rangeDetection;
    [SerializeField] protected RangeDetect        rangeDetectionUpgrade;
    [SerializeField] protected TowerAnimation     towerAnimation;

    private CircleCollider2D            rangeDetectCol;
    private CircleCollider2D            rangeRaycastCol;
    
    public event Action<Enemy, TowerViewBase> OnEnemyEnter;
    public event Action<Enemy, TowerViewBase> OnEnemyExit;

    private void Awake()
    { 
        rangeDetectCol = GetComponent<CircleCollider2D>();
        rangeRaycastCol = GetComponent<CircleCollider2D>();
        towerAnimation = GetComponent<TowerAnimation>();
    }
    
    public void SetRangeRaycat(float rangeDetect)
    {
        rangeRaycastCol.radius = rangeDetect;
    }
    
    public void SetRangeDetect(float rangeDetect)
    {
        rangeDetectCol.radius = rangeDetect;
        rangeDetection.SetSprtieIndicator(rangeDetect);
    }
    
    public void SetRangeDetectUpgrade(float rangeDetect)
    {
        rangeDetectionUpgrade.SetSprtieIndicator(rangeDetect);
    }

    public void ShowRangeDetection(bool show)
    {
        if(show) rangeDetection.gameObject.SetActive(true);
        else rangeDetection.gameObject.SetActive(false);
    }

    public void ShowRangeDetectionUpgrade(bool show)
    {
        if(show) rangeDetectionUpgrade.gameObject.SetActive(true);
        else rangeDetectionUpgrade.gameObject.SetActive(false);
    }

    public Vector2 GetPos()
    {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            OnEnemyEnter?.Invoke(enemyScript, this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            OnEnemyExit?.Invoke(enemyScript, this);
        }
    }

    #region ANIMATION
    public void InitTowerAnimation()
    {
        // towerAnimation.SetTrigger("init");
    }
    #endregion
}
