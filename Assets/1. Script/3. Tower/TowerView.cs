using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    [SerializeField] Transform      towerObject;
    [SerializeField] RangeDetect    rangeDetection;
    [SerializeField] RangeDetect    rangeDetectionUpgrade;
    [SerializeField] Transform      spawnBulletTrans;
    private CircleCollider2D        rangeDetechCol;
    private CircleCollider2D        rangeRaycastCol;
    private Animator                towerAnimation;
    public event            Action<Enemy, TowerView> OnEnemyEnter;
    public event            Action<Enemy, TowerView> OnEnemyExit;

    private void Awake()
    { 
        rangeDetechCol = towerObject.GetComponent<CircleCollider2D>();
        rangeRaycastCol = GetComponent<CircleCollider2D>();
        towerAnimation = towerObject.GetComponent<Animator>();
    }
    
    private void OnDisable()
    {
        OnEnemyEnter = null;
        OnEnemyExit = null;
    }
    public void SetRangeRaycat(float rangeDetect)
    {
        rangeRaycastCol.radius = rangeDetect;
    }
    
    public void SetRangeDetect(float rangeDetect)
    {
        rangeDetechCol.radius = rangeDetect;
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

    public Vector2 GetSpawnBulletPos()
    {
        return spawnBulletTrans.position;
    }

    #region ANIMATION
    public void InitTowerAnimation()
    {
        // towerAnimation.SetTrigger("init");
    }
    public void FireBulletAnimation()
    {
        // towerAnimation.SetTrigger("FireBullet");
    }
    #endregion
}
