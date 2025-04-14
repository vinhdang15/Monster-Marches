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

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;
    private MapImageController mapImageController;

    [Header("GameManager")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private MapManager mapManager;
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
        yield return StartCoroutine(BindGameManagerObject());

        yield return StartCoroutine(BindCanvas());

        yield return StartCoroutine(PrepareGame());

        yield return StartCoroutine(InitializeGameObject());

        RegisterEvent();

        LoadIntroScene();
        canvasManager.HideLoadingImage();
        canvasManager.ShowIntroBtn();

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

        mapManager = Instantiate(mapManager);

        GameObject mapImage = new("MapImageController");
        mapImage.transform.SetParent(gameObject.transform);
        mapImageController = mapImage.AddComponent<MapImageController>();


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

    private IEnumerator PrepareGame()
    {
        gamePlayManager.PrepareGame();
        mapManager.PrepareGame();
        mapImageController.PrepareGame();
        cautionManager.PrepareGame();
        panelManager.PrepareGame();
        inputButtonHandler.PrepareGame();
        raycastHandler.RaycastHandlerPrepareGame();

        barrackTowerManager.PrepareGame();
        bulletTowerManager.PrepareGame();

        towerActionHandler.PrepareGame();
        canvasManager.HideAllUI();
        yield return null;
    }

    private IEnumerator InitializeGameObject()
    {
        unitPool.Initialize();
        bulletPool.Initialize();
        visualEffectPool.Initialize();
        yield return null;
    }

     private void LoadIntroScene()
    {
        SceneController.Instance.LoadIntroScene();

        mapImageController.LoadIntroSprite();
        CameraController.Instance.SetBoundingShape(mapImageController);
    }

    private void RegisterEvent()
    {
        mapManager.OnLoadSelectedMap += HandleLoadSelectedMap;
        canvasManager.OnLoadWorldMapBtnClick += HandleLoadWorldMapScene;
        canvasManager.OnReloadWorldMapBtnClick += HandleReloadWorldMapScene;
        canvasManager.OnReloadCurrentMapBtnClick += HandleReloadCurrentMap;
    }

     private void HandleLoadWorldMapScene()
    {
        CanvasManager.Instance.HideIntroUIList();
        SceneController.Instance.LoadWorldMapScene();
        mapImageController.LoadMapSelectionSprite();
        CameraController.Instance.SetBoundingShape(mapImageController);

        mapManager.InitMapBtn();
    }

    private void HandleLoadSelectedMap(MapPresenter selectedMapPresenter)
    {
        currentMapData = selectedMapPresenter.mapModel.mapData;

        mapImageController.LoadSelectedMapSprite(currentMapData);
        CameraController.Instance.SetBoundingShape(mapImageController);

        emptyPlotManager.InitializeEmptyPlot(currentMapData);
        endPointManager.CreateEndPoint(currentMapData);
        barrackTowerManager.InitializeGuardPointPosList(currentMapData);

        enemySpawnerManager.GetInfor(currentMapData);
        cautionManager.InitializeCautionBtn();

        ObjGetMapInfor(currentMapData);
        Time.timeScale = 1;

        SceneController.Instance.LoadSelectedMapScene();
        CanvasManager.Instance.ShowFPSText();
        CanvasManager.Instance.ShowAllGamePlayIUList();
        fPSCounter.PrepareGame();
    }

    private void HandleReloadWorldMapScene()
    {
        Time.timeScale = 1;
        ClearCurrentMapObj();
        emptyPlotManager.ClearEmptyPlot();
        endPointManager.ClearEndPoints();
        barrackTowerManager.ClearInitGuardPointposList();
        
        mapImageController.LoadMapSelectionSprite();
        CameraController.Instance.SetBoundingShape(mapImageController);

        PanelManager.Instance.HidePauseMenu();
        CanvasManager.Instance.HideAllUI();
        SceneController.Instance.LoadWorldMapScene();
        MapManager.Instance.ShowMapBtn();
        MapManager.Instance.UpdateMapPresenterInfo();

    }

    private void HandleReloadCurrentMap()
    {
        ClearCurrentMapObj();
        enemySpawnerManager.GetInfor(currentMapData);
        emptyPlotManager.ShowAllEmptyPlot();
        ObjGetMapInfor(currentMapData);
        PanelManager.Instance.HidePauseMenu();
        SceneController.Instance.ReLoadCurrentScene();
        cautionManager.InitializeCautionBtn();
    }

    private void ClearCurrentMapObj()
    {
        bulletTowerManager.ClearBulletTowers();
        barrackTowerManager.ClearBarrackTowers();
        soldierManager.ReturnAllSoldierToPool();
        enemyManager.ClearEnemyManager();
        enemySpawnerManager.ResetEnemySpawnerManager();
        cautionManager.ClearCautionBtnManager();
    }

    private void ObjGetMapInfor(MapData mapData)
    {
        gamePlayManager.GetInfor(mapData);
        panelManager.GetInfor();
    }
}
