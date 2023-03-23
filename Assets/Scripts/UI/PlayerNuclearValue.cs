using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNuclearValue : MonoBehaviour
{
    public Player player => BattleManager.Instance.player as Player;
    public Text hp;
    public RectTransform bgL;
    public RectTransform vL;
    public RectTransform bgR;
    public RectTransform vR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        hp.text = player.vectorCrystalNum.ToString();
        bgL.sizeDelta = new Vector2(player.nuclearValueUpperLimit / 2, 45);
        bgR.sizeDelta = new Vector2(player.nuclearValueUpperLimit / 2, 45);
        vL.sizeDelta = new Vector2(player.nuclearValue / 2, 45);
        vR.sizeDelta = new Vector2(player.nuclearValue / 2, 45);
    }
}
