using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;
    private MapDataReader mapDataReader;
    private SpriteDisplayController spriteDisplayController;

    private SceneController sceneController;
    private MapManager mapManager;
    private EndPointManager endPointManager;
    private GamePlayManager gamePlayManager;
    private EmptyPlotManager emptyPlotManager;
    private DecorObjectManager decorObjManager;
    private BulletTowerManager bulletTowerManager;
    private BarrackTowerManager barrackTowerManager;
    private BulletManager bulletManager;
    private SoldierManager soldierManager;
    private EnemyManager enemyManager;
    private EnemySpawnerManager enemySpawnerManager;
    private CautionManager cautionManager;

    private GamePlayUIManager gamePlayUIManager;
    private ScreenUIManager screenUIManager;

    public MapData currentMapData;

    public void Init(
        MapDataReader mDR,
        SpriteDisplayController sDC,
        SceneController sC,
        MapManager mM,
        EndPointManager endPointM,

        GamePlayManager gPM,
        EmptyPlotManager emptyPlotPM,

        DecorObjectManager dOM,
        BulletTowerManager bulletTM,
        BarrackTowerManager barackTM,

        BulletManager bM,
        SoldierManager sM,

        EnemyManager eM,
        EnemySpawnerManager eSM,

        CautionManager cM,
        GamePlayUIManager gPUIM,
        ScreenUIManager sUIM
    )
    {
        mapDataReader = mDR;
        spriteDisplayController = sDC;
        sceneController = sC;
        mapManager = mM;
        endPointManager = endPointM;
        gamePlayManager = gPM;
        emptyPlotManager = emptyPlotPM;
        decorObjManager = dOM;
        bulletTowerManager = bulletTM;
        barrackTowerManager = barackTM;
        bulletManager = bM;
        soldierManager = sM;
        enemyManager = eM;
        enemySpawnerManager = eSM;
        cautionManager = cM;
        gamePlayUIManager = gPUIM;
        screenUIManager = sUIM;
    }

    public void RegisterEvent()
    {
        mapManager.OnLoadSelectedMap += HandleLoadSelectedMap;
    }

    public IEnumerator LoadIntroScene()
    {
        sceneController.LoadIntroScene();

        spriteDisplayController.LoadIntroSprite();
        CameraController.Instance.ResetBoundingShape(spriteDisplayController);
        yield return null;
    }

    public void HandleReloadIntroScene()
    {
        screenUIManager.HideWorldMapSceneUI();
        screenUIManager.ShowStartGameMenu();
        spriteDisplayController.LoadIntroSprite();
        mapManager.HideMapBtn();
    }

    // Button Event
    public void HandleLoadWorldMapScene()
    {
        screenUIManager.ShowWorldMapUI();
        screenUIManager.HideSaveGameMenu();
        sceneController.LoadWorldMapScene();
        spriteDisplayController.LoadWorldMapSprite();
        CameraController.Instance.ResetBoundingShape(spriteDisplayController);
        mapManager.InitMapBtn();
    }

    // Button envent - Show save game menu
    public void ShowSaveGameMenu()
    {
        screenUIManager.ShowSaveGameMenuHandler();
    }

    #region LOAD SELECTED MAP
    private void HandleLoadSelectedMap(MapPresenter selectedMapPresenter)
    {
        currentMapData = selectedMapPresenter.mapModel.mapData;
        SetupVisuals();
        SetupGameObject();
        SetupGameStt();
        SetupGamePlayUI();
        sceneController.LoadSelectedMapScene();
        CheckToShowInstructionMenu();
    }

    private void SetupVisuals()
    {
        spriteDisplayController.LoadSelectedMapSprite(currentMapData);
        CameraController.Instance.ResetBoundingShape(spriteDisplayController);
    }

    private void SetupGameObject()
    {
        decorObjManager.InitializeDecorObj(currentMapData);
        emptyPlotManager.InitializeEmptyPlot(currentMapData);
        endPointManager.CreateEndPoint(currentMapData);
        barrackTowerManager.InitializeGuardPointPosList(currentMapData);

        enemySpawnerManager.GetInfor(currentMapData);
        cautionManager.InitializeCautionBtn();
        Time.timeScale = 1;
    }

    private void SetupGameStt()
    {
        gamePlayManager.GetMapInfor(currentMapData);
        gamePlayUIManager.GetInfor();
    }

    private void SetupGamePlayUI()
    {
        screenUIManager.HideWorldMapSceneUI();
        screenUIManager.ShowFPSText();
        screenUIManager.ShowAllGamePlayIUList();
    }

    private void CheckToShowInstructionMenu()
    {
        gamePlayUIManager.ShowInstructionMenu(mapManager.HasActiveMapID2());
    }
    #endregion

    // Button Event - reload world map after finished a game
    public void HandleFinishMap()
    {
        screenUIManager.ResetVictoryMenuState();
        screenUIManager.ShowWorldMapUI();
        HandleQuitCurrentMap();
        mapManager.UpdateMapPresenterInfo();
    }

    // Button Event
    public void HandleQuitCurrentMap()
    {
        ClearAllGameManager();
        gamePlayUIManager.HidePauseMenu();
        spriteDisplayController.LoadWorldMapSprite();
        CameraController.Instance.ResetBoundingShape(spriteDisplayController);
        mapManager.ShowMapBtn();
        screenUIManager.ShowWorldMapUI();

        sceneController.LoadWorldMapScene();
        Time.timeScale = 1;
    }

    private void ClearAllGameManager()
    {
        decorObjManager.ClearDecorObj();
        emptyPlotManager.ClearEmptyPlot();

        bulletTowerManager.ClearBulletTowers();
        barrackTowerManager.ClearBarrackTowers();

        bulletManager.ClearBulletManager();
        soldierManager.ClearSoldierManager();
        enemyManager.ClearEnemyManager();

        enemySpawnerManager.ClearEnemySpawnerManager();
        cautionManager.ClearCautionBtnManager();
        endPointManager.ClearEndPoints();
    }

    // Button Event
    public void HandleReloadCurrentMap()
    {
        ClearGameManager();
        SetupGameStt();

        decorObjManager.ResetDecayObjSprite();
        emptyPlotManager.ShowAllEmptyPlot();
        gamePlayUIManager.HidePauseMenu();
        gamePlayUIManager.HideGameOverMenu();
        gamePlayUIManager.ResetVictoryMenu();
        sceneController.ReLoadCurrentScene();

        enemySpawnerManager.ClearEnemySpawnerManager();
        enemySpawnerManager.GetInfor(currentMapData);

        cautionManager.InitializeCautionBtn();
    }

    private void ClearGameManager()
    {
        bulletTowerManager.ClearBulletTowers();
        barrackTowerManager.ClearBarrackTowers();

        bulletManager.ClearBulletManager();
        soldierManager.ClearSoldierManager();
        enemyManager.ClearEnemyManager();
        cautionManager.ClearCautionBtnManager();
    }

    // Button Event
    public void HandleSetNewGameBtnClick()
    {
        screenUIManager.ShowWorldMapSceneUIList();
        ResetMapProgressDataCoroutine();
    }

    private async void ResetMapProgressDataCoroutine()
    {
        await JSONDataLoader.ResetMapProgressData();
        mapManager.CLearAllMapBtn();
        mapDataReader.ResetFullMapData();
        HandleLoadWorldMapScene();
    }
}
