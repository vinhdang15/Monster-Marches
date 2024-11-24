using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // Detect button click, sent button click information to GamePlayManager to proccess
    // Set up for initPanel, upgradePanel, checkSymbol visible
    [SerializeField] GameObject initPanel;
    [SerializeField] GameObject upgradePanel;
    [SerializeField] GameObject checkSmybol;
    public delegate void HandelTowerClick();
    public event HandelTowerClick OnInitArcherTower;
    public event HandelTowerClick OnInitMageTower;
    public event HandelTowerClick OnInitBarrackTower;
    public event HandelTowerClick OnInitCannonTower;
    public event HandelTowerClick OnTryToUpgradeTower;
    public event HandelTowerClick OnUpgradeTower;
    public event HandelTowerClick OnSellTower;
    private Button currentButton;

    public delegate void RaycastInputClickHandler();
    public event RaycastInputClickHandler OnRaycastHitNull;
    public event Action<TowerPresenter> OnSelectedTowerView;
    public event Action<EmptyPlot>OnSelectedEmptyPlot;

    // RaycastHit2D
    private Vector2 mousePos;
    private Vector2 worldPos;
    private RaycastHit2D hit;

    private void Update()
    {
        GetRaycastHit();
    }

    // Detect raycast hit null to hide Panel
    private void GetRaycastHit()
    {
        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            mousePos = Input.GetMouseButtonUp(0) ? Input.mousePosition : Input.GetTouch(0).position;
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            int layerMask = LayerMask.GetMask("TowerRaycast", "BarrackRange", "Button", "EmptyPlot");
            hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);

            if(hit.collider == null)
            {
                OnRaycastHitNull?.Invoke();
                HideInitPanel();
                HideUpgradePanel();
            }
            else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("EmptyPlot"))
            {
                ResetPanelState();
                EmptyPlot emptyPlot = hit.collider.gameObject.GetComponent<EmptyPlot>();
                OnSelectedEmptyPlot?.Invoke(emptyPlot);
            }
            else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("TowerRaycast"))
            {
                TowerPresenter selectedTower = hit.collider.gameObject.GetComponent<TowerPresenter>();
                OnSelectedTowerView?.Invoke(selectedTower);
            }
            else
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }

    #region INIT, UPGRADE PANEL, CHECKSYMBOL VISIBLE
    public void ShowInitPanel(Vector2 pos)
    {
        HideUpgradePanel();
        initPanel.transform.position = pos;
        initPanel.SetActive(true);
    }

    public void ShowUpgradePanel(Vector2 pos)
    {
        HideInitPanel();
        upgradePanel.transform.position = pos;
        upgradePanel.SetActive(true);
    }

    public void HideInitPanel()
    {
        if(initPanel.activeSelf)
        {
            ResetPanelState();
            initPanel.SetActive(false);
        }
    }

    public void HideUpgradePanel()
    {
        if(upgradePanel.activeSelf)
        {
            ResetPanelState();
            upgradePanel.SetActive(false);
        }
    }

    public void HidePanel()
    {
        HideInitPanel();
        HideUpgradePanel();
    }

    private void ShowCheckSymbol(Button clickedButton)
    {
        checkSmybol.transform.position = clickedButton.transform.transform.position;
        checkSmybol.SetActive(true);
    }

    private void HideCheckSymbol()
    {
        checkSmybol.SetActive(false);
    }

    private void ResetPanelState()
    {
        HideCheckSymbol();
        currentButton = null;
    }
    #endregion

    #region INIT && UPGRADE TOWER BUTTON
    private void OnButtonClick(Button clickedButton, HandelTowerClick eventAction)
    {
        if(currentButton != clickedButton)
        {
            ShowCheckSymbol(clickedButton);
        }
        else
        {
            eventAction?.Invoke();
        }
        currentButton = clickedButton;
    }

    // Init tower
    public void InitArcherTowerBtn(Button clickedButton)
    {
        OnButtonClick(clickedButton, OnInitArcherTower);
    }

    public void InitMageTowerBtn(Button clickedButton)
    {
        OnButtonClick(clickedButton, OnInitMageTower);
    }

    public void InitBarrackTowerBtn(Button clickedButton)
    {
        OnButtonClick(clickedButton, OnInitBarrackTower);
    }

    public void InitCannonTowerBtn(Button clickedButton)
    {
        OnButtonClick(clickedButton, OnInitCannonTower);
    }

    // Upgrade Tower
    public void UpgradeTower(Button clickedButton)
    {
        if(currentButton != clickedButton)
        {
            ShowCheckSymbol(clickedButton);
            OnTryToUpgradeTower?.Invoke();
        }
        else
        {
            OnUpgradeTower?.Invoke();
        }
        currentButton = clickedButton;
    }

    //sell Tower
    public void SellTower(Button clickedButton)
    {
        OnButtonClick(clickedButton, OnSellTower);
    } 
    #endregion
}
