using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float preinputTime;

    [SerializeField]
    private Vector3 moveDirection;
    private bool attackPressed;
    private float attackPreTime;
    private int combo;

    private Rigidbody rb;
    private Animator animator;


    private int move_speed_id = Animator.StringToHash("moveSpeed");
    private int attack_id = Animator.StringToHash("attack");
    private int combo_id = Animator.StringToHash("combo");



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

    void Attack()
    {
   
        if (Input.GetKeyDown(KeyCode.J))
        {
            attackPressed = true;
            combo = 1;
            attackPreTime = 0;
        }
        if (attackPreTime > preinputTime)
        {
            attackPressed = false;
            attackPreTime = 0;
        }
        attackPreTime += Time.deltaTime;
        animator.SetBool(attack_id, attackPressed);
        animator.SetInteger(combo_id, combo);
    }

    void ComboFinished()
    {
        combo = 0;
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
