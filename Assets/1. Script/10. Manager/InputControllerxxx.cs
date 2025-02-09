using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class InputControllerxxx : MonoBehaviour
{
    private Button archerBtn;
    private Button barrackBtn;
    private EmptyPlot emptyPlot;
    private TowerPresenter selectedTower;

    private Button currentButton = null;
    private bool isGuardPointSelected = false;

    public event Action<Button> OnFirstButtonClick;
    public event Action         OnButtonDoubleClick;

    public event Action<TowerType, EmptyPlot> OnTryToInitTower;
    public event Action<TowerType> OnInitTower;
    public event Action<TowerPresenter> OnTryToUpgradeTower;
    public event Action OnUpgradeTower;
    public event Action OnSellTower;
    
    public event Action OnRaycastHitNull;
    public event Action<EmptyPlot>OnSelectedEmptyPlot;
    public event Action<TowerPresenter> OnSelectedBulletTower;
    public event Action<TowerPresenter> OnSelectedBarrackTower;
    public event Action OnSelectedGuardPointBtnClick;
    public event Action<Vector2> OnSelectedNewGuardPointPos;

    // RaycastHit2D
    private Vector2 mousePos;
    private Vector2 worldPos;
    private RaycastHit2D hit;
    private TowerPresenter  currentTowerClick;
    private EmptyPlot       currentEptyPlot;
    private bool isMenuPanelOn = false;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] SoundEffectSO soundEffectSO;

    public void AddArcherBtn()
    {
        archerBtn = GameObject.Find("ArcherTowerBtn").GetComponent<Button>();
        archerBtn.onClick.AddListener(() => InitArcherTowerBtn(archerBtn));

        barrackBtn = GameObject.Find("BarrackTowerBtn").GetComponent<Button>();
        barrackBtn.onClick.AddListener(() => InitBarrackTowerBtn(barrackBtn));
    }

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
                if(!isMenuPanelOn) IgnoreBarrackRangeDetect();
                else TakeButtonRaycastDetect();
            }
            else
            {
                TakeBarrackRangeDetect();
            }
        }
    }
    
    private void TakeButtonRaycastDetect()
    {
        GetWorldPos();
        int layerMask = LayerMask.GetMask("Button");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if(hit.collider == null)
        {
            currentButton = null;
            OnRaycastHitNull?.Invoke();
            isMenuPanelOn = false;
        }
        // execute order: button event => Update function ( GetRaycastHit() in Update function)
        // after button function execute => menuPanel hide aka button hide => then GetRaycastHit() execute = null => isMenuPanelOn = false
        // don't need to write "isMenuPanelOn = false" to reset isMenuPanelOn after hide menuPanel
        return;
    }

    private void IgnoreBarrackRangeDetect()
    {
        GetWorldPos();

        int layerMask = LayerMask.GetMask("TowerRaycast", "EmptyPlot", "BarrackRaycast");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);
        if(hit.collider == null)
        {
            currentButton = null;
            OnRaycastHitNull?.Invoke();
            return;
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("EmptyPlot"))
        {
            emptyPlot = hit.collider.gameObject.GetComponent<EmptyPlot>();
            CheckCurrentEmptyPlot(emptyPlot);

            OnSelectedEmptyPlot?.Invoke(emptyPlot);
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("TowerRaycast"))
        {
            selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            CheckcurrentTowerClick(selectedTower);

            OnSelectedBulletTower?.Invoke(selectedTower);
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("BarrackRaycast"))
        {
            selectedTower = hit.collider.transform.parent.GetComponent<TowerPresenter>();
            CheckcurrentTowerClick(selectedTower);

            OnSelectedBarrackTower?.Invoke(selectedTower);
        }
        isMenuPanelOn = true;
    }

    private void TakeBarrackRangeDetect()
    {
        GetWorldPos();
        
        int layerMask = LayerMask.GetMask("Button", "BarrackRangeDetect");
        hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, layerMask);

        if(hit.collider == null)
        {
            OnRaycastHitNull?.Invoke();
            ResetIsGuardPointSelected();
        }
        else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
        {
            // execute order: button event => Update function ( GetRaycastHit() in Update function).
            // To hit raycast button first then hit BarrackRangeDetect
            // then show BarrackRangeDetect after process button event
            // inform OnSelectedGuardPointBtnClic to GamePlayManager in here
            OnSelectedGuardPointBtnClick?.Invoke();
        }
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BarrackRangeDetect"))
        {
            ResetIsGuardPointSelected();
            OnSelectedNewGuardPointPos?.Invoke(worldPos);
        }
    }

    private void CheckCurrentEmptyPlot(EmptyPlot emptyPlot)
    {
         if(currentEptyPlot != emptyPlot)
        {
            currentButton = null;
        }
        currentEptyPlot = emptyPlot;
    }

    private void CheckcurrentTowerClick(TowerPresenter selectedTower)
    {
        if(currentTowerClick != selectedTower)
        {
            currentButton = null;
        }
        currentTowerClick = selectedTower;
    }
    
    #region GUARD POINT
    // Button event
    // GuardPoint selected
    public void GuardPointBtn()
    {
        isGuardPointSelected = true;
    }

    private void ResetIsGuardPointSelected()
    {
        isGuardPointSelected = false;
    }
    #endregion

    #region INIT && UPGRADE TOWER BUTTON
    private void HandleInitBtnClick(Button clickedButton, TowerType towerType)
    {
        if(currentButton != clickedButton)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            OnTryToInitTower?.Invoke(towerType, emptyPlot);
            OnFirstButtonClick?.Invoke(clickedButton);
        }
        else
        {
            OnInitTower?.Invoke(towerType);
        }
        currentButton = clickedButton;
    }

    // Button event: Init tower
    public void InitArcherTowerBtn(Button clickedButton)
    {
        HandleInitBtnClick(clickedButton, TowerType.ArcherTower);      
    }

    public void InitMageTowerBtn(Button clickedButton)
    {
        HandleInitBtnClick(clickedButton, TowerType.MageTower);
    }

    public void InitBarrackTowerBtn(Button clickedButton)
    {
        HandleInitBtnClick(clickedButton, TowerType.Barrack);
    }

    public void InitCannonTowerBtn(Button clickedButton)
    {
        HandleInitBtnClick(clickedButton, TowerType.CannonTower);
    }

    // Button event: Upgrade Tower
    public void UpgradeTower(Button clickedButton)
    {
        if(currentButton != clickedButton)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            OnFirstButtonClick?.Invoke(clickedButton);
            OnTryToUpgradeTower?.Invoke(selectedTower);
        }
        else
        {
            OnUpgradeTower?.Invoke();
        }
        currentButton = clickedButton;
    }

    // Button event: Sell Tower
    public void SellTower(Button clickedButton)
    {
        if(currentButton != clickedButton)
        {
            AudioManager.Instance.PlaySound(soundEffectSO.clickSound);
            OnFirstButtonClick?.Invoke(clickedButton);
        }
        else
        {
            OnSellTower?.Invoke();
            ButtonDoubleClickAction();
            return;
        }
        currentButton = clickedButton;
    }

    public void ButtonDoubleClickAction()
    {
        OnButtonDoubleClick?.Invoke();
    }
    #endregion


    public int GetOnFirstButtonClickSubscriberCount()
    {
        return OnFirstButtonClick?.GetInvocationList().Length ?? 0;
    }
}
