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
        Destroy(gameObject, 0.1f);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isPlayerDamageZone)
        {
            Enemy _unit = other.GetComponent<Enemy>();
            if (damage.targetUnits.Exists(i => i == _unit))
            {

            }
            else
            {
                damage.targetUnits.Add(_unit);
                _unit.TakeDamage(damage);
                Debug.Log(damage.sourceUnit + " 打到了 " + other.gameObject);
            }
        }
        if (other.gameObject.tag == "Player" && !isPlayerDamageZone)
        {
            Player _unit = other.GetComponent<Player>();
            if (damage.targetUnits.Exists(i => i == _unit))
            {

            }
            else
            {
                damage.targetUnits.Add(_unit);
                _unit.TakeDamage(damage);
                Debug.Log(damage.sourceUnit + " 打到了 " + other.gameObject);
            }
        }

    }
    IEnumerator DestroySelf()
    {
        isDying = true;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
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
            StartCoroutine(DestroySelf());
        }
    }
    
}
