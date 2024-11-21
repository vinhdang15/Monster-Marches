using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    [SerializeField] Transform      towerRaycastObject;
    [SerializeField] RangeDetect    rangeDetection;
    [SerializeField] RangeDetect    rangeDetectionUpgrade;
    [SerializeField] Transform      spawnBulletTrans;
    public List<BulletBase>         BulletList;
    private CircleCollider2D        rangeDetechCol;
    private CircleCollider2D        rangeRaycastCol;
    private Animator                towerAnimation;
    
    public delegate void    EnemyEnterHanlder(UnitBase enemy, TowerView view);
    public event            EnemyEnterHanlder OnEnemyEnter;
    public delegate void    EnemyExitHanlder(UnitBase enemy, TowerView view);
    public event            EnemyEnterHanlder OnEnemyExit;
    public delegate void    SelectedTowerViewHanlder(TowerView towerView);
    public event            SelectedTowerViewHanlder OnSelectedTowerView;

    private void Awake()
    { 
        rangeDetechCol = GetComponent<CircleCollider2D>();
        rangeRaycastCol = towerRaycastObject.GetComponent<CircleCollider2D>();
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

    private void OnMouseDown()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("TowerRaycast"));
        if(hit.collider != null ) OnSelectedTowerView?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            UnitBase enemyScript = other.GetComponent<UnitBase>();
            OnEnemyEnter?.Invoke(enemyScript, this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            UnitBase enemyScript = other.GetComponent<UnitBase>();
            OnEnemyExit?.Invoke(enemyScript, this);
        }
    }

    public Transform GetSpawnBulletTrans()
    {
        return spawnBulletTrans;
    }

    // public Transform GetFristEnemy()
    // {
    //     if(enemies.Count == 0) return null;
    //     return enemies[0].transform;
    // }

    #region ANIMATION
    public void InitTowerAnimation()
    {
        towerAnimation.SetTrigger("init");
    }
    public void AttackEnemyAnimation()
    {
        towerAnimation.SetTrigger("attack");
    }
    #endregion
}
