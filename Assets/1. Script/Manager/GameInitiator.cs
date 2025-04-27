using System;
using System.Collections;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    public static GameInitiator Instance { get; private set; }

    [Header("Load Data")]
    [SerializeField] private MapDataReader mapDataReader;
    [SerializeField] private WayPointDataReader wayPointDataReader;
    [SerializeField] private TowerDataReader towerDataReader;
    [SerializeField] private BulletDataReader bulletDataReader;
    [SerializeField] private BulletEffectDataReader bulletEffectDataReader;
    [SerializeField] private UnitDataReader unitDataReader;
    [SerializeField] private SkillDataReader skillDataReader;
    [SerializeField] private EnemyWaveDataReader enemyWaveDataReader;
    private SpriteController spriteController;

    [Header("GameManager")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private MapBtnManager mapBtnManager;
    [SerializeField] private EndPointManager endPointManager;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private EmptyPlotManager emptyPlotManager;

    [SerializeField] private BulletTowerManager bulletTowerManager;
    [SerializeField] private BarrackTowerManager barrackTowerManager;
    [SerializeField] private BulletManager bulletManager;
    [SerializeField] private SoldierManager soldierManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private EnemySpawnerManager enemySpawnerManager;
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private UnitPool unitPool;
    [SerializeField] private VisualEffectPool visualEffectPool;

    [SerializeField] private PanelManager panelManager;
    [SerializeField] private CautionManager cautionManager;

    [Header("Handler")]
    [SerializeField] private FPSCounter fPSCounter;
    [SerializeField] private RaycastHandler raycastHandler;
    [SerializeField] private InputButtonHandler inputButtonHandler;
    [SerializeField] private TowerActionHandler towerActionHandler;

    [Header("UI Canvas")]
    [SerializeField] private CanvasManager canvasManager;
    
    [Header("SelectedMap")]
    public MapData currentMapData;
    
    private GameObject gameManagerHolder;
    private GameObject handlerHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gameManagerHolder = CreateHolder("GameManagerHolder");
            handlerHolder = CreateHolder("HandlerHolder");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(SpriteControllerdPrepareGame());
            
        yield return StartCoroutine(JSONManagerPrepareGame());

        yield return StartCoroutine(BindGameManagerObject());

        yield return StartCoroutine(BindCanvas());

        yield return StartCoroutine(PrepareGame());

        yield return StartCoroutine(InitializeGameObject());

        yield return StartCoroutine(LoadIntroScene());

        RegisterEvent();

        canvasManager.HideLoadingImage();
        canvasManager.ShowStartGameMenu();

    }

    private GameObject CreateHolder(string name)
    {
        GameObject holder = new GameObject(name);
        holder.transform.SetParent(this.transform);
        return holder;
    }

    private IEnumerator BindGameManagerObject()
    {
        // cameraController = Instantiate(cameraController);
        // cameraController.name = InitNameObject.Camera.ToString();

        sceneController = Instantiate(sceneController);

        mapDataReader = Instantiate(mapDataReader);

        wayPointDataReader = Instantiate(wayPointDataReader);

        towerDataReader = Instantiate(towerDataReader);

        bulletDataReader = Instantiate(bulletDataReader);
        
        bulletEffectDataReader = Instantiate(bulletEffectDataReader);

        unitDataReader = Instantiate(unitDataReader);

        skillDataReader = Instantiate(skillDataReader);

        enemyWaveDataReader = Instantiate(enemyWaveDataReader);

        mapBtnManager = Instantiate(mapBtnManager);

        endPointManager = Instantiate(endPointManager, gameManagerHolder.transform);

        gamePlayManager = Instantiate(gamePlayManager, gameManagerHolder.transform);
        gamePlayManager.name = InitNameObject.GamePlayManager.ToString();

        emptyPlotManager = Instantiate(emptyPlotManager, gameManagerHolder.transform);
        emptyPlotManager.name = InitNameObject.EmptyPlotManager.ToString();

        bulletTowerManager = Instantiate(bulletTowerManager, gameManagerHolder.transform);
        bulletTowerManager.name = InitNameObject.BulletTowerManager.ToString();

        barrackTowerManager = Instantiate(barrackTowerManager, gameManagerHolder.transform);
        barrackTowerManager.name = InitNameObject.BarrackTowerManager.ToString();
        
        bulletManager = Instantiate(bulletManager, gameManagerHolder.transform);
        bulletManager.name = InitNameObject.BulletManager.ToString();

        soldierManager = Instantiate(soldierManager, gameManagerHolder.transform);
        soldierManager.name = InitNameObject.SoldierManager.ToString();

        enemyManager = Instantiate(enemyManager, gameManagerHolder.transform);
        enemyManager.name = InitNameObject.EnemyManager.ToString();

        enemySpawnerManager = Instantiate(enemySpawnerManager, gameManagerHolder.transform);

        cautionManager = Instantiate(cautionManager, gameManagerHolder.transform);
        cautionManager.name = InitNameObject.CautionManager.ToString();

        bulletPool = Instantiate(bulletPool);
        unitPool = Instantiate(unitPool);
        visualEffectPool = Instantiate(visualEffectPool);

        panelManager = Instantiate(panelManager, gameManagerHolder.transform);

        // Handler
        fPSCounter = Instantiate(fPSCounter, handlerHolder.transform);
        raycastHandler = Instantiate(raycastHandler, handlerHolder.transform);
        inputButtonHandler = Instantiate(inputButtonHandler, handlerHolder.transform);
        towerActionHandler = Instantiate(towerActionHandler, handlerHolder.transform);
        
        yield return null;
    }

    private IEnumerator BindCanvas()
    {
        canvasManager = Instantiate(canvasManager);
        yield return null;
    }

    private IEnumerator JSONManagerPrepareGame()
    {
        var JSONPrepareDataTask = JSONManager.PrepareGameAsync();
        yield return new WaitUntil(() =>JSONPrepareDataTask.IsCompleted);
    }

    private IEnumerator SpriteControllerdPrepareGame()
    {
        GameObject mapImageControllerObj = new("MapImageController");
        mapImageControllerObj.transform.SetParent(gameObject.transform);
        spriteController = mapImageControllerObj.AddComponent<SpriteController>();

        var prepareSpriteTask = spriteController.PrepareGameAsync();
        yield return new WaitUntil(() => prepareSpriteTask.IsCompleted);
    }

    private IEnumerator PrepareGame()
    {
        gamePlayManager.PrepareGame();
        mapBtnManager.PrepareGame();
        mapDataReader.PrepareGame();
        cautionManager.PrepareGame();
        panelManager.PrepareGame();
        inputButtonHandler.PrepareGame();
        raycastHandler.RaycastHandlerPrepareGame();
        barrackTowerManager.PrepareGame();
        bulletTowerManager.PrepareGame();
        towerActionHandler.PrepareGame();
        canvasManager.PrepareGame();
        yield return null;
    }

    private IEnumerator InitializeGameObject()
    {
        unitPool.Initialize();
        bulletPool.Initialize();
        visualEffectPool.Initialize();
        yield return null;
    }

    private IEnumerator LoadIntroScene()
    {
        sceneController.LoadIntroScene();

        spriteController.LoadIntroSprite();
        CameraController.Instance.SetBoundingShape(spriteController);
        yield return null;
    }

    private void RegisterEvent()
    {
        mapBtnManager.OnLoadSelectedMap += HandleLoadSelectedMap; 
    }

    public void HandleReloadIntroScene()
    {
        spriteController.LoadIntroSprite();
        mapBtnManager.HideMapBtn();
    }

    public void HandleLoadWorldMapScene()
    {
        canvasManager.HideSaveGameMenu();
        sceneController.LoadWorldMapScene();
        spriteController.LoadMapSelectionSprite();
        CameraController.Instance.SetBoundingShape(spriteController);
        mapBtnManager.InitMapBtn();
    }

    private void HandleLoadSelectedMap(MapPresenter selectedMapPresenter)
    {
        
        currentMapData = selectedMapPresenter.mapModel.mapData;

        spriteController.LoadSelectedMapSprite(currentMapData);
        CameraController.Instance.SetBoundingShape(spriteController);

        emptyPlotManager.InitializeEmptyPlot(currentMapData);
        endPointManager.CreateEndPoint(currentMapData);
        barrackTowerManager.InitializeGuardPointPosList(currentMapData);

        enemySpawnerManager.GetInfor(currentMapData);
        cautionManager.InitializeCautionBtn();

        ObjGetMapInfor(currentMapData);
        Time.timeScale = 1;

        sceneController.LoadSelectedMapScene();
        canvasManager.HideWorldMapSceneUI();
        canvasManager.ShowFPSText();
        canvasManager.ShowAllGamePlayIUList();
        fPSCounter.PrepareGame();
    }

    // reload world map after finished a game
    public void HandleFinishMap()
    {
        HandleQuitCurrentMap();
        mapBtnManager.UpdateMapPresenterInfo();
    }

    public void HandleQuitCurrentMap()
    {
        Time.timeScale = 1;
        ClearCurrentMapObj();
        emptyPlotManager.ClearEmptyPlot();
        endPointManager.ClearEndPoints();
        
        spriteController.LoadMapSelectionSprite();
        CameraController.Instance.SetBoundingShape(spriteController);

        panelManager.HidePauseMenu();
        sceneController.LoadWorldMapScene();
        mapBtnManager.ShowMapBtn();
    }

    public void HandleReloadCurrentMap()
    {
        ClearCurrentMapObj();
        enemySpawnerManager.GetInfor(currentMapData);
        emptyPlotManager.ShowAllEmptyPlot();
        ObjGetMapInfor(currentMapData);
        panelManager.HidePauseMenu();
        sceneController.ReLoadCurrentScene();
        cautionManager.InitializeCautionBtn();
    }

    public void HandleSetNewGameBtnClick()
    {
        canvasManager.ShowWorldMapSceneUIList();
        StartCoroutine(ResetMapProgressDataCoroutine());
    }

    private IEnumerator ResetMapProgressDataCoroutine()
    {
        JSONManager.SetHasSaveGameData(true);
        Debug.Log($"newgame click: {JSONManager.HasSaveGameData()}");
        
        yield return StartCoroutine(JSONManager.ResetMapProgressData());
        Debug.Log("START RE INIT MAP BTN");
        mapBtnManager.CLearAllMapBtn();
        mapDataReader.ResetFullMapData();
        HandleLoadWorldMapScene();
    }

    private void ClearCurrentMapObj()
    {
        bulletTowerManager.ClearBulletTowers();

        barrackTowerManager.ClearBarrackTowers();
        soldierManager.ClearSoldierManager();

        soldierManager.ReturnAllSoldierToPool();
        enemyManager.ResetEnemyManager();
        
        enemySpawnerManager.ResetEnemySpawnerManager();

        cautionManager.ClearCautionBtnManager();
    }

    private void ObjGetMapInfor(MapData mapData)
    {
        gamePlayManager.GetInfor(mapData);
        panelManager.GetInfor();
    }
}
