using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public Animator animator;
    public Vector2 attackCD;
    public float attackRange;
    public bool moveToPlayer;
    public GameObject damageZone;
    private bool canAttack = true;
    private bool isMovingToPlayer;
    private bool isPreAct = false;
    public AudioSource audioSource;


    #region ¶¯»­²ÎÊýID
    private int move_speed_id = Animator.StringToHash("moveSpeed");
    private int attack_id = Animator.StringToHash("attack");
    private int die_id = Animator.StringToHash("die");
    private int get_hit_id = Animator.StringToHash("get_hit");
    #endregion

    protected override void Update()
    {
        base.Update();
        Attack();
        MoveToPlayer(stopDis);
    }

    void Attack()
    {
        if (isHittingBack)
        {
            return;
        }
        if (canAttack)
        {
            if (GetPlayerDistance() <= attackRange)
            {
                animator.SetTrigger(attack_id);
                animator.SetFloat(move_speed_id, 0);
                canAttack = false;
                isPreAct = true;
                moveToPlayer = false;
                StartCoroutine(AttackCD());
            }
        }
    }


    IEnumerator AttackCD()
    {
        float _cd = Random.Range(attackCD.x, attackCD.y);
        yield return new WaitForSeconds(_cd);
        canAttack = true;
    }
    protected override void MoveToPlayer(float stop_dis)
    {
        if (isHittingBack)
        {
            return;
        }
        moveDirection = player.transform.position - transform.position;
        moveDirection.y = 0;
        TrunSmooth(moveDirection.normalized);
        if (stop_dis >= GetPlayerDistance() || !canAttack)
        {
            animator.SetFloat(move_speed_id, 0);
            return;
        }
        animator.SetFloat(move_speed_id, 1);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        audioSource.Play();
        if (hp <= 1)
        {
            animator.SetTrigger(die_id);
            return;
        }
        if (!isPreAct)
        {
            animator.SetTrigger(get_hit_id);
            animator.SetFloat(move_speed_id, 0);
        }
    }

    public override void AnimEvt_PreActFinished()
    {
        isPreAct = false;
    }

    public void AnimEvt_DamageZone()
    {
        damageZone.SetActive(true);
    }
}
