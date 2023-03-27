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
    bool needSwitch = true;
    private void FixedUpdate()
    {

        if (needSwitch && lockTargets.Count != 0)
        {
            AudioManager.Instance.PlayAudio("战斗", 1);
            AudioManager.Instance.PauseAudio("无意义", 1);
        }
        if (!needSwitch && lockTargets.Count == 0)
        {
            AudioManager.Instance.PlayAudio("无意义", 1);
            AudioManager.Instance.PauseAudio("战斗", 1);
        }
        if (lockTargets.Count == 0)
        {
            needSwitch = true;
        }
        else
        {
            needSwitch = false;
        }
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
        if (playerController.lockTarget == target)
        {
            playerController.lockTarget = null;
            playerController.ReSetLock();
        }

        lockTargets.Remove(target);

    }

    #endregion
}