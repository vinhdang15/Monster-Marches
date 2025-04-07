using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
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
    private int finishedSpawnEnemiesCount = 0;
    [SerializeField] private float timeBetweenEnemy;
    [SerializeField] private float timeWaitForNextWave = 3f;

    // Time show BtnCaution, time call next wave
    private float waitCautionStartTime;
    private Coroutine WaitToCallNextWave;
    public event Action OnCallNextWave;
    public event Action      OnCautionBtnClicked;
    public event Action<int> OnUpdateCurrentWave;
    public event Action<int> OnAddGoldWhenCautionClick;

    // trigger to call the first wave when BtnCautionSlider click
    private bool isBeginFristWave = false;

    public void GetInfor(MapData mapData)
    {
        GetMainPathWayInfoList(mapData);
        InitEnemySpawner();
        RegisterFinishCurrentWaveEvent();
        GetTotalWave();
        GetTotalEnemies();
    }
    
    public void ResetEnemySpawnerManager()
    {
        ResetEnemySpawner();
        isBeginFristWave = false;
        UnregisterFinishCurrentWaveEvent();
    }

    private void GetMainPathWayInfoList(MapData mapData)
    {
        mainPathWayInfoList = WayPointDataReader.Instance.GetMainPathWayInfoList(mapData);
    }

    #region SPAWN ENEMYSPAWNER
    private void InitEnemySpawner()
    {
        ResetEnemySpawner();

        foreach(MainPathWayInfo mainPathWayInfo in mainPathWayInfoList)
        {
            Vector2 cautionPos = mainPathWayInfo.cautionBtnPos;
            List<PathWaySegment> pathWaySegmentList =  mainPathWayInfo.pathWaySegmentList;

            GameObject enemySpawnerObj = new GameObject();
            enemySpawnerObj.name = "EnemySpawner";
            enemySpawnerObj.transform.SetParent(transform);
            enemySpawnerObj.transform.position = Vector2.zero;
            
            EnemySpawner enemySpawner = enemySpawnerObj.AddComponent<EnemySpawner>();
            enemySpawner.PrepareGame(this, cautionPos, pathWaySegmentList);
            enemySpawnerList.Add(enemySpawner);
        }
    }

    private void ResetEnemySpawner()
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
    private void HandleFinishCurrentWave()
    {
        if(CurrentWave == TotalWave) return;
        finishedSpawnEnemiesCount++;
        if(finishedSpawnEnemiesCount == NumberPathWay)
        {
            finishedSpawnEnemiesCount = 0;
            WaitToCallNextWave = StartCoroutine(WaitToCallNextWaveCoroutine());
        }
    }
    
    private IEnumerator WaitToCallNextWaveCoroutine()
    {
        yield return new WaitForSeconds(2f);
        CheckToShowCautionBtnInWhichEnemySpawner();
        waitCautionStartTime = Time.time;
        yield return new WaitForSeconds(timeWaitForNextWave);
        waitCautionStartTime = 0;
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
        foreach(var enemySpawner in enemySpawnerList)
        {
            // spawnEnemy.cautionBtn.gameObject.SetActive(false);
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
            HandleCautionClick();
            UpdateCurrentWaveIndex();
        }
        OnCautionBtnClicked?.Invoke();
    }

    private void HandleCautionClick()
    {
        float elapsedTime = Time.time - waitCautionStartTime;
        float timeCallEarly = timeWaitForNextWave - elapsedTime;
        int goldCallEarly = (int)timeCallEarly*2;

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

    public float GetTimeBetweenEnemy()
    {
        return timeBetweenEnemy;
    }

    public float GetTimeWaitForNextWave()
    {
        return timeWaitForNextWave;
    }

    private void GetTotalWave()
    {
        TotalWave = enemySpawnerList[0].GetTotalWave();
    }

    private void GetTotalEnemies()
    {
        foreach(var enemySpawner in enemySpawnerList)
        {
            foreach(var enemyEntry in enemySpawner.enemyEntries)
            {
                totalEnemies += enemyEntry.numberEnemyInWave;
            }
        }   
    }
}