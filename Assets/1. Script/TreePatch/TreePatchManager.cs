using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePatchManager : MonoBehaviour
{
    [SerializeField] TreePatch treePatchPrefab;
    private EnemyManager enemyManager;
    private List<Enemy> ActiveUnitList => enemyManager.ActiveUnitList;
    public List<TreePatch> treePatchList = new List<TreePatch>();

    private Coroutine decayRoutine;
    private float checkInterval = 2f;

    public void PrepareGame(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }

    public void InitializeTreePatch(MapData mapData)
    {
        InitTreePatch(mapData);
    }

    private void InitTreePatch(MapData mapData)
    {
        List<TreePatchInfo> infoList = WayPointDataReader.Instance.GetTreePatchInfoList(mapData);
        foreach(var treePatchInfo in infoList)
        {
            foreach(var pos in treePatchInfo.treePatchList)
            {
                TreePatch treePatchScript = TreePatchPool.Instance.GetTreePatch(treePatchInfo.treePatchID);
                treePatchScript.transform.position = pos;
                treePatchScript.gameObject.SetActive(true);
                treePatchScript.PrepareGame();
                treePatchList.Add(treePatchScript);
            }
        }
        StartTreePatchDecayCoroutine();
    }

    public void StartTreePatchDecayCoroutine()
    {
        if(decayRoutine != null)
        {
            StopCoroutine(decayRoutine);
        }
        decayRoutine = StartCoroutine(TreePatchDecay());
    }

    private IEnumerator TreePatchDecay()
    {
        while(true)
        {
            if(ActiveUnitList.Count > 0)
            {
                TreePatchDecayLoop(3f);
                yield return new WaitForSeconds(checkInterval);

                TreePatchDecayLoop(4f);
                yield return new WaitForSeconds(checkInterval/2);
            }
            else
            {
                yield return new WaitForSeconds(checkInterval);
            }
        }
    }

    private void TreePatchDecayLoop(float decayTriggerRange)
    {
        foreach(var treePatch in treePatchList)
        {
            if(GetDistance(treePatch, ActiveUnitList[0]) < decayTriggerRange)
            {
                treePatch.StartDecay();
            }
        }
    }

    public void ResetTreePatchSprite()
    {
        foreach(var t in treePatchList)
        {
            t.SetDefaultSprite();
        }
    }

    public void ClearTreePatch()
    {
        foreach(var t in treePatchList)
        {
            TreePatchPool.Instance.ReturnTreePatch(t);
        }
    }

    private float GetDistance(TreePatch treePatch, Enemy enemy)
    {
        if(enemy == null) return 10f;
        return Vector2.Distance(treePatch.transform.position, enemy.transform.position);
    }
}
