using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    public List<GameObject> enemyList;

    public CinemachineImpulseSource impulseSource_01;

    public float moveSpeed;
    public float turnSpeed;
    public float preinputTime;
    public float skillCd;
    public float skillCdTimer;
    public float evadeTime;

    private float evadeTimer;

    [Header("相机和Cinemachine、输入")]
    public GameObject myCamera;
    public GameObject cinemachineCameraTarget;
    [Tooltip("你可以把摄像机往上移动多少度 ")]
    public float TopClamp = 70.0f;
    [Tooltip("你可以把摄像机往下移动多少度 ")]
    public float BottomClamp = -30.0f;
    [Tooltip("额外的角度覆盖摄像头。 锁定时微调相机位置有用")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("锁住相机")]
    public bool LockCameraPosition = false;
    private StarterAssetsInputs _input;
    private CharacterController _controller;
    private PlayerInput _playerInput;
    private const float _threshold = 0.01f;

    private bool IsCurrentDeviceMouse = true;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    public Vector3 moveDirection;



    #region 攻击参数
    private bool attackPressed;
    private float attackPreTime;
    private int combo;
    private int attackType = 1;
    public float attackHoldTime;
    public float evadeAttackTime = 1;
    private float attackHoldTimer;
    private float evadeAttackTimer;
    #endregion

    #region 状态控制开关
    private bool canMove = true;
    private bool canAttack = true;
    private bool canEvade = true;
    private bool canSkill = true;
    private bool canBurst = false;
    private bool canSpAttack = false;
    public bool isSkillCd = true;
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
    private int skill_id = Animator.StringToHash("skill");
    private int burst_id = Animator.StringToHash("burst_attack");
    private int sp_attack_id = Animator.StringToHash("sp_attack");
    private int start_sp_attack_id = Animator.StringToHash("start_sp_attack");
    #endregion

    #region Unity函数
    protected override void Awake()
    {
        base.Awake();
        moveDirection = new Vector3();
        attackHoldTimer = 0;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        skillCdTimer = 0;
        isSkillCd = true;
    }
    private void Start()
    {
        InitInputAndCamera();
    }
    // Update is called once per frame
    void Update()
    {
        SpAttack();
        BurstAttack();
        Evade();
        Movement();
        Attack();
        PlaySound();
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    #endregion

    #region 闪避

    void Evade()
    {

        evadeTimer += Time.deltaTime;
        if (!canEvade)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //if (IsMovePressed()) 关闭向前闪避
            if (false)
                {
                Debug.Log("向前闪避！");
                moveDirection = Vector3.zero;
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
            attackPressed = false;
            attackPreTime = 0;
            evadeTimer = 0;

            // 变更攻击连招
            evadeAttackTimer = evadeAttackTime;
            attackType = 2;
        }
    }



    #endregion

    #region 受伤
    public void TakeDamage(float damage)
    {
        Debug.Log("打到了！");
        if (evadeTimer < evadeTime)
        {
            PrefectEvade();
        }
    }

    IEnumerator TimeFreeze(float time)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }

    public void PrefectEvade()
    {
        StartCoroutine(TimeFreeze(0.3f));
        skillCdTimer = 0;
        isSkillCd = true;
        Debug.Log("完美闪避！");
    }
    #endregion

    #region 攻击
    void BurstAttack()
    {
        //if (!canBurst)
        //{
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger(burst_id);
        }
    }

    void SpAttack()
    {
        //if (!canSpAttack)
        //{
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger(start_sp_attack_id);
            animator.SetBool(sp_attack_id, true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            animator.SetBool(sp_attack_id, false);
        }
    }

    void Attack()
    {
        // 闪避变更普攻动作，1秒计时计数后恢复
        evadeAttackTimer -= Time.deltaTime;
        if (evadeAttackTimer <= 0)
        {
            attackType = 1;
        }
        // 废弃长按连招分支
        //if (Input.GetKey(KeyCode.J))
        //{
        //    attackHoldTimer += Time.deltaTime;
        //}
        //if (Input.GetKeyUp(KeyCode.J))
        //{
        //    attackHoldTimer = 0;
        //}
        //if (attackHoldTimer > attackHoldTime && combo >= 2 && combo <= 3 && canAttack)
        //{
        //    Debug.Log("长按生效了" + combo);
        //    GameObject _nearestEnemy = GetNearestEnemy();
        //    if (_nearestEnemy)
        //    {
        //        Vector3 _tmp = _nearestEnemy.transform.position - transform.position;
        //        _tmp.y = 0;
        //        transform.forward = _tmp;
        //    }
        //    else if (IsMovePressed())
        //    {
        //        transform.forward = moveDirection;
        //    }
        //    attackHoldTimer = 0;
        //    attackType = 2;
        //    combo += 1;
        //    animator.SetBool(attack_id, true);
        //    animator.SetInteger(combo_id, combo);
        //    animator.SetInteger(attack_type_id, attackType);
        //    attackPressed = false;
        //    attackPreTime = 0;
        //    canAttack = false;
        //    canMove = false;
        //}
        if (attackPressed)
        {
            attackPreTime += Time.deltaTime;
            if (canAttack)
            {
                //Debug.Log("应用攻击！" + combo);
                GameObject _nearestEnemy = GetNearestEnemy();
                if (_nearestEnemy)
                {
                    Vector3 _tmp = _nearestEnemy.transform.position - transform.position;
                    _tmp.y = 0;
                    transform.forward = _tmp;
                }
                else if (IsMovePressed())
                {
                    StartCoroutine(IE_TrunSmooth(moveDirection, 0.02f));    // 丝滑转向
                    //transform.forward = moveDirection;  // 按照移动方向变化角色方向
                }
                combo += 1;
                animator.SetInteger(attack_type_id, attackType);
                animator.SetInteger(combo_id, combo);
                animator.SetBool(attack_id, true);
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
    #endregion

    #region 移动和摄像机

    void TrunSmooth(Vector3 target) 
    {
        transform.forward += (target - transform.forward) * turnSpeed * Time.deltaTime;
    }

    IEnumerator IE_TrunSmooth(Vector3 target, float delta_time)
    {
        transform.forward += (target - transform.forward) * turnSpeed * delta_time;
        float _dif = (target - transform.forward).magnitude;
        yield return new WaitForSeconds(delta_time);
        if (_dif <= 0.01f)
        {
            IE_TrunSmooth(target, delta_time);
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (moveSpeed <= 2.1f)
            {
                moveSpeed = 5f;
            }
            else
            {
                moveSpeed = 2f;
            }
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
    public Vector3 cameraDirection;
    Vector3 GetCameraDirection()
    {
        Vector3 _directon;
        //_directon = transform.position - myCamera.transform.position;
        _directon = myCamera.transform.forward;
        cameraDirection = myCamera.transform.forward;
        _directon.y = 0;
        return _directon.normalized;
    }
    void InitInputAndCamera()
    {
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _playerInput = GetComponent<PlayerInput>();
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            Debug.Log("x: " + _input.look.x);
            Debug.Log("y: " + _input.look.x);
            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion

    #region 声音
    void PlaySound()
    {
        //try
        //{
        //    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Run" && effectSound.clip != effectSound_Run)
        //    {
        //        effectSound.clip = effectSound_Run;
        //        effectSound.Play();
        //    }
        //    if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Run" && effectSound.clip == effectSound_Run)
        //    {
        //        effectSound.clip = effectSound_Attack_01;
        //    }
        //}
        //finally { }
    }

    void AnimEvt_PlayEffectSound(string sound_name)
    {

    }

    void AnimEvt_PlayGirlSound(string sound_name)
    {

    }
    #endregion

    #region 全局管理
    void UpdateEnemyList()
    {
        GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyList = new List<GameObject>();
        foreach (var item in _enemies)
        {
            if (!item.GetComponent<Enemy_Old>().dead)
            {
                enemyList.Add(item);
            }
        }
    }

    GameObject GetNearestEnemy()
    {
        UpdateEnemyList();
        float _dis = 99999;
        GameObject _enemy = null;
        foreach (GameObject item in enemyList)
        {
            if (Vector3.Distance(item.transform.position, transform.position) < _dis)
            {
                _dis = Vector3.Distance(item.transform.position, transform.position);
                _enemy = item;
            }
        }
        if (_dis > 7.5)
        {
            return null;
        }
        return _enemy;
    }


    IEnumerator SlowUpdate()
    {
        //UpdateEnemyList();
        yield return new WaitForSeconds(1);
        StartCoroutine(SlowUpdate());
    }

    #endregion

    #region 动画事件

    private void AnimEvt_MoveClose()
    {
        Debug.Log("关闭移动");
        canMove = false;
    }
    private void AnimEvt_MoveOpen()
    {
        Debug.Log("打开移动");
        canMove = true;
    }
    private void AnimEvt_SetCombo(int combo)
    {
        Debug.Log("设置combo " + combo);
        this.combo = combo;
    }
    private void AnimEvt_ResetEveryThing()
    {
        canMove = true;
        canAttack = true;
        canEvade = true;
        combo = 0;
    }
    private void AnimEvt_AttackClose()
    {
        canAttack = false;
        animator.SetBool(attack_id, false);
    }
    private void AnimEvt_AttackOpen()
    {
        canAttack = true;
    }
    private void AnimEvt_EvadeClose()
    {
        canEvade = false;
    }
    private void AnimEvt_EvadeOpen()
    {
        canEvade = true;
    }
    void AnimEvt_SkillFinished()
    {
        canAttack = true;
        canMove = true;
        canEvade = true;
        canSkill = true;
    }
    void AnimEvt_AttackDamage(int index)
    {
        Debug.Log("Damage!");
    }

    void AnimEvt_TimeToSwitch(int is_all_combo_finish) // 可以接下一段攻击了
    {
        if (is_all_combo_finish != 0)
        {
            combo = 0;
        }
        canAttack = true;
    }

    void AnimEvt_SwitchAttackType() // 废弃
    {
        attackType = 2;
    }
    void AnimEvt_ComboFinished()
    {
        //Debug.Log("结束连招！"  + combo);
        combo = 0;
        attackType = 1;
        animator.SetBool(attack_id, false);
        animator.SetInteger(combo_id, combo);
        animator.SetInteger(attack_type_id, attackType);
        canAttack = true;
        Attack();
        canMove = true;
    }
    #endregion
}
