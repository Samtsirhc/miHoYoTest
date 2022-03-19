using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float hp = 600;
    public GameObject hpBarPfb;
    public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
        GameObject hpBar = Instantiate(hpBarPfb, GameObject.Find("Canvas").transform);
        hpBar.GetComponent<HpBar>().enemy = this;
        hpBar.GetComponent<HpBar>().host = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0.1)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;
        StartCoroutine(DestorySelf(3));
    }

    IEnumerator DestorySelf(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
