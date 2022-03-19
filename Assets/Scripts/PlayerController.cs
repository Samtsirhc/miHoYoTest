using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float preinputTime;
    public GameObject myCamera;

    public Vector3 moveDirection;
    public float attackHoldTime;
    private float cameraY;


    #region 攻击参数
    private bool attackPressed;
    private float attackPreTime;
    private int combo;
    private int attackType = 1;
    private float attackHoldTimer;
    #endregion

    #region 状态控制开关
    private bool canMove = true;
    private bool canAttack = true;
    private bool canEvade = true;
    #endregion

    #region 组件
    private Rigidbody rb;
    private Animator animator;
    private CharacterController characterController;
    #endregion

    #region 动画参数ID
    private int move_speed_id = Animator.StringToHash("moveSpeed");
    private int attack_id = Animator.StringToHash("attack");
    private int combo_id = Animator.StringToHash("combo");
    private int attack_type_id = Animator.StringToHash("attack_type");
    private int evade_backward_id = Animator.StringToHash("evade_backward");
    private int evade_forward_id = Animator.StringToHash("evade_forward");
    #endregion


    private void Awake()
    {
        moveDirection = new Vector3();
        attackHoldTimer = 0;
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
        Evade();
        Movement();
        Attack();
    }

    #region 闪避

    void Evade()
    {
        if (!canEvade)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (IsMovePressed())
            {
                Debug.Log("向前闪避！");
                animator.SetTrigger(evade_forward_id);
            }
            else
            {
                Debug.Log("向后闪避！");
                animator.SetTrigger(evade_backward_id);
            }
            AnimEvt_ComboFinished();
            canMove = false;
            canAttack = false;
            canEvade = false;
        }
    }

    void AnimEvt_EvadeFinished()
    {
        Debug.Log("闪避结束");
        canMove = true;
        canAttack = true;
        canEvade = true;
    }

    #endregion

    #region 攻击
    void Attack()
    {
        if (Input.GetKey(KeyCode.J))
        {
            attackHoldTimer += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            attackHoldTimer = 0;
        }
        if (attackHoldTimer > attackHoldTime && combo >= 2 && combo <= 3 && canAttack)
        {
            Debug.Log("长按生效了" + combo);
            if (IsMovePressed())
            {
                transform.forward = moveDirection;
            }
            attackHoldTimer = 0;
            attackType = 2;
            combo += 1;
            animator.SetBool(attack_id, true);
            animator.SetInteger(combo_id, combo);
            animator.SetInteger(attack_type_id, attackType);
            attackPressed = false;
            attackPreTime = 0;
            canAttack = false;
            canMove = false;
        }
        if (attackPressed)
        {
            attackPreTime += Time.deltaTime;
            if (canAttack)
            {
                Debug.Log("应用攻击！" + combo);
                if (IsMovePressed())
                {
                    transform.forward = moveDirection;
                }
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

    void AnimEvt_DamageZone()
    {

    }

    void AnimEvt_TimeToSwitch()
    {
        CanAttack();
    }

    void AnimEvt_SwitchAttackType()
    {
        attackType = 2;
    }

    void AnimEvt_ComboFinished()
    {
        Debug.Log("结束连招！"  + combo);
        combo = 0;
        attackType = 1;
        animator.SetBool(attack_id, false);
        animator.SetInteger(combo_id, combo);
        animator.SetInteger(attack_type_id, attackType);
        CanAttack();
        Attack();
        canMove = true;
    }

    void CanAttack()
    {
        canAttack = true;
    }
    #endregion

    #region 移动和摄像机

    void TrunSmooth(Vector3 target) 
    {
        transform.forward += (target - transform.forward) * turnSpeed * Time.deltaTime;
    }

    void Movement_abandoned()
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
    void Movement()
    {
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
        if (animator.GetBool(attack_id))
        {
            return;
        }
        if (IsMovePressed() && canMove)
        {
            //Debug.Log("在移动");
            animator.SetFloat(move_speed_id, moveSpeed);
            if (canMove)
            {
                TrunSmooth(moveDirection.normalized);
                characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat(move_speed_id, 0);
        }
    }

    bool IsMovePressed()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
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
