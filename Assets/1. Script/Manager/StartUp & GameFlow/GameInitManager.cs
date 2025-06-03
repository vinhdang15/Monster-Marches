using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitManager : MonoBehaviour
{
    [Header("Check For Update")]
    [SerializeField] private UpdateAndDownload checkForUpdateAndDownload;

    [Header("Data Reader")]
    [SerializeField] private MapDataReader mapDataReader;
    [SerializeField] private DecorObjDataReader decorObjDataReader;
    [SerializeField] private WayPointDataReader wayPointDataReader;
    [SerializeField] private TowerDataReader towerDataReader;
    [SerializeField] private BulletDataReader bulletDataReader;
    [SerializeField] private BulletEffectDataReader bulletEffectDataReader;
    [SerializeField] private UnitDataReader unitDataReader;
    [SerializeField] private SkillDataReader skillDataReader;
    [SerializeField] private EnemyWaveDataReader enemyWaveDataReader;
    private SpriteDisplayController spriteDisplayController;

    [Header("Game Manager")]
    [SerializeField] private SceneController sceneController;
    [SerializeField] private MapManager mapManager;
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
    [SerializeField] private CautionManager cautionManager;

    [Header("FX")]
    [SerializeField] private DustFX dustFX;

    [Header("Handler")]
    [SerializeField] private RaycastHandler raycastHandler;
    [SerializeField] private InputButtonHandler inputButtonHandler;
    [SerializeField] private TowerActionHandler towerActionHandler;

    [Header("UI Manager")]
    [SerializeField] private GamePlayUIManager gamePlayUIManager;
    [SerializeField] private ScreenUIManager screenUIManager;

    [Header("Pool Manager")]
    [SerializeField] private VisualEffectPool visualEffectPool;
    [SerializeField] private DecorObjectPool decorObjPool;
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private UnitPool unitPool;

    // Getter
    public MapDataReader            GetMapDataReader() => mapDataReader;
    public SpriteDisplayController  GetSpriteDisplayController() => spriteDisplayController;
    public SceneController          GetSceneController() => sceneController;
    public MapManager               GetMapManager() => mapManager;
    public EndPointManager          GetEndPointManager() => endPointManager;
    public GamePlayManager          GetGamePlayManager() => gamePlayManager;
    public EmptyPlotManager         GetEmptyPlotManager() => emptyPlotManager;
    public DecorObjectManager       GetDecorObjManager() => decorObjManager;
    public BulletTowerManager       GetBulletTowerManager() => bulletTowerManager;
    public BarrackTowerManager      GetBarrackTowerManager() => barrackTowerManager;
    public BulletManager            GetBulletManager() => bulletManager;
    public SoldierManager           GetSoldierManager() => soldierManager;
    public EnemyManager             GetEnemyManager() => enemyManager;
    public EnemySpawnerManager      GetEnemySpawnerManager() => enemySpawnerManager;
    public CautionManager           GetCautionManager() => cautionManager;
    public GamePlayUIManager        GetGamePlayUIManager() => gamePlayUIManager;
    public ScreenUIManager          GetScreenUIManager() => screenUIManager;

    public IEnumerator CheckForUpdateAndDownloadData()
    {
        GameObject newGameObject = new("CheckForUpdateAndDownload");
        checkForUpdateAndDownload = newGameObject.AddComponent<UpdateAndDownload>();

        bool isDone = false;
        checkForUpdateAndDownload.CheckForUpdateAndDownload(() => { isDone = true; });
        yield return new WaitUntil(() => isDone);
    }

    public IEnumerator PrepareJsonData()
    {
        var task = JSONDataLoader.PrepareGameAsync();
        yield return new WaitUntil(() => task.IsCompleted);
    }

    public IEnumerator InitSpriteController(Transform parent)
    {
        GameObject spriteGO = new("SpriteDisplayController");
        spriteGO.transform.SetParent(parent);

        spriteDisplayController = spriteGO.AddComponent<SpriteDisplayController>();
        var prepareSpriteTask = spriteDisplayController.PrepareGameAsync();
        yield return new WaitUntil(() => prepareSpriteTask.IsCompleted);
    }

    public IEnumerator Init(Transform dataHolder, Transform managerHolder,
                            Transform handlerHolder, Transform uIHolder)                 
    {
        // Data init
        mapDataReader           = Instantiate(mapDataReader, dataHolder);
        decorObjDataReader      = Instantiate(decorObjDataReader, dataHolder);
        wayPointDataReader      = Instantiate(wayPointDataReader, dataHolder);
        towerDataReader         = Instantiate(towerDataReader, dataHolder);
        bulletDataReader        = Instantiate(bulletDataReader, dataHolder);
        bulletEffectDataReader  = Instantiate(bulletEffectDataReader, dataHolder);
        unitDataReader          = Instantiate(unitDataReader, dataHolder);
        skillDataReader         = Instantiate(skillDataReader, dataHolder);
        enemyWaveDataReader     = Instantiate(enemyWaveDataReader, dataHolder);
        yield return null;

        // Game manager init
        mapManager              = Instantiate(mapManager, managerHolder);
        gamePlayManager         = Instantiate(gamePlayManager, managerHolder);
        decorObjManager         = Instantiate(decorObjManager, managerHolder);
        emptyPlotManager        = Instantiate(emptyPlotManager, managerHolder);
        endPointManager         = Instantiate(endPointManager, managerHolder);
        bulletTowerManager      = Instantiate(bulletTowerManager, managerHolder);
        barrackTowerManager     = Instantiate(barrackTowerManager, managerHolder);
        bulletManager           = Instantiate(bulletManager, managerHolder);
        soldierManager          = Instantiate(soldierManager, managerHolder);
        enemyManager            = Instantiate(enemyManager, managerHolder);
        enemySpawnerManager     = Instantiate(enemySpawnerManager, managerHolder);
        cautionManager          = Instantiate(cautionManager, managerHolder);
        dustFX                  = Instantiate(dustFX, managerHolder);
        sceneController         = Instantiate(sceneController, managerHolder);

        // Handler init
        raycastHandler          = Instantiate(raycastHandler, handlerHolder);
        inputButtonHandler      = Instantiate(inputButtonHandler, handlerHolder);
        towerActionHandler      = Instantiate(towerActionHandler, handlerHolder);

        // UI manager init
        gamePlayUIManager       = Instantiate(gamePlayUIManager, uIHolder);
        screenUIManager         = Instantiate(screenUIManager, uIHolder);

        // Pool init
        unitPool                = Instantiate(unitPool);
        bulletPool              = Instantiate(bulletPool);
        decorObjPool            = Instantiate(decorObjPool);
        visualEffectPool        = Instantiate(visualEffectPool);

        yield return null;
    }

    public IEnumerator PrepareGame()
    {
        // data prepare game
        mapDataReader.PrepareGame();
        decorObjDataReader.PrepareGame();
        wayPointDataReader.PrepareGame();
        towerDataReader.PrepareGame();
        bulletDataReader.PrepareGame();
        bulletEffectDataReader.PrepareGame();
        unitDataReader.PrepareGame();
        skillDataReader.PrepareGame();
        enemyWaveDataReader.PrepareGame();

        // Game manager prepare game
        gamePlayManager.PrepareGame(towerDataReader, gamePlayUIManager, mapManager, enemyManager,
                                    raycastHandler, towerActionHandler,
                                    enemySpawnerManager, bulletTowerManager,
                                    barrackTowerManager, dustFX);
        enemySpawnerManager.PrepareGame(enemyManager, wayPointDataReader, enemyWaveDataReader);
        mapManager.PrepareGame(mapDataReader);
        cautionManager.PrepareGame();
        bulletTowerManager.PrepareGame(towerDataReader, bulletDataReader);
        barrackTowerManager.PrepareGame(towerDataReader, unitDataReader, wayPointDataReader);
        decorObjManager.PrepareGame(enemyManager, gamePlayManager, decorObjDataReader);
        emptyPlotManager.PrepareGame(wayPointDataReader);
        endPointManager.PrepareGame(wayPointDataReader);
        dustFX.PrepareGame();

        // Handler prepare game
        raycastHandler.PrepareGame();
        inputButtonHandler.PrepareGame();
        towerActionHandler.PrepareGame(inputButtonHandler, raycastHandler);

        // Pool Prepare game
        bulletPool.PrepareGame(bulletDataReader, bulletEffectDataReader);
        unitPool.PrepareGame(unitDataReader, skillDataReader);

        // UI manager prepare game
        gamePlayUIManager.PrepareGame(towerDataReader, bulletDataReader, unitDataReader);
        screenUIManager.PrepareGame();

        yield return null;
    }

    public IEnumerator InitPoolObj()
    {
        unitPool.Initialize();
        bulletPool.Initialize();
        decorObjPool.Initialize();
        visualEffectPool.Initialize();

        yield return null;
    }
}
