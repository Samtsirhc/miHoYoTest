using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Unit sourceUnit;
    public List<Unit> targetUnits;

    public float hitbackForce;  // 击退力量大小
    public float getNuclearValue;   // 命中时获取核力量
    [HideInInspector]
    public float nuclearValue;  // 本次技能消耗的核力

    public float basicFakeDamage;   // 基础假血伤害
    public float nuclearToFakeDamageRatio;  // 核力转化为假血伤害的比率
    [HideInInspector]
    public float fakeDamage;    // 最终假血伤害

    public float basicRealDamage;   // 基础真血伤害
    public float nuclearToRealDamageRatio;  // 核力转化为真血伤害的比率
    [HideInInspector]
    public float realDamage;    // 最终真血伤害

    public int unbalanceLevel;  // 本次伤害失衡值


    public void Init(Unit source_unit, float nuclear_value)
    {
        sourceUnit = source_unit;
        nuclearValue = nuclear_value;
        _CalculateDamge();
    }

    private void _CalculateDamge()
    {
        fakeDamage = nuclearToFakeDamageRatio * nuclearValue + basicFakeDamage;
        realDamage = nuclearToRealDamageRatio * nuclearValue + basicRealDamage;
    }

}
