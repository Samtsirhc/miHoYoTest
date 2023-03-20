using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit :MonoBehaviour
{
    public int constantBalanceLevel;
    public int curBalanceLevel;
    public virtual void TakeDamage(Damage damage) { }


    #region 动画事件
    private void AnimEvt_SetBalanceLevel(int level)
    {
        if (level == 0)
        {
            curBalanceLevel = constantBalanceLevel;
        }
        else
        {
            curBalanceLevel = level;
        }
    }
    #endregion

    #region 战斗事件
    public virtual void Die() { }
    #endregion
}
