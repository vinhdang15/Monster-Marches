using System.Collections;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    public static GameInitiator Instance { get; private set; }

    [Header("Check For Update")]
    [SerializeField] private UpdateAndDownload checkForUpdateAndDownload;

    [Header("Load Data")]
    [SerializeField] private MapDataReader mapDataReader;
    [SerializeField] private MapObjDataReader mapObjDataReader;
    [SerializeField] private WayPointDataReader wayPointDataReader;
    [SerializeField] private TowerDataReader towerDataReader;
    [SerializeField] private BulletDataReader bulletDataReader;
    [SerializeField] private BulletEffectDataReader bulletEffectDataReader;
    [SerializeField] private UnitDataReader unitDataReader;
    [SerializeField] private SkillDataReader skillDataReader;
    [SerializeField] private EnemyWaveDataReader enemyWaveDataReader;
    private SpriteDisplayController spriteController;

    [Header("FX")]
    [SerializeField] private DustFX dustFX;

    [Header("GameManager")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private MapBtnManager mapBtnManager;
    [SerializeField] private EndPointManager endPointManager;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private EmptyPlotManager emptyPlotManager;
    [SerializeField] private DecorObjectManager decorObjManager;

    [SerializeField] private BulletTowerManager bulletTowerManager;
    [SerializeField] private BarrackTowerManager barrackTowerManager;
    [SerializeField] private BulletManager bulletManager;
    [SerializeField] private SoldierManager soldierManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private EnemySpawnerManager enemySpawnerManager;
    [SerializeField] private DecorObjectPool decorObjPool;
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
    
    private GameObject dataReaderHolder;
    private GameObject gameManagerHolder;
    private GameObject handlerHolder;
    private GameObject poolHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            dataReaderHolder = CreateHolder("DataReaderHolder");
            gameManagerHolder = CreateHolder("GameManagerHolder");
            handlerHolder = CreateHolder("HandlerHolder");
            poolHolder = CreateHolder("PoolHolder");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(CheckForUpdateAndDownloadData());

        yield return StartCoroutine(JSONManagerPrepareGame());

        yield return StartCoroutine(SpriteControllerdPrepareGame());

        yield return StartCoroutine(BindGameObject());

        yield return StartCoroutine(BindCanvas());

        yield return StartCoroutine(PrepareGame());

        yield return StartCoroutine(InitializeGameObject());

        yield return StartCoroutine(LoadIntroScene());

        RegisterEvent();

        canvasManager.ShowStartGameMenu();
    }

    private IEnumerator CheckForUpdateAndDownloadData()
    {
        GameObject newGameObject = new("checkForUpdateAndDownload");
        checkForUpdateAndDownload = newGameObject.AddComponent<UpdateAndDownload>();

        bool isDone = false;
        checkForUpdateAndDownload.CheckForUpdateAndDownload(() => {isDone = true;});
        yield return new WaitUntil(() => isDone);
    }

    private IEnumerator JSONManagerPrepareGame()
    {
        var JSONPrepareDataTask = JSONManager.PrepareGameAsync();
        yield return new WaitUntil(() => JSONPrepareDataTask.IsCompleted);
    }

    private GameObject CreateHolder(string name)
    {
        GameObject holder = new GameObject(name);
        holder.transform.SetParent(this.transform);
        return holder;
    }
    
    private IEnumerator SpriteControllerdPrepareGame()
    {
        GameObject spriteDisplayController = new("SpriteDisplayController");
        spriteDisplayController.transform.SetParent(gameObject.transform);
        spriteController = spriteDisplayController.AddComponent<SpriteDisplayController>();

        var prepareSpriteTask = spriteController.PrepareGameAsync();
        yield return new WaitUntil(() => prepareSpriteTask.IsCompleted);
    }

    private IEnumerator BindGameObject()
    {
        sceneController = Instantiate(sceneController, dataReaderHolder.transform);

        mapDataReader = Instantiate(mapDataReader, dataReaderHolder.transform);

        mapObjDataReader = Instantiate(mapObjDataReader, dataReaderHolder.transform);

        wayPointDataReader = Instantiate(wayPointDataReader, dataReaderHolder.transform);

        towerDataReader = Instantiate(towerDataReader, dataReaderHolder.transform);

        bulletDataReader = Instantiate(bulletDataReader, dataReaderHolder.transform);
        
        bulletEffectDataReader = Instantiate(bulletEffectDataReader, dataReaderHolder.transform);

        unitDataReader = Instantiate(unitDataReader, dataReaderHolder.transform);

        skillDataReader = Instantiate(skillDataReader, dataReaderHolder.transform);

        enemyWaveDataReader = Instantiate(enemyWaveDataReader, dataReaderHolder.transform);

        mapBtnManager = Instantiate(mapBtnManager, gameManagerHolder.transform);

        endPointManager = Instantiate(endPointManager, gameManagerHolder.transform);

        dustFX = Instantiate(dustFX, gameManagerHolder.transform);

        gamePlayManager = Instantiate(gamePlayManager, gameManagerHolder.transform);

        decorObjManager = Instantiate(decorObjManager, gameManagerHolder.transform);

        emptyPlotManager = Instantiate(emptyPlotManager, gameManagerHolder.transform);

        bulletTowerManager = Instantiate(bulletTowerManager, gameManagerHolder.transform);

        barrackTowerManager = Instantiate(barrackTowerManager, gameManagerHolder.transform);
        
        bulletManager = Instantiate(bulletManager, gameManagerHolder.transform);

        soldierManager = Instantiate(soldierManager, gameManagerHolder.transform);

        enemyManager = Instantiate(enemyManager, gameManagerHolder.transform);

        enemySpawnerManager = Instantiate(enemySpawnerManager, gameManagerHolder.transform);

        cautionManager = Instantiate(cautionManager, gameManagerHolder.transform);

        // Object Pooling
        decorObjPool = Instantiate(decorObjPool, poolHolder.transform);
        bulletPool = Instantiate(bulletPool, poolHolder.transform);
        unitPool = Instantiate(unitPool, poolHolder.transform);
        visualEffectPool = Instantiate(visualEffectPool, poolHolder.transform);

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

    private IEnumerator PrepareGame()
    {
        gamePlayManager.PrepareGame(panelManager, enemyManager, raycastHandler,
                            towerActionHandler, enemySpawnerManager,
                            bulletTowerManager, barrackTowerManager,
                            dustFX);
        enemySpawnerManager.PrepareGame(enemyManager);
        decorObjManager.PrepareGame(enemyManager, gamePlayManager);
        dustFX.PrepareGame();
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
        decorObjPool.Initialize();
        unitPool.Initialize();
        bulletPool.Initialize();
        visualEffectPool.Initialize();
        yield return null;
    }

    private IEnumerator LoadIntroScene()
    {
        sceneController.LoadIntroScene();

        spriteController.LoadIntroSprite();
        CameraController.Instance.ResetBoundingShape(spriteController);
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
        spriteController.LoadWorldMapSprite();
        CameraController.Instance.ResetBoundingShape(spriteController);
        mapBtnManager.InitMapBtn();
    }

    private void HandleLoadSelectedMap(MapPresenter selectedMapPresenter)
    {
        currentMapData = selectedMapPresenter.mapModel.mapData;

        spriteController.LoadSelectedMapSprite(currentMapData);
        CameraController.Instance.ResetBoundingShape(spriteController);

        decorObjManager.InitializeDecorObj(currentMapData);
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

        panelManager.ShowInstructionMenu(mapBtnManager.HasActiveMapID2());
    }

    // reload world map after finished a game
    public void HandleFinishMap()
    {
        HandleQuitCurrentMap();
        mapBtnManager.UpdateMapPresenterInfo();
    }

    public void HandleQuitCurrentMap()
    {
        panelManager.HidePauseMenu();
        ClearCurrentMapObj();
        decorObjManager.ClearDecayObj();
        emptyPlotManager.ClearEmptyPlot();
        endPointManager.ClearEndPoints();
        
        spriteController.LoadWorldMapSprite();
        CameraController.Instance.ResetBoundingShape(spriteController);
        mapBtnManager.ShowMapBtn();
        sceneController.LoadWorldMapScene();
        Time.timeScale = 1;
        
    }

    public void HandleReloadCurrentMap()
    {
        ClearCurrentMapObj();
        enemySpawnerManager.GetInfor(currentMapData);
        decorObjManager.ResetDecayObjSprite();
        emptyPlotManager.ShowAllEmptyPlot();
        ObjGetMapInfor(currentMapData);
        panelManager.HidePauseMenu();
        panelManager.HideGameOverMenu();
        panelManager.ResetVictoryMenu();
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
        soldierManager.ClearActiveSoldierList();

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
