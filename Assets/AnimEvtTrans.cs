using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvtTrans : MonoBehaviour
{
    public Enemy enemy;
    private void AnimEvt_StopHit()
    {
        enemy.AnimEvt_StopHit();
    }
    private void AnimEvt_MoveClose()
    {
        enemy.AnimEvt_MoveClose();
    }
    private void AnimEvt_MoveOpen()
    {
        enemy.AnimEvt_MoveOpen();
    }
    private void AnimEvt_AttackClose()
    {
        enemy.AnimEvt_AttackClose();
    }
    private void AnimEvt_AttackOpen()
    {
        enemy.AnimEvt_AttackOpen();
    }
    private void AnimEvt_AttackStart()
    {
        enemy.AnimEvt_AttackStart();
    }
    private void AnimEvt_AttackEnd()
    {
        enemy.AnimEvt_AttackEnd();
    }

}
