using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonsLeft : MonoBehaviour
{
    public PlayerController playerController => BattleManager.Instance.playerController;
    public Player player => BattleManager.Instance.player as Player;
    public Image Q_1;
    public Image Q_2;
    public Image E;
    public Image W;
    public Image A;
    public Image S;
    public Image D;
    private void FixedUpdate()
    {
        W.color = Color.white;
        if (Input.GetKey(KeyCode.W))
        {
            W.color = new Color(0.3f,0.77f,1f);
        }
        A.color = Color.white;
        if (Input.GetKey(KeyCode.A))
        {
            A.color = new Color(0.3f, 0.77f, 1f);
        }
        S.color = Color.white;
        if (Input.GetKey(KeyCode.S))
        {
            S.color = new Color(0.3f, 0.77f, 1f);
        }
        D.color = Color.white;
        if (Input.GetKey(KeyCode.D))
        {
            D.color = new Color(0.3f, 0.77f, 1f);
        }

        if (playerController.canBurstAttack)
        {
            E.color = new Color(1f, 0.84f, 0.25f);
        }
        else
        {
            E.color = Color.gray;
        }
        Q_1.color = Color.gray;
        Q_2.rectTransform.sizeDelta = new Vector2(100, 100 * player.energy / 1000f);

    }

}
