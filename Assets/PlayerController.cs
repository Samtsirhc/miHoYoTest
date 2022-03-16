using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float preinputTime;

    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private bool attackPressed;
    [SerializeField]
    private float attackPreTime;
    [SerializeField]
    private int combo;
    [SerializeField]
    private int attackType;
    [SerializeField]
    private bool canAttack = true;

    private Rigidbody rb;
    private Animator animator;


    private int move_speed_id = Animator.StringToHash("moveSpeed");
    private int attack_id = Animator.StringToHash("attack");
    private int combo_id = Animator.StringToHash("combo");
    private int attack_type_id = Animator.StringToHash("attack_type");



    private void Awake()
    {
        moveDirection = new Vector3();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        if (attackPressed)
        {
            attackPreTime += Time.deltaTime;
            if (canAttack)
            {
                combo += 1;
                attackType = 1;
                animator.SetBool(attack_id, true);
                animator.SetInteger(combo_id, combo);
                animator.SetInteger(attack_type_id, attackType);
                attackPressed = false;
                canAttack = false;
            }
        }

    }

    void ComboFinished()
    {
        combo = 0;
        attackType = 0;
        animator.SetBool(attack_id, false);
        animator.SetInteger(combo_id, combo);
        animator.SetInteger(attack_type_id, attackType);
    }

    void CanAttack()
    {
        canAttack = true;
    }


    void Movement()
    {
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
            rb.velocity = moveDirection.normalized * moveSpeed;
            transform.forward = moveDirection;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        animator.SetFloat(move_speed_id, rb.velocity.magnitude);
    }
}
