using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField]
    private Vector3 moveDirection;

    private Rigidbody rb;
    private Animator animator;


    private int move_speed_id = Animator.StringToHash("moveSpeed");



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
