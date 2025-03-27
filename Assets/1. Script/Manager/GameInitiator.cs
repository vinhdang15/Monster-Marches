using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    public static GameInitiator Instance { get; private set; }

    [Header("Load Data")]
    [SerializeField] private MapDataReader mapDataReader;
    [SerializeField] private EmptyPlotDataReader emptyPlotDataReader;
    [SerializeField] private TowerDataReader towerDataReader;
    [SerializeField] private BulletDataReader bulletDataReader;
    [SerializeField] private UnitDataReader unitDataReader;

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;

    [Header("GameManager")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private MapManager mapManager;
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

    [SerializeField] private SpawnGuardPointPath spawnGuardPointPath;
    [SerializeField] private EndPoint endPoint;

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
    public MapPresenter selectedMapPresenter;
    
    private GameObject gameManagerHolder;
    private GameObject poolManagerHolder;
    private GameObject mapDataHolder;
    private GameObject handlerHolder;

    private string sceneName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gameManagerHolder = CreateHolder("GameManagerHolder");
            poolManagerHolder = CreateHolder("PoolManagerHolder");
            mapDataHolder = CreateHolder("MapDataHolder");
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
        // yield return StartCoroutine(BindDataReader());
        
        yield return StartCoroutine(BindGameManagerObject());

        yield return StartCoroutine(BindCanvas());

        yield return StartCoroutine(PrepareGame());

        yield return StartCoroutine(InitializeGameObject());

        yield return StartCoroutine(ObjectGetInfor());

        RegisterEvent();

        LoadMapSelectionScene();
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

        emptyPlotDataReader = Instantiate(emptyPlotDataReader);

        towerDataReader = Instantiate(towerDataReader);

        bulletDataReader = Instantiate(bulletDataReader);

        unitDataReader = Instantiate(unitDataReader);

        mapManager = Instantiate(mapManager);

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

        spawnGuardPointPath = Instantiate(spawnGuardPointPath, mapDataHolder.transform);
        endPoint = Instantiate(endPoint, mapDataHolder.transform);

        bulletPool = Instantiate(bulletPool,poolManagerHolder.transform);
        unitPool = Instantiate(unitPool,poolManagerHolder.transform);

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
        panelManager.PrepareGame();
        inputButtonHandler.PrepareGame();
        raycastHandler.RaycastHandlerPrepareGame();

        barrackTowerManager.PrepareGame();
        bulletTowerManager.PrepareGame();
        bulletManager.PrepareGame();

        soldierManager.PrepareGame();
        enemyManager.PrepareGame();

        towerActionHandler.PrepareGame();
        enemySpawnerManager.PrepareGame();
        canvasManager.HideALLIU();
        yield return null;
    }

    private IEnumerator InitializeGameObject()
    {
        // bulletPool.Initialize();
        unitPool.Initialize();
        yield return null;
    }

    private IEnumerator ObjectGetInfor()
    {
        gamePlayManager.GetInfor();
        // enemySpawnerManager.GetInfor();
        panelManager.GetInfor();
        yield return null;
    }

    private void LoadMapSelectionScene()
    {
        SceneController.Instance.LoadScene("MapSelectionScene");
        mapManager.InitMapBtn();
    }

    private void RegisterEvent()
    {
        mapManager.OnLoadSelectedMap += HandleLoadSelectedMap;
        canvasManager.OnLoadMapSelectionClick += HandleReLoadSelectionMap;
    }

    private void HandleLoadSelectedMap(MapPresenter selectedMapPresenter)
    {
        Time.timeScale = 1;
        SceneController.Instance.LoadScene("PlaySelectedMapScene");
        CanvasManager.Instance.ShowFPSText();
        CanvasManager.Instance.ShowAllGamePlayIUList();
        fPSCounter.PrepareGame();

        this.selectedMapPresenter = selectedMapPresenter;
        int mapID = selectedMapPresenter.mapModel.MapID;
        emptyPlotManager.InitializeEmptyPlot(mapID);
    }

    private void HandleReLoadSelectionMap()
    {
        panelManager.HidePauseMenu();
        canvasManager.HideALLIU();
        emptyPlotManager.ClearEmptyPlot();
        bulletTowerManager.ClearBulletTowers();
        barrackTowerManager.ClearBarrackTowers();
        SceneController.Instance.LoadScene("MapSelectionScene");
        MapManager.Instance.ShowMapBtn();
    }
}
