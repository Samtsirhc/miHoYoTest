using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public List<GameObject> damageZones;
    public float energy
    {
        get => _energy;
        set
        {
            if (value >= 1000)
            {
                _energy = 1000;
            }
            else
            {
                _energy = value;
            }
        }
    }
    private float _energy;
    private PlayerController playerController => GetComponent<PlayerController>();

    #region Unity函数
    private void Start()
    {
        BattleManager.Instance.player = this;
    }
    #endregion

    #region 核力与上下限，矢量晶体
    public float nuclearValue
    {
        get => _nuclearValue;
        set
        {
            if (value >= nuclearValueUpperLimit)
            {
                _nuclearValue = nuclearValueUpperLimit;
            }
            else
            {
                _nuclearValue = value;
            }
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
            if (value < nuclearValue && value > nuclearValueLowerLimit)
            {
                GravitationBreak();
                return;
            }
            _nuclearValueUpperLimit = value;
            return;
        }
    }
    private float _nuclearValueUpperLimit = 1000f;

    public float nuclearValueLowerLimit = 0f;

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
    private int _vectorCrystalNum = 3;
    #endregion

    #region 战斗相关
    [Header("战斗数值设定")]
    public float nVToLimit = 0.3f;
    public float nVToEnergy = 0.3f;
    public override void Init()
    {
        base.Init();
    }
    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        if (playerController.isInvinciable)
        {
            (damage.sourceUnit as Enemy).ShowWeak();
            Debug.Log("闪避了");
            return;
        }
        if (canClash)
        {
            Clash();
            (damage.sourceUnit as Enemy).HitReact(21);
            (damage.sourceUnit as Enemy).fakeHp -= 50;
            nuclearValue += damage.fakeDamage * 2;
            return;
        }
        if (playerController.isSuperArmor)
        {
            (damage.sourceUnit as Enemy).HitReact(11);
            nuclearValue += damage.fakeDamage * 1.5f;
            return;
        }
        nuclearValue += damage.fakeDamage;
        nuclearValueUpperLimit -= damage.realDamage;
        playerController.Hit_L();
    }

    public void GravitationBreak()  // 引力破碎，恢复上限，清空核力，晶体数量减少
    {
        nuclearValueUpperLimit = 1000f;
        nuclearValue = 0;
        vectorCrystalNum -= 1;
        playerController.Hit_H();

    }

    public override void Die()
    {
        base.Die();
    }
    public void BurstAttack()
    {
        consumeNuclearTime = 2;
        nuclearValueToUse = nuclearValue;
        ConsumeNuclearValue();
    }
    public void ConsumeNuclearValue()
    {
        nuclearValueUpperLimit += nuclearValue * nVToLimit;
        if (nuclearValueUpperLimit >= 1000)
        {
            nuclearValueUpperLimit = 1000;
        }
        energy += nuclearValue * nVToEnergy;
        nuclearValue = 0;
    }
    public int consumeNuclearTime = 0;
    public float nuclearValueToUse = 0f;

    public bool canClash = false;
    public void Clash()
    {
        playerController.Clash();
        nuclearValue += 100;
    }
    #endregion

    #region 动画事件
    public override void AnimEvt_AttackDamage(int index)
    {
        base.AnimEvt_AttackDamage(index);
        if (consumeNuclearTime > 0)
        {
            consumeNuclearTime -= 1;
            CreatDamageZone(damageZones[index], nuclearValueToUse);
        }
        else
        {
            CreatDamageZone(damageZones[index]);
        }
        if (index != 3)
        {
            playerController.impulseSource_01.GenerateImpulse();
        }
        else
        {
            playerController.impulseSource_02.GenerateImpulse();
        }
    }
    public void AnimEvt_ClashOpen()
    {
        canClash = true;
    }
    public void AnimEvt_ClashClose()
    {
        canClash = false;
    }
    #endregion

}
