using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTarget : MonoBehaviour
{
    private void Start()
    {
        BattleManager.Instance.AddLockTarget(this);
    }
    private void OnDestroy()
    {
        BattleManager.Instance.RemoveLockTarget(this);
    }
}
