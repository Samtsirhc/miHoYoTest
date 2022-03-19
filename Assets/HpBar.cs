using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject quickBar;
    public GameObject host;
    public float curRate;

    private Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = host.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        curRate = enemy.hp / enemy.maxHp;
        quickBar.GetComponent<Slider>().value = curRate;
        GetComponent<Slider>().value = Mathf.Lerp(GetComponent<Slider>().value, curRate, Time.deltaTime * 2);
    }

    private void _BossHurt(float rate)
    {
        // Debug.Log("BOSS ‹…À¡À"+rate);
        curRate = rate;
        //boss_blood_rate = _Resize(rate);
        quickBar.GetComponent<Slider>().value = rate;
    }
}
