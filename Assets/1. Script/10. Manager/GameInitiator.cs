using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CameraController cameraController;

    [Header("Load Data")]
    [SerializeField] private CSVEmptyPlotDataReader     _CSVEmptyPlotDataReader;
    [SerializeField] private CSVTowerDataReader         _CSVTowerDataReader;
    [SerializeField] private CSVBulletDataReader        _CSVBulletDataReader;
    [SerializeField] private CSVEffectDataReader        _CSVEffectDataReader;
    [SerializeField] private CSVUnitDataReader          _CSVUnitDataReader;

    [Header("Init Canvas")]
    [SerializeField] private GameObject canvasScreenSpace;
    [SerializeField] private GameObject canvasWorldSpace;

    [Header("LevelManager")]
    [SerializeField] private LevelManager levelManager;

    [Header("GameManager")]        
    [SerializeField] private FPSCounter fPSCounter;
    
    [SerializeField] private EmptyPlotManager emptyPlotManager;
    [SerializeField] private BulletTowerManager bulletTowerManager;
    [SerializeField] private BarrackTowerManager barrackTowerManager;
    [SerializeField] private SoldierManager soldierManager;
    [SerializeField] private BulletManager bulletManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private InputControllerxxx inputController;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private CautionManager cautionManager;
    private GameObject gameManager;
    private GameObject levelData;
    
    private void Awake()
    {
        gameManager = new GameObject("GameManager");
        levelData = new GameObject("LevelManager");
    }
    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadData());
        yield return StartCoroutine(BindCanvas());
        yield return StartCoroutine(BindObjects());
        yield return StartCoroutine(PrepareGame());
    }
    
    private IEnumerator LoadData()
    {
        Instantiate(_CSVEmptyPlotDataReader);
        Instantiate(_CSVTowerDataReader);
        Instantiate(_CSVBulletDataReader);
        Instantiate(_CSVEffectDataReader);
        Instantiate(_CSVUnitDataReader);
        yield return null;
    }

    private IEnumerator BindCanvas()
    {
        canvasScreenSpace = Instantiate(canvasScreenSpace);
        canvasScreenSpace.name = InitNameObject.CanvasScreenSpace.ToString();
        canvasWorldSpace = Instantiate(canvasWorldSpace);
        canvasWorldSpace.name = InitNameObject.CanvasWorldSpace.ToString();
        yield return null;
    }

    private IEnumerator BindObjects()
    {
        cameraController = Instantiate(cameraController);
        cameraController.name = InitNameObject.Camera.ToString();

        levelManager.BindMap(levelData);

        fPSCounter = Instantiate(fPSCounter, gameManager.transform);
        fPSCounter.name = InitNameObject.FPSCounter.ToString();

        levelManager.BindPoolObject(levelData);

        emptyPlotManager = Instantiate(emptyPlotManager, gameManager.transform);
        emptyPlotManager.name = InitNameObject.EmptyPlotManager.ToString();

        bulletManager = Instantiate(bulletManager, gameManager.transform);
        bulletManager.name = InitNameObject.BulletManager.ToString();

        soldierManager = Instantiate(soldierManager, gameManager.transform);
        soldierManager.name = InitNameObject.SoldierManager.ToString();

        bulletTowerManager = Instantiate(bulletTowerManager, gameManager.transform);
        bulletTowerManager.name = InitNameObject.BulletTowerManager.ToString();

        barrackTowerManager = Instantiate(barrackTowerManager, gameManager.transform);
        barrackTowerManager.name = InitNameObject.BarrackTowerManager.ToString();

        enemyManager = Instantiate(enemyManager, gameManager.transform);
        enemyManager.name = InitNameObject.EnemyManager.ToString();

        cautionManager = Instantiate(cautionManager, gameManager.transform);
        cautionManager.name = InitNameObject.CautionManager.ToString();

        levelManager.BindSpawnEnemyManager(levelData);

        levelManager.BindSpawnEnemiesObject(levelData);

        inputController = Instantiate(inputController, gameManager.transform);
        inputController.name = InitNameObject.InputController.ToString();
        inputController.AddArcherBtn();
        
        gamePlayManager = Instantiate(gamePlayManager, gameManager.transform);
        gamePlayManager.name = InitNameObject.GamePlayManager.ToString();

        panelManager = Instantiate(panelManager, gameManager.transform);
        panelManager.name = InitNameObject.PanelManager.ToString();
        yield return null;
    }

    private IEnumerator PrepareGame()
    {
        yield return new WaitForSeconds(0.5f);
        panelManager.PanelManagerPrepareGame();
        levelManager.SpawnEnemyManagerPrepareGame();
        levelManager.SpawnEnemiesPrepareGame();
        yield return null;
        cameraController.LoadComponents(levelManager.GetMapPolygonCollider2D());
    }

}
