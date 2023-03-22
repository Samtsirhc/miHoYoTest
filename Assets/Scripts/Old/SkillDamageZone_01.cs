using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageZone_01 : MonoBehaviour
{
    public Collider coll;
    private void Awake()
    {
        coll = GetComponent<Collider>();
        StartCoroutine(CloseZone());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy_Old _enemy = other.GetComponent<Enemy_Old>();
            float _distance = Vector3.Distance(_enemy.transform.position, transform.position);
            Vector3 _direction = _enemy.transform.position - transform.position;
            _direction.y = 0;
            float _damage = 0;
            if (_distance < 2)
            {
                _damage = 800;
            }
            else if (_distance < 2.5)
            {
                _damage = 500;
            }
            else if (_distance < 3)
            {
                _damage = 400;
            }
            else if (_distance < 3.5)
            {
                _damage = 300;
            }
            else
            {
                _damage = 200;
            }
            _enemy.TakeDamage(_damage);
            _enemy.HitBack(_direction, 5 - _distance, 5);
        }
    }

    IEnumerator CloseZone()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider>().enabled = false;
    }

}
