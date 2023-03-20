using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    #region 真假血量 血量恢复
    public float maxHp = 1000f;
    public float realHp
    {
        get => _realHp;
        set
        {
            if (value <= 0)
            {
                _realHp = 0;
                Die();
            }
            else
            {
                _realHp = value;
            }
        }
    }
    private float _realHp;

    public float fakeHp
    {
        get => _fakeHp;
        set
        {
            if (value >= realHp)
            {
                _fakeHp = realHp;
            }
            else
            {
                _fakeHp = value;
            }
        }
    }
    private float _fakeHp;

    public float startRecoverTime = 10f;
    private float recoverTimer = 0;
    public float recoverRate = 30f;
    #endregion

    #region Unity函数

    #endregion

    #region 战斗相关
    public void RecoverFakeHp() // 在fixed update中使用
    {
        if (recoverTimer <= 0)
        {
            fakeHp += recoverRate * Time.fixedDeltaTime;
        }
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        recoverTimer = startRecoverTime;
    }
    #endregion
}
