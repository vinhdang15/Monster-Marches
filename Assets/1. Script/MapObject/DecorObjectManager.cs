using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorObjectManager : MonoBehaviour
{
    private EnemyManager        enemyManager;
    private GamePlayManager     gamePlayManager;
    private List<Enemy>         ActiveUnitList => enemyManager.ActiveUnitList;
    private Enemy               decayEnemy;
    public List<DecorObject> decorObjectList = new();
    public List<DecorObject>    staticObjList = new();
    public List<DecorObject>    animatedObjList = new();
    public List<DecorObject>    decayObjList = new();
    

    private Coroutine decayObjCoroutine;
    private Coroutine animatedObjCoroutine;
    private float checkInterval = 3f;

    public void PrepareGame(EnemyManager enemyManager, GamePlayManager gamePlayManager)
    {
        this.enemyManager = enemyManager;
        this.gamePlayManager = gamePlayManager;
    }

    public void InitializeDecorObj(MapData mapData)
    {
        InitDecorObj(mapData);
    }

    private void InitDecorObj(MapData mapData)
    {
        List<DecorObjectInfo> infoList = MapObjDataReader.Instance.GetDecorObjectInfoList(mapData);
        foreach(var info in infoList)
        {
            foreach(var pos in info.decorObjectPosList)
            {
                DecorObject DecorObjectScript = DecorObjectPool.Instance.GetDecorObject(info.decorID);
                DecorObjectScript.transform.position = pos;
                DecorObjectScript.gameObject.SetActive(true);
                decorObjectList.Add(DecorObjectScript);
            }
        }

        ClassifyDecorObject();
        if(decayObjList.Count > 0) StartDecayObjCoroutine();
        if(animatedObjList.Count > 0) StartAnimatedObjCoroutine();
    }

    private void ClassifyDecorObject()
    {
        foreach(var obj in decorObjectList)
        {
            if(obj.isStaticObj)
            {
                staticObjList.Add(obj);
            }
            else if(obj.isDecayObj)
            {
                obj.PrepareGame();
                obj.SetDefaultSprite();
                decayObjList.Add(obj);
            }
            else if(obj.isAnimatedObj)
            {
                obj.PrepareGame();
                obj.SetRandomDefaultSprite();
                animatedObjList.Add(obj);
            }
        }
    }

    #region DECOR OBJECT
    private void StartDecayObjCoroutine()
    {
        if(decayObjCoroutine != null)
        {
            StopCoroutine(decayObjCoroutine);
        }
        decayObjCoroutine = StartCoroutine(UpdateDecayObjsprite());
    }

    private void StartAnimatedObjCoroutine()
    {
        if(animatedObjCoroutine != null)
        {
            StopCoroutine(animatedObjCoroutine);
        }
        animatedObjCoroutine = StartCoroutine(UpdateAnimatedObj());
    }

    private IEnumerator UpdateDecayObjsprite()
    {
        while (true)
        {
            if (HasDecayEnemy())
            {
                DecayObjChangeSprite(3f);
                yield return new WaitForSeconds(checkInterval / 3);

                DecayObjChangeSprite(4f);
                yield return new WaitForSeconds(checkInterval / 3);

                DecayObjChangeSprite(5f);
                yield return new WaitForSeconds(checkInterval / 3);
            }
            else if (gamePlayManager.currentLives != 0)
            {
                DecayObjHealing();
                yield return new WaitForSeconds(checkInterval / 4);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void DecayObjChangeSprite(float decayTriggerRange)
    {
        if(decayEnemy.CurrentHp == 0) return;
        foreach(var decayObj in decayObjList)
        {
            if(GetDistance(decayObj, decayEnemy) < decayTriggerRange && decayEnemy.isMoving)
            {
                decayObj.ChangeSprite();
            }
        }
    }

    private bool HasDecayEnemy()
    {
        foreach (Enemy enemy in ActiveUnitList)
        {
            if (enemy.UnitID == UnitID.Enemy_A_1.ToString())
            {
                decayEnemy = enemy;
                return true;
            }
        }
        decayEnemy = null;
        return false;
    }

    private void DecayObjHealing()
    {
        foreach (var decayObj in decayObjList)
        {
            decayObj.StartHealing();
        }
    }

    public void ResetDecayObjSprite()
    {
        foreach(var d in decayObjList)
        {
            d.SetDefaultSprite();
        }
    }

    public void ClearDecayObj()
    {
        foreach(var obj in decorObjectList)
        {
            if(obj.isDecayObj)
            {
               obj.SetDefaultSprite();
            }
            DecorObjectPool.Instance.ReturnDecayObj(obj);
        }
    }
    #endregion

    #region ANIMATED OBJECT
    private IEnumerator UpdateAnimatedObj()
    {
        while(true)
        {
            foreach(var obj in decorObjectList)
            {
                if(obj.isAnimatedObj)
                {
                    obj.LoopChangeSprite();
                }
            }
                yield return new WaitForSeconds(0.1f);
        }
        
    }
    #endregion

    private float GetDistance(DecorObject decayObj, Enemy enemy)
    {
        if(enemy == null) return 10f;
        return Vector2.Distance(decayObj.transform.position, enemy.transform.position);
    }
}
