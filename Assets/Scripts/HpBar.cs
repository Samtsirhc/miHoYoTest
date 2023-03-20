using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject quickBar;
    public GameObject host;
    public float curRate;
    public float offset = 1.25f;
    public Text text;

    public Enemy_Old enemy;
    private Camera cam;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        //enemy = host.GetComponent<Enemy>();
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition(offset);
        curRate = enemy.hp / enemy.maxHp;
        text.text = enemy.hp.ToString() + "/" + enemy.maxHp.ToString();
        quickBar.GetComponent<Slider>().value = curRate;
        GetComponent<Slider>().value = Mathf.Lerp(GetComponent<Slider>().value, curRate, Time.deltaTime * 2);
        if (curRate < 0.001)
        {
            StartCoroutine(DestorySelf(3f));
        }
    }

    private void _BossHurt(float rate)
    {
        // Debug.Log("BOSS受伤了"+rate);
        curRate = rate;
        //boss_blood_rate = _Resize(rate);
        quickBar.GetComponent<Slider>().value = rate;
    }

    void SetPosition(float height_offset)
    {
        Vector3 hostPos = host.transform.position;
        Vector3 _tmp = hostPos + new Vector3(0, height_offset, 0);
        Vector3 _pos = cam.WorldToScreenPoint(_tmp);
        rectTransform.position = _pos;
    }
    IEnumerator DestorySelf(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
