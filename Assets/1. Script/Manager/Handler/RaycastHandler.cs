using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastHandler : MonoBehaviour
{
    private TowerActionHandler towerActionHandler;
    private bool isGuardPointBtnClicked = false;
    private Vector2 mousePos;
    private Vector2 worldPos;
    private RaycastHit2D hit;

    public event Action OnRaycastHitNull;
    public event Action<EmptyPlot> OnSelectedEmptyPlot;
    public event Action<TowerPresenter> OnSelectedBulletTower;
    public event Action<TowerPresenter> OnSelectedBarrackTower;
    public event Action<Vector2> OnSelectedNewGuardPointPos;

    public void PrepareGame()
    {
        LoadComponents();
        RegisterTowerActionHandlerEvent();
    }
    private void LoadComponents()
    {
        towerActionHandler = FindObjectOfType<TowerActionHandler>();
    }

    private void RegisterTowerActionHandlerEvent()
    {
        towerActionHandler.OnGuardPointBtnClick += HandleGuardPointBtnClick;
    }

    private void HandleGuardPointBtnClick()
    {
        isGuardPointBtnClicked = true;
    }

    private void Update()
    {
        GetRaycastHit();
    }

     private void GetRaycastHit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if (!isGuardPointBtnClicked)
            {
                IgnoreBarrackRangeDetect();
            }
            else
            {
                // execute order: button event => Update function.
                TakeBarrackRangeDetect();
            }
        }
    }

    public void GetWorldPos()
    {
        mousePos = Input.GetMouseButtonUp(0) ? Input.mousePosition : Input.GetTouch(0).position;
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void IgnoreBarrackRangeDetect()
    {
        GetWorldPos();

        int layerMask = LayerMask.GetMask("EmptyPlot", "BulletTowerRaycast", "BarrackTowerRaycast");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
            return;
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("EmptyPlot"))
        {
            EmptyPlot emptyPlot = hit.collider.gameObject.GetComponent<EmptyPlot>();
            OnSelectedEmptyPlot?.Invoke(emptyPlot);
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("BulletTowerRaycast"))
        {
            TowerPresenter selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            OnSelectedBulletTower?.Invoke(selectedTower);
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("BarrackTowerRaycast"))
        {
            TowerPresenter selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            OnSelectedBarrackTower?.Invoke(selectedTower);
        }
    }

    private void TakeButtonRaycastDetect()
    {
        GetWorldPos();
        int layerMask = LayerMask.GetMask("Button");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
        }
        return;
    }

    private void TakeBarrackRangeDetect()
    {
        GetWorldPos();
        int layerMask = LayerMask.GetMask("BarrackRangeDetect");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);

        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
            ResetIsGuardPointClicked();
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BarrackRangeDetect"))
        {
            ResetIsGuardPointClicked();
            OnSelectedNewGuardPointPos?.Invoke(worldPos);
        }
    }

    private void ResetIsGuardPointClicked()
    {
        isGuardPointBtnClicked = false;
    }
}
