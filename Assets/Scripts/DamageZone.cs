using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageZone : MonoBehaviour
{

    public Collider coll;

    private bool isDying = false;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartDying();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy_Old _enemy = other.GetComponent<Enemy_Old>();
            _enemy.TakeDamage(100);
            Debug.Log(other.gameObject);
        }

    }
    IEnumerator DisableSelf()
    {
        isDying = true;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        isDying = false;
    }

    void StartDying()
    {
        if (isDying)
        {
            return;
        }
        else
        {
            StartCoroutine(DisableSelf());
        }
    }
    
}
