using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonsRight : MonoBehaviour
{
    public PlayerController playerController => BattleManager.Instance.playerController;
    public Player player => BattleManager.Instance.player as Player;
    public Image J;
    public Image K;
    public Image Lock;

    private void FixedUpdate()
    {
        J.color = Color.white;
        if (Input.GetKey(KeyCode.J) || Input.GetMouseButton(0))
        {
            J.color = new Color(0.3f, 0.77f, 1f);
        }
        K.color = Color.white;
        if (Input.GetKey(KeyCode.K) || Input.GetMouseButton(1))
        {
            K.color = new Color(0.3f, 0.77f, 1f);
        }
        Lock.color = Color.white;
        if (playerController.lockTarget != null)
        {
            Lock.color = new Color(0.3f, 0.77f, 1f);
        }


    }
}
