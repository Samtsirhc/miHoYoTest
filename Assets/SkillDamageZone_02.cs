using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageZone_02 : MonoBehaviour
{
    public Collider coll;
    float scale = 1;
    private void Awake()
    {
        coll = GetComponent<Collider>();
        StartCoroutine(Shrink());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy _enemy = other.GetComponent<Enemy>();
            _enemy.TakeDamage(200);
            Debug.Log(other.gameObject);
        }
    }

    IEnumerator Shrink()
    {
        if (scale > 0.01f)
        {
            scale -= 0.02f;
        }
        transform.localScale = Vector3.one * scale * 5;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(Shrink());
    }

}
