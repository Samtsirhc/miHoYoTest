using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float preinputTime;
    public GameObject myCamera;

    public Vector3 moveDirection;
    private float cameraY;

    #region 攻击参数
    private bool attackPressed;
    private float attackPreTime;
    private int combo;
    private int attackType = 1;
    #endregion

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
        Movement2();
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
                transform.forward = moveDirection;
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

    #region 移动和摄像机
    void Movement()
    {
        if (animator.GetBool(attack_id))
        {
            return;
        }
        if (IsMovePressed())
        {
            moveDirection = Vector3.zero;
        }
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
        if (IsMovePressed())
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
    void Movement2()
    {
        if (animator.GetBool(attack_id))
        {
            return;
        }
        if (IsMovePressed())
        {
            moveDirection = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = GetCameraDirection();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection = -GetCameraDirection(); ;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.Cross(GetCameraDirection(), new Vector3(0, 1, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.Cross(GetCameraDirection(), new Vector3(0, -1, 0));
        }
        if (IsMovePressed())
        {
            animator.SetFloat(move_speed_id, moveSpeed);
            transform.forward = moveDirection.normalized;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat(move_speed_id, 0);
        }
    }

    bool IsMovePressed()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            return true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            return true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            return true;
        }
        return false;
    }
    Vector3 GetCameraDirection()
    {
        Vector3 _directon;
        _directon = transform.position - myCamera.transform.position;
        _directon.y = 0;
        return _directon.normalized;
    }
    #endregion
}
