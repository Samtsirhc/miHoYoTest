using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public List<GameObject> damageZones;
    #region 核力与上下限，矢量晶体
    public bool flagForBreak = false;   // 引力破碎标记
    public float nuclearValue
    {
        get => _nuclearValue;
        set
        {
            if (value >= nuclearValueUpperLimit)
            {
                _nuclearValue = nuclearValueUpperLimit;
                if (flagForBreak)
                {
                    GravitationBreak();
                    return;
                }
            }
            else
            {
                _nuclearValue = value;
            }
            flagForBreak = false;   // 每次Set完，把标记取消掉
            return;
        }
    }
    private float _nuclearValue = 0f;

    public float nuclearValueUpperLimit
    {
        get => _nuclearValueUpperLimit;
        set
        {
            if (value <= nuclearValueLowerLimit)
            {
                GravitationBreak();
                return;
            }
            if (value <= nuclearValue)
            {
                GravitationBreak();
                return;
            }
            _nuclearValueUpperLimit = value;
            return;
        }
    }
    private float _nuclearValueUpperLimit = 1000f;

    public float nuclearValueLowerLimit = 300f;

    public int vectorCrystalNum
    {
        get => _vectorCrystalNum;
        set
        {
            _vectorCrystalNum = value;
            if (value <= 0)
            {
                Die();
            }
        }
    }
    private int _vectorCrystalNum;
    #endregion

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
    }

    public void GravitationBreak()  // 引力破碎，恢复上限，清空核力，晶体数量减少
    {
        flagForBreak = false;
        nuclearValueUpperLimit = 1000f;
        nuclearValue = 0;
        vectorCrystalNum -= 1;

    }

    public override void Die()
    {
        base.Die();
    }
    public override void AnimEvt_AttackDamage(int index)
    {
        base.AnimEvt_AttackDamage(index);
        CreatDamageZone(damageZones[index]);

    }

}
