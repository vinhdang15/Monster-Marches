using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    // Detect button click, sent button click information to GamePlayManager to proccess
    [Header("TowerBuildingStatus")]
    [SerializeField] GameObject initPanel;
    [SerializeField] GameObject upgradePanel;
    [Header("TowerInitStatus")]
    [SerializeField] GameObject guardPointBtn;
    [SerializeField] GameObject checkSmybol;
    private Button currentButton = null;
    public bool isGuardPointSelected = false;
    public delegate void HandelTowerClick();
    public event HandelTowerClick OnInitArcherTower;
    public event HandelTowerClick OnInitMageTower;
    public event HandelTowerClick OnInitBarrackTower;
    public event HandelTowerClick OnInitCannonTower;
    public event HandelTowerClick OnTryToUpgradeTower;
    public event HandelTowerClick OnUpgradeTower;
    public event HandelTowerClick OnSellTower;
    public delegate void RaycastInputClickHandler();
    public event RaycastInputClickHandler OnRaycastHitNull;
    public event Action<EmptyPlot>OnSelectedEmptyPlot;
    public event Action<TowerPresenter> OnSelectedBulletTower;
    public event Action<TowerPresenter> OnSelectedBarrackTower;
    public event Action OnSelectedGuardPointBtnClick;
    public event Action<Vector2> OnSelectedNewGuardPointPos;

    // RaycastHit2D
    private Vector2 mousePos;
    private Vector2 worldPos;
    private RaycastHit2D hit;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] SoundEffectSO soundEffectSO;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetRaycastHit();
    }

    // Get VoeldPos
    public void GetWorldPos()
    {
        mousePos = Input.GetMouseButtonUp(0) ? Input.mousePosition : Input.GetTouch(0).position;
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);
    }

    // Detect raycast hit null to hide Panel
    private void GetRaycastHit()
    {
        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if(!isGuardPointSelected)
            {
                IgnoreBarrackRangeDetect();
            }
            else
            {
                TakeBarrackRangeDetect();
            }
        }
    }
    
    private void IgnoreBarrackRangeDetect()
    {
        GetWorldPos();

        int layerMask = LayerMask.GetMask("TowerRaycast", "Button", "EmptyPlot", "BarrackRaycast");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
            HideInitPanel();
            HideUpgradePanel();
            ShowGuardPointBtn(false);
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("EmptyPlot"))
        {
            ResetPanelState();
            EmptyPlot emptyPlot = hit.collider.gameObject.GetComponent<EmptyPlot>();
            OnSelectedEmptyPlot?.Invoke(emptyPlot);
            ShowInitPanel(emptyPlot.GetPos());
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("TowerRaycast"))
        {
            TowerPresenter selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            OnSelectedBulletTower?.Invoke(selectedTower);

            Vector2 SelectedTowerPos = selectedTower.towerView.GetPos();
            ShowUpgradePanel(SelectedTowerPos);
            ShowGuardPointBtn(false);
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("BarrackRaycast"))
        {
            TowerPresenter selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            OnSelectedBarrackTower?.Invoke(selectedTower);

            Vector2 SelectedTowerPos = selectedTower.towerView.GetPos();
            ShowUpgradePanel(SelectedTowerPos);
            ShowGuardPointBtn(true);
        }
    }

    private void TakeBarrackRangeDetect()
    {
        GetWorldPos();
        
        int layerMask = LayerMask.GetMask("Button", "BarrackRangeDetect");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);

        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
            HideInitPanel();
            HideUpgradePanel();
            ResetIsGuardPointSelected();
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            HideUpgradePanel();
            // execute order: button event => Update
            // to hit raycast button first then hit BarrackRangeDetect
            // then so BarrackRangeDetect after process button event
            // inform OnSelectedGuardPointBtnClic to GamePlayManager in here
            OnSelectedGuardPointBtnClick?.Invoke();
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BarrackRangeDetect"))
        {
            ResetIsGuardPointSelected();
            

            TowerPresenter selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            OnSelectedNewGuardPointPos?.Invoke(worldPos);

            //OnRaycastHitNull?.Invoke();
            // HideInitPanel();
            // HideUpgradePanel();
            // ResetPanelState();
        }
    }

    #region GUARD POINT
    // GuardPoint selected
    public void GuardPointBtn()
    {
        isGuardPointSelected = true;
    }

    private void ResetIsGuardPointSelected()
    {
        isGuardPointSelected = false;
    }

    private void ShowGuardPointBtn(bool show)
    {
        if(show) guardPointBtn.SetActive(true);
        else guardPointBtn.SetActive(false);
    }
    #endregion

    #region INIT, UPGRADE PANEL, CHECKSYMBOL VISIBLE
    public void ShowInitPanel(Vector2 pos)
    {
        HideUpgradePanel();
        initPanel.transform.position = pos;
        initPanel.SetActive(true);
    }

    public void HideInitPanel()
    {
        if(initPanel.activeSelf)
        {
            ResetPanelState();
            initPanel.SetActive(false);
        }
    }

    public void ShowUpgradePanel(Vector2 pos)
    {
        HideInitPanel();
        upgradePanel.transform.position = pos;
        upgradePanel.SetActive(true);
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
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
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
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            ShowCheckSymbol(clickedButton);
            OnTryToUpgradeTower?.Invoke();
        }
        else
        {
            OnUpgradeTower?.Invoke();
            return;
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
