using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorObjectManager : MonoBehaviour
{
    private DecorObjDataReader  decorObjDataReader;
    private EnemyManager        enemyManager;
    private GamePlayManager     gamePlayManager;
    private List<Enemy>         ActiveUnitList => enemyManager.ActiveUnitList;
    private Enemy               decayEnemy;
    public List<DecorObject>    decorObjectList = new();
    public List<DecorObject>    staticObjList = new();
    public List<DecorObject>    animatedObjList = new();
    public List<DecorObject>    decayObjList = new();
    

    private Coroutine decayObjCoroutine;
    private Coroutine animatedObjCoroutine;
    private float checkInterval = 3f;

    public void PrepareGame(EnemyManager enemyManager, GamePlayManager gamePlayManager,
                            DecorObjDataReader decorObjDataReader)
    {
        this.enemyManager = enemyManager;
        this.gamePlayManager = gamePlayManager;
        this.decorObjDataReader = decorObjDataReader;
    }

    public void InitializeDecorObj(MapData mapData)
    {
        InitDecorObj(mapData);
    }

    private void InitDecorObj(MapData mapData)
    {
        List<DecorObjectInfo> infoList = decorObjDataReader.GetDecorObjectInfoList(mapData);
        foreach (var info in infoList)
        {
            foreach (var pos in info.decorObjectPosList)
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

    #region DECAY OBJECT
    private void StartDecayObjCoroutine()
    {
        if(decayObjCoroutine != null)
        {
            StopCoroutine(decayObjCoroutine);
        }
        decayObjCoroutine = StartCoroutine(UpdateDecayObjsprite());
    }

    private IEnumerator UpdateDecayObjsprite()
    {
        while (true)
        {
            if (HasDecayEnemy())
            {
                DecayObjChangeSprite(2f);
                yield return new WaitForSeconds(checkInterval / 3);

                DecayObjChangeSprite(3f);
                yield return new WaitForSeconds(checkInterval / 3);

                DecayObjChangeSprite(4f);
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
        if(decayEnemy == null) return;
        foreach(var decayObj in decayObjList)
        {
            if(GetDistance(decayObj, decayEnemy) < decayTriggerRange && decayEnemy.isMoving)
            {
                decayObj.ChangeSprite();
            }
        }
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
    #endregion

    #region ANIMATED OBJECT
    private void StartAnimatedObjCoroutine()
    {
        if(animatedObjCoroutine != null)
        {
            StopCoroutine(animatedObjCoroutine);
        }
        animatedObjCoroutine = StartCoroutine(UpdateAnimatedObjSprite());
    }

    private IEnumerator UpdateAnimatedObjSprite()
    {
        while (true)
        {
            if (!HasDecayEnemy())
            {
                LoopAllAnimatedObjects();
                yield return new WaitForSeconds(0.1f);
            }
            if (HasDecayEnemy())
            {
                LoopAnimatedObjectsOutsideRange(4f);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void LoopAllAnimatedObjects()
    {
        foreach (var obj in animatedObjList)
        {
            obj.LoopChangeSprite();
        }
    }
    private void LoopAnimatedObjectsOutsideRange(float decayTriggerRange)
    {
        foreach (var obj in animatedObjList)
        {
            if (ShouldAnimate(obj, decayTriggerRange))
            {
                obj.LoopChangeSprite();
            }
        }

    }

    private bool ShouldAnimate(DecorObject obj, float range)
    {
        return  decayEnemy != null &&
                decayEnemy.isMoving &&
                GetDistance(obj, decayEnemy) > range &&
                obj.transform.position.x < decayEnemy.transform.position.x;
    }
    #endregion

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

    public void ClearDecorObj()
    {
        foreach (var obj in decorObjectList)
        {
            if (obj.isDecayObj)
            {
                obj.SetDefaultSprite();
            }
            DecorObjectPool.Instance.ReturnDecayObj(obj);
        }
    }

    private float GetDistance(DecorObject decayObj, Enemy enemy)
    {
        if (enemy == null) return 10f;
        return Vector2.Distance(decayObj.transform.position, enemy.transform.position);
    }
}
