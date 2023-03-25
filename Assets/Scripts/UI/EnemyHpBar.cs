using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Unit enemy => BattleManager.Instance.curLockTarget;
    public GameObject hpBar;
    public RectTransform bgHpBar;
    public RectTransform realHpBar;
    public RectTransform fakeHpBar;
    public Text name;
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
        if (enemy != null)
        {
            Vector2 bg_sz = new Vector2(bgHpBar.rect.width, bgHpBar.rect.height);
            hpBar.SetActive(true);
            Enemy e = enemy as Enemy;

            float real_rate;
            real_rate = e.realHp / e.maxHp;

            float fake_rate;
            fake_rate = e.fakeHp / e.maxHp;

            realHpBar.sizeDelta = new Vector2(bg_sz.x * real_rate, bg_sz.y);
            fakeHpBar.sizeDelta = new Vector2(bg_sz.x * fake_rate, bg_sz.y);

            name.text = e.name;


        }
        else
        {
            hpBar.SetActive(false);
        }
    }
}
