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
    private void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        RecoverFakeHp();
    }
    #endregion

    #region 战斗相关
    public override void Init()
    {
        base.Init();
        realHp = maxHp;
        fakeHp = maxHp;
    }
    public void RecoverFakeHp() // 在fixed update中使用
    {
        recoverTimer -= Time.fixedDeltaTime;
        if (recoverTimer <= 0)
        {
            fakeHp += recoverRate * Time.fixedDeltaTime;
        }
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        recoverTimer = startRecoverTime;

        float fake_dmg;
        if (damage.fakeDamage >= fakeHp)
        {
            fake_dmg = fakeHp;
        }
        else
        {
            fake_dmg = damage.fakeDamage;
        }
        fakeHp -= fake_dmg;
        Debug.Log("结构伤害: " + fake_dmg + " " + damage.sourceUnit + " => " + this);

        float hp_dif = realHp - fakeHp;
        float real_dmg;
        if (damage.realDamage >= hp_dif)
        {
            real_dmg = hp_dif;
        }
        else
        {
            real_dmg = damage.realDamage;
        }
        realHp -= real_dmg;
        Debug.Log("消解伤害: " + real_dmg + " " + damage.sourceUnit + " => " + this);

        Player p = damage.sourceUnit as Player;
        p.nuclearValue += damage.getNuclearValue;
    }
    #endregion
}
