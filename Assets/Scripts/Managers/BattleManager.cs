using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{


    protected override void Awake()
    {
        base.Awake();
        lockTargets = new List<LockTarget>();
    }

    #region 锁定
    [Header("锁定相关配置")]
    public List<LockTarget> lockTargets;
    public Unit player;
    public PlayerController playerController => player.GetComponent<PlayerController>();
    public Unit curLockTarget 
    {
        get
        {
            try
            {
                return playerController.lockTarget.GetComponentInParent<Unit>();
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }

    public void AddLockTarget(GameObject obj)
    {
        AddLockTarget(obj.GetComponent<LockTarget>());
    }
    public void AddLockTarget(LockTarget target)
    {
        lockTargets.Add(target);
    }
    public void RemoveLockTarget(GameObject obj)
    {
        lockTargets.Remove(obj.GetComponent<LockTarget>());
    }
    public void RemoveLockTarget(LockTarget target)
    {
        lockTargets.Remove(target);
    }

    #endregion
}