using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHp;
    public float hp
    {
        get { return _hp; }
        set
        {
            if (value <= 0)
            {
                _hp = 0;
            }
            else
            {
                _hp = value;
            }
        }
    }
    private float _hp;
    public GameObject hpBarPfb;
    public bool dead;
    public float moveSpeed;
    public Vector3 moveDirection;
    public float turnSpeed;
    public float stopDis;

    protected PlayerController player;
    protected bool isHittingBack;
    protected CharacterController characterController;

    private float value = 1;
    private Vector3 hitBackStart;
    private float hitBackFactor = 5;
    private Vector3 hitBackDirection;
    private float hitBackDistance;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        GameObject hpBar = Instantiate(hpBarPfb, GameObject.Find("Canvas").transform);
        hpBar.GetComponent<HpBar>().enemy = this;
        hpBar.GetComponent<HpBar>().host = gameObject;
        characterController = GetComponent<CharacterController>();
        player = PlayerController.Instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HitBackMove();
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log(damage);
        hp -= damage;
        if (hp <= 0.1)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        dead = true;
        StartCoroutine(DestorySelf(3));
    }

    public void HitBack(Vector3 direction, float distance, float factor)
    {
        hitBackDirection = direction;
        hitBackDistance = distance;
        isHittingBack = true;
        value = 1;
        hitBackStart = transform.position;
    }

    protected float GetPlayerDistance()
    {
        Vector3 _dis = transform.position - player.transform.position;
        _dis.y = 0;
        return _dis.magnitude;
    }

    protected virtual void HitBackMove()
    {
        if (isHittingBack)
        {

            value = Mathf.Lerp(value, 0, Time.deltaTime * hitBackFactor);
            Vector3 pos = (1 - value) * hitBackDistance * hitBackDirection.normalized + hitBackStart;
            transform.position = pos;
        }
        if (value < 0.05)
        {
            isHittingBack = false;
        }
    }

    protected virtual void MoveToPlayer(float stop_dis)
    {
        moveDirection = player.transform.position - transform.position;
        moveDirection.y = 0;
        TrunSmooth(moveDirection.normalized);
        if (stop_dis >= GetPlayerDistance())
        {
            return;
        }
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    protected void TrunSmooth(Vector3 target)
    {
        transform.forward += (target - transform.forward) * turnSpeed * Time.deltaTime;
    }
    IEnumerator DestorySelf(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }


    public virtual void AnimEvt_PreActFinished()
    {

    }
}
