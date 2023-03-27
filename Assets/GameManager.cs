using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPfb;
    public GameObject enemySpawnPoint;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            // 召唤新敌人
            Instantiate(enemyPfb, enemySpawnPoint.transform).transform.localPosition = new Vector3();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            // 刷新自己状态
            var p = BattleManager.Instance.player as Player;
            p.Refresh();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            // 满能量
            var p = BattleManager.Instance.player as Player;
            p.energy = 1000;

        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            // 杀死所有敌人
            foreach (var item in BattleManager.Instance.lockTargets)
            {
                item.GetComponentInParent<Enemy>().Die();
            }
        }
    }
}
