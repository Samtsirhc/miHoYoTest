using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float preinputTime;

    private Vector3 moveDirection;
    private bool attackPressed;
    private float attackPreTime;
    private int combo;
    private int attackType = 1;

    #region 状态控制开关
    private bool canMove = true;
    private bool canAttack = true;
    #endregion

    #region 组件
    private Rigidbody rb;
    private Animator animator;
    private CharacterController characterController;
    #endregion


    private int move_speed_id = Animator.StringToHash("moveSpeed");
    private int attack_id = Animator.StringToHash("attack");
    private int combo_id = Animator.StringToHash("combo");
    private int attack_type_id = Animator.StringToHash("attack_type");
    private int start_attack_id = Animator.StringToHash("start_attack");



    private void Awake()
    {
        moveDirection = new Vector3();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
    }

    void AnimStatusCheck()
    {

    }

    void Attack()
    {
        if (attackPressed)
        {
            attackPreTime += Time.deltaTime;
            if (canAttack  && !animator.IsInTransition(0))
            {
                Debug.Log("应用攻击！" + combo);
                combo += 1;
                animator.SetBool(attack_id, true);
                animator.SetInteger(combo_id, combo);
                animator.SetInteger(attack_type_id, attackType);
                attackPressed = false;
                attackPreTime = 0;
                canAttack = false;
                canMove = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            attackPressed = true;
            attackPreTime = 0;
        }
        if (attackPreTime > preinputTime)
        {
            attackPressed = false;
            attackPreTime = 0;
        }
    }

    void TimeToSwitch()
    {
        CanAttack();
    }

    void SwitchAttackType()
    {
        attackType = 2;
    }

    void ComboFinished()
    {
        Debug.Log("结束连招！"  + combo);
        combo = 0;
        attackType = 1;
        animator.SetBool(attack_id, false);
        animator.SetInteger(combo_id, combo);
        animator.SetInteger(attack_type_id, attackType);
        CanAttack();
        Attack();
    }

    void CanAttack()
    {
        if (!animator.IsInTransition(0))
        {
            canAttack = true;
            canMove = true;
        }
        else
        {
            StartCoroutine(CanAttackCheck());
        }
    }

    IEnumerator CanAttackCheck()
    {
        yield return new WaitForFixedUpdate();
        if (!animator.IsInTransition(0))
        {
            canAttack = true;
            canMove = true;
        }
        else
        {
            StartCoroutine(CanAttackCheck());
        }
    }

    void Movement()
    {
        if (animator.GetBool(attack_id))
        {
            return;
        }
        moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += new Vector3(0, 0, -1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection += new Vector3(0, 0, 1);
        }
        if (moveDirection.magnitude > 0.1)
        {
            animator.SetFloat(move_speed_id, moveSpeed);
            transform.forward = moveDirection;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat(move_speed_id, 0);
        }
    }
}
