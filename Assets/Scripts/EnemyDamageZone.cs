using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDamageZone : MonoBehaviour
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
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.TakeDamage(100);
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
