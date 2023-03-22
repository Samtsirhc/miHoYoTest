using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageZone : MonoBehaviour
{

    public Collider coll;
    public Damage damage;
    public bool isPlayerDamageZone;
    private bool isDying = false;
    public Vector3 initPos;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
        damage = GetComponent<Damage>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Init()
    {
        transform.localPosition = initPos;
        coll.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        StartDying();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isPlayerDamageZone)
        {
            Enemy _unit = other.GetComponent<Enemy>();
            _unit.TakeDamage(damage);
            Debug.Log(damage.sourceUnit + " 打到了 " + other.gameObject);
        }
        if (other.gameObject.tag == "Player" && !isPlayerDamageZone)
        {
            Player _unit = other.GetComponent<Player>();
            _unit.TakeDamage(damage);
            Debug.Log(damage.sourceUnit + " 打到了 " + other.gameObject);
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
