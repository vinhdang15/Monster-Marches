using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    [SerializeField] TowerActionHandler towerActionHandler;
    [SerializeField] GamePlayManager gamePlayManager;
    private bool isGuardPointBtnClicked = false;
    private Vector2 mousePos;
    private Vector2 worldPos;
    private RaycastHit2D hit;
    private bool isMenuPanelOn = false;
    private bool allow = false;

    public event Action OnRaycastHitNull;
    public event Action<EmptyPlot> OnSelectedEmptyPlot;
    public event Action<TowerPresenter> OnSelectedBulletTower;
    public event Action<TowerPresenter> OnSelectedBarrackTower;
    // public event Action OnSelectedGuardPointBtnClick;
    public event Action<Vector2> OnSelectedNewGuardPointPos;

    public void RaycastHandlerPrepareGame()
    {
        LoadComponents();
        RegistertowerActionHandlerEvent();
    }
    private void LoadComponents()
    {
        towerActionHandler = FindObjectOfType<TowerActionHandler>();
    }

    private void RegistertowerActionHandlerEvent()
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
        // isMenuPanelOn là một biến bool private, xác định xem initMenu hoặc UpgradeMenu có đang bật hay không
        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if(!isGuardPointBtnClicked)
            {
                if(!isMenuPanelOn) IgnoreBarrackRangeDetect();
                else TakeButtonRaycastDetect();
            }
            else
            {
                // execute order: button event => Update function.

                // sau khi GuardPointBtn được nhấn, 
                // các hàm trong button event của nút này sẽ được xử lý trước
                // sau đó mới tới hàm Update, nhưng vì button event được xử lý trước
                // nên sẽ rơi vào trường hợp sau khi xử lý xong button event 
                // thì chương trình sẽ chay thẳng xuống hàm TakeBarrackRangeDetect()
                // và điều binh lính tới ngay vị trí nút vừa nhấn chứ không đợi input từ user

                // reset isMenuPanelOn bằng cách sử dụng dùng if - return để
                // ngăn chương trình chạy thẳng xuống TakeBarrackRangeDetect()

                if(isMenuPanelOn)
                {
                    isMenuPanelOn = false;
                    return;
                }
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
        isMenuPanelOn = true;
    }

    private void TakeButtonRaycastDetect()
    {
        GetWorldPos();
        int layerMask = LayerMask.GetMask("Button");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
            isMenuPanelOn = false;
        }
        // execute order: button event => Update function ( GetRaycastHit() in Update function)
        // after button function execute => menuPanel hide aka button hide => then GetRaycastHit() execute = null => isMenuPanelOn = false
        // don't need to write "isMenuPanelOn = false" to reset isMenuPanelOn after hide menuPanel
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
