using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageZone_01 : MonoBehaviour
{
    public Collider coll;
    private void Awake()
    {
        coll = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy _enemy = other.GetComponent<Enemy>();
            float _distance = Vector3.Distance(_enemy.transform.position, transform.position);
            float _damage = 0;
            if (_distance < 2)
            {
                _damage = 400;
            }
            else if (_distance < 2.5)
            {
                _damage = 200;
            }
            else if (_distance < 3)
            {
                _damage = 150;
            }
            else if (_distance < 3.5)
            {
                _damage = 100;
            }
            else
            {
                _damage = 50;
            }
            _enemy.TakeDamage(_damage);
            Debug.Log(other.gameObject);
        }
    }



}
