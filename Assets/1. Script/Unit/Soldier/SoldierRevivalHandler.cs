using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoldierRevivalHandler : MonoBehaviour
{
    private Soldier soldier;

    public void Init(Soldier soldier)
    {
        this.soldier = soldier;
    }
    public IEnumerator RevivalCoroutine(float revivalTime)
    {
        yield return null;

        soldier.ResetSoldierState();
        yield return new WaitForSeconds(soldier.unitAnimation.GetCurrentAnimationLength());

        gameObject.SetActive(false);

        soldier.ReturnToBarrackGatePos();
        yield return new WaitForSeconds(revivalTime);

        // if barrack tower is upgrading, 
        // this soldier will be process to return to unit pool
        // therefor soldier.barrackTowerView will be null
        // stop RevivalCoroutine here
        
        if(soldier.isBarrackUpgrade == true) yield break;
        if(soldier.barrackTowerView == null) yield break;

        soldier.barrackTowerView.OpenGateAnimation();
        yield return new WaitForSeconds(0.8f);

        soldier.ResetUnit();
        soldier.isDead = false;

        gameObject.SetActive(true);
        yield break;
    }
}
