using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    private EnemyWaveDataReader enemyWaveDataReader;
    private WayPointDataReader wayPointDataReader;
    private EnemyManager enemyManager;
    // pathway infor
    public List<EnemySpawner> enemySpawnerList = new();
    private int NumberPathWay => enemySpawnerList.Count;
    List<MainPathWayInfo> mainPathWayInfoList = new();

    // Wave infor
    public int TotalWave = 0;
    private int CurrentWaveIndex { get; set; } = 0;
    public int CurrentWave => CurrentWaveIndex + 1;
    public int totalEnemies = 0;

    // spawn enemy infor
    // add 1 if any pathway finish spawn enemy in current wave
    private int totalPathWayFinishSpawnEnemy = 0;
    private float timeWaitForNextWave = 0f;

    // Time show BtnCaution, time call next wave
    private float               waitCautionStartTime;
    private Coroutine           WaitToCallNextWave;
    public event Action         OnCallNextWave;
    public event Action         OnCautionBtnClicked;
    public event Action<int>    OnAddGoldWhenCautionClick;
    public event Action<float>  OnUpdateTimeWaitForNextWave;

    // Update PanelManager
    public event Action<int> OnUpdateCurrentWave;

    // Trigger to call the first wave when BtnCautionSlider click
    private bool isBeginFristWave = false;

    public void PrepareGame(EnemyManager enemyManager,
                            WayPointDataReader wayPointDataReader,
                            EnemyWaveDataReader enemyWaveDataReader)
    {
        this.enemyManager = enemyManager;
        this.wayPointDataReader = wayPointDataReader;
        this.enemyWaveDataReader = enemyWaveDataReader;
    }

    public void GetInfor(MapData mapData)
    {    
        GetMainPathWayInfoList(mapData);
        InitEnemySpawner(mapData);
        RegisterFinishCurrentWaveEvent();
        GetTotalWave();
        GetTotalEnemies(mapData);
    }
    
    public void ClearEnemySpawnerManager()
    {
        StopAllCoroutines();
        mainPathWayInfoList.Clear();
        CLearEnemySpawnerList();
        isBeginFristWave = false;
        CurrentWaveIndex = 0;
        timeWaitForNextWave = 0;
        waitCautionStartTime = 0;
        totalPathWayFinishSpawnEnemy = 0;
        TotalWave = 0;
        totalEnemies = 0;
        WaitToCallNextWave = null;
        UnregisterFinishCurrentWaveEvent();
    }

    private void GetMainPathWayInfoList(MapData mapData)
    {
        mainPathWayInfoList.AddRange(wayPointDataReader.GetMainPathWayInfoList(mapData));
    }

    #region SPAWN ENEMYSPAWNER
    private void InitEnemySpawner(MapData mapData)
    {
        foreach (MainPathWayInfo mainPathWayInfo in mainPathWayInfoList)
        {
            Vector2 cautionPos = mainPathWayInfo.cautionBtnPos;
            int pathID = mainPathWayInfo.pathWayID;
            List<PathWaySegment> pathWaySegmentList = mainPathWayInfo.pathWaySegmentList;

            GameObject enemySpawnerObj = new("EnemySpawner");
            enemySpawnerObj.transform.SetParent(transform);
            enemySpawnerObj.transform.position = Vector2.zero;

            EnemySpawner enemySpawner = enemySpawnerObj.AddComponent<EnemySpawner>();
            enemySpawner.PrepareGame(enemyManager, this, cautionPos, pathID, mapData, enemyWaveDataReader, pathWaySegmentList);
            enemySpawnerList.Add(enemySpawner);
        }
    }

    private void CLearEnemySpawnerList()
    {
        foreach(var enemySpawner in enemySpawnerList)
        {
            enemySpawner.OnFinishCurrentWave -= HandleFinishCurrentWave;
            enemySpawner.UnregisterStartNextWaveEvent();
            Destroy(enemySpawner.gameObject);
        }
        enemySpawnerList.Clear(); 
    }

    private void RegisterFinishCurrentWaveEvent()
    {
        foreach(var enemySpawner in enemySpawnerList)
        {
            enemySpawner.OnFinishCurrentWave += HandleFinishCurrentWave;
        }
    } 

    private void UnregisterFinishCurrentWaveEvent()
    {
        foreach(var enemySpawner in enemySpawnerList)
        {
            enemySpawner.OnFinishCurrentWave -= HandleFinishCurrentWave;
        }
    }

    // after all enemies in current wave had spawn, start WaitToCallNextWaveCoroutine
    // to call next enemies
    private void HandleFinishCurrentWave(float timeWaitForNextWaveInOneWave)
    {
        UpdateTimeWaitForNextWave(timeWaitForNextWaveInOneWave);

        if(CurrentWave == TotalWave) return;
        totalPathWayFinishSpawnEnemy++;
        if(totalPathWayFinishSpawnEnemy == NumberPathWay)
        {
            totalPathWayFinishSpawnEnemy = 0;
            OnUpdateTimeWaitForNextWave?.Invoke(timeWaitForNextWave);
            WaitToCallNextWave = StartCoroutine(WaitToCallNextWaveCoroutine());
        }
    }

    private void UpdateTimeWaitForNextWave(float timeWaitForNextWaveInOneWave)
    {
        if(timeWaitForNextWave < timeWaitForNextWaveInOneWave)
        {
            timeWaitForNextWave = timeWaitForNextWaveInOneWave;
        }
    }
    
    private IEnumerator WaitToCallNextWaveCoroutine()
    {
        yield return new WaitForSeconds(4f);
        CheckToShowCautionBtnInWhichEnemySpawner();
        waitCautionStartTime = Time.time;
        yield return new WaitForSeconds(timeWaitForNextWave);
        waitCautionStartTime = 0;
        timeWaitForNextWave = 0;
        HideAllCautionFill();
        OnCallNextWave?.Invoke();
        UpdateCurrentWaveIndex();
    }
    #endregion

    #region CAUTION SLIDER
    private void CheckToShowCautionBtnInWhichEnemySpawner()
    {
        foreach(var enemyspawner in enemySpawnerList)
        {
            if(enemyspawner.HasEnemyInWave(CurrentWaveIndex + 1))
            {
                enemyspawner.cautionBtn.StartActiveCautionFill();
            }
            else
            {
                enemyspawner.cautionBtn.HideCautionFill();
            }
        }
    }

    private void HideAllCautionFill()
    {
        foreach (var enemySpawner in enemySpawnerList)
        {
            enemySpawner.cautionBtn.HideCautionFill();
        }
    }

    public void HandleCautionButtonClicked()
    {
        if(!isBeginFristWave)
        {
            foreach(var enemySpawner in enemySpawnerList)
            {
                enemySpawner.StartSpawnEnemyCoroutine();
            }
            isBeginFristWave = true;
            OnUpdateCurrentWave?.Invoke(CurrentWave);
        }

        if(WaitToCallNextWave != null)
        {
            StopCoroutine(WaitToCallNextWave);
            WaitToCallNextWave = null;

            // call next wave
            OnCallNextWave?.Invoke();

            // update gold when call next wave early
            HandleAddGoldCautionClick();
            UpdateCurrentWaveIndex();
        }
        OnCautionBtnClicked?.Invoke();

        // Reset timeWaitForNextWave
        timeWaitForNextWave = 0;
    }

    private void HandleAddGoldCautionClick()
    {
        if(waitCautionStartTime == 0) return;
        float elapsedTime = Time.time - waitCautionStartTime;
        float timeCallEarly = timeWaitForNextWave - elapsedTime;
        int goldCallEarly = (int)timeCallEarly*3;

        foreach(var enemySpawner in enemySpawnerList)
        {
            if(enemySpawner.cautionBtn.IsCautionFillActive())
            {
                OnAddGoldWhenCautionClick?.Invoke(goldCallEarly);
            }
        }
    }
    #endregion

    private void UpdateCurrentWaveIndex()
    {
        if(CurrentWaveIndex >= TotalWave) return;
        CurrentWaveIndex++;
        // update UI current wave
        OnUpdateCurrentWave?.Invoke(CurrentWave);
    }

    public float GetTimeWaitForNextWave()
    {
        Debug.Log(timeWaitForNextWave);
        return timeWaitForNextWave;
    }

    private void GetTotalWave()
    {
        TotalWave = enemySpawnerList[0].GetTotalWave();
    }

    private void GetTotalEnemies(MapData mapData)
    {
        totalEnemies = enemyWaveDataReader.GetTotalSelectedMapEnemies(mapData);
    }
}