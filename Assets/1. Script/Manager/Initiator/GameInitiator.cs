using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    public static GameInitiator Instance { get; private set; }

    [Header("Load Data")]
    [SerializeField] private MapDataReader mapDataReader;

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;

    [Header("GameManager")]
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private MapManager mapManager;
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

    [Header("LevelManager")]

    [Header("UI Canvas")]
    [SerializeField] private CanvasManager canvasManager;

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
        }
        else
        {
            Destroy(gameObject);
            return;
    }
        gameManagerHolder = CreateHolder("GameManagerHolder");
        poolManagerHolder = CreateHolder("PoolManagerHolder");
        mapDataHolder = CreateHolder("MapDataHolder");
        handlerHolder = CreateHolder("HandlerHolder");
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadDataReader());
        
        yield return StartCoroutine(BindGameManagerObject());

        yield return StartCoroutine(BindCanvas());

        yield return StartCoroutine(PrepareGame());

        yield return StartCoroutine(InitializeGameObject());

        yield return StartCoroutine(ObjectGetInfor());

        PlayMapSelectionScene();
    }

    private GameObject CreateHolder(string name)
    {
        GameObject holder = new GameObject(name);
        holder.transform.SetParent(this.transform);
        return holder;
    }
    private IEnumerator LoadDataReader()
    {
        mapDataReader = Instantiate(mapDataReader);
        yield return null;
    }

    private IEnumerator BindGameManagerObject()
    {
        // cameraController = Instantiate(cameraController);
        // cameraController.name = InitNameObject.Camera.ToString();

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

        barrackTowerManager.PrepareGame();
        bulletTowerManager.PrepareGame();
        bulletManager.PrepareGame();

        soldierManager.PrepareGame();
        enemyManager.PrepareGame();

        towerActionHandler.PrepareGame();
        enemySpawnerManager.PrepareGame();
        
        CanvasManager.Instance.HideALLIU();
        
        // fPSCounter.PrepareGame();
        yield return null;
    }

    private IEnumerator InitializeGameObject()
    {
        // bulletPool.Initialize();
        // unitPool.Initialize();
        yield return null;
    }

    private IEnumerator ObjectGetInfor()
    {
        // gamePlayManager.GetInfor();
        // enemySpawnerManager.GetInfor();
        yield return null;
    }

    private void PlayMapSelectionScene()
    {
        sceneName = "MapSelectionScene";
        if(!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            canvasManager.ShowAllGamePlayIUList();
            mapManager.InitMapBtn();
        }
        else
        {
            Debug.LogError("Scene name is not set.");
        }
    }
}
