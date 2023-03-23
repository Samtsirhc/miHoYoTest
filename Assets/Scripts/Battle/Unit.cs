using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit :MonoBehaviour
{
    public int constantBalanceLevel;
    public int curBalanceLevel;
    public virtual void TakeDamage(Damage damage) 
    {
        if (damage.targetUnits.Exists(i => i == this))
        {
            return;
        }
        damage.targetUnits.Add(this);
    }


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
    public virtual void Init() { }
    public virtual void Die() { }
    #endregion

    #region 动画事件
    public virtual void AnimEvt_AttackDamage(int index)
    {
    }
    public void CreatDamageZone(GameObject pfb)
    {
        GameObject obj = Instantiate(pfb, transform);
        obj.GetComponent<Damage>().Init(this, 0);
        obj.GetComponent<DamageZone>().Init();
    }
    public void CreatDamageZone(GameObject pfb, float nuclear_v)
    {
        GameObject obj = Instantiate(pfb, transform);
        obj.GetComponent<Damage>().Init(this, nuclear_v);
        obj.GetComponent<DamageZone>().Init();
    }
    #endregion
}
