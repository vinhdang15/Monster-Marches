using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUpManager : MonoBehaviour
{
    public static GameStartUpManager Instance { get; private set; }
    [SerializeField] GameInitManager gameInitManager;
    [SerializeField] GameFlowManager gameFlowManager;

    private GameObject dataHolder;
    private GameObject managerHolder;
    private GameObject handlerHolder;
    private GameObject uIHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateHolders();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(gameInitManager.CheckForUpdateAndDownloadData());

        yield return StartCoroutine(gameInitManager.PrepareJsonData());

        yield return StartCoroutine(gameInitManager.InitSpriteController(managerHolder.transform));

        yield return StartCoroutine(gameInitManager.Init(dataHolder.transform, managerHolder.transform,
                                                        handlerHolder.transform, uIHolder.transform));
                                                    
        yield return StartCoroutine(gameInitManager.PrepareGame());

        yield return StartCoroutine(gameInitManager.InitPoolObj());

        yield return StartCoroutine(InitGameFlowManager());

        yield return StartCoroutine(gameFlowManager.LoadIntroScene());
        gameFlowManager.RegisterEvent();

        SetUpSingleton();
    }

    // Create Holder
    private GameObject CreateHolderHandle(string name)
    {
        GameObject holder = new(name);
        holder.transform.SetParent(transform);
        return holder;
    }

    private void CreateHolders()
    {
        dataHolder = CreateHolderHandle("DataHolder");
        managerHolder = CreateHolderHandle("ManagerHolder");
        handlerHolder = CreateHolderHandle("HandlerHolder");
        uIHolder = CreateHolderHandle("UIHolder");
    }

    private IEnumerator InitGameFlowManager()
    {
        gameFlowManager = Instantiate(gameFlowManager, transform);
        GameFlowManagerGetReference();
        yield return null;
    }

    private void GameFlowManagerGetReference()
    {
        gameFlowManager.Init(gameInitManager.GetMapDataReader(),
                            gameInitManager.GetSpriteDisplayController(),
                            gameInitManager.GetSceneController(),
                            gameInitManager.GetMapManager(),
                            gameInitManager.GetEndPointManager(),
                            gameInitManager.GetGamePlayManager(),
                            gameInitManager.GetEmptyPlotManager(),
                            gameInitManager.GetDecorObjManager(),
                            gameInitManager.GetBulletTowerManager(),
                            gameInitManager.GetBarrackTowerManager(),
                            gameInitManager.GetBulletManager(),
                            gameInitManager.GetSoldierManager(),
                            gameInitManager.GetEnemyManager(),
                            gameInitManager.GetEnemySpawnerManager(),
                            gameInitManager.GetCautionManager(),
                            gameInitManager.GetGamePlayUIManager(),
                            gameInitManager.GetScreenUIManager()
        );
    }

    private void SetUpSingleton()
    {
        GameFlowManager.Instance = gameFlowManager;
    }
}
