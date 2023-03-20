using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    public GameObject myCamera;


    public GameObject damageZone_01;
    public GameObject damageZone_02;
    public GameObject damageZone_03;
    public GameObject damageZone_04;

    public Vector3 moveDirection;
    public float attackHoldTime;

    #region 声音文件
    public AudioSource girlSound;
    public AudioSource effectSound;
    public AudioClip girlSound_01_01;
    public AudioClip girlSound_01_02;
    public AudioClip girlSound_01_03;
    public AudioClip girlSound_01_04;
    public AudioClip girlSound_01_05;
    public AudioClip girlSound_02_03;
    public AudioClip girlSound_02_04;
    public AudioClip effectSound_Attack_01;
    public AudioClip effectSound_Attack_02;
    public AudioClip effectSound_Attack_04;
    public AudioClip effectSound_Attack_05;
    public AudioClip effectSound_Run;
    public AudioClip effectSound_Evade;
    #endregion

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
    private bool canSkill = true;
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
    #endregion

    #region 脚本函数
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

    // Update is called once per frame
    void Update()
    {
        Skill();
        Evade();
        Movement();
        Attack();
        PlaySound();
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
            if (IsMovePressed())
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
        }
    }

    void AnimEvt_EvadeFinished()
    {
        Debug.Log("闪避结束");
        canMove = true;
        canAttack = true;
        canEvade = true;
        attackPressed = false;
        attackPreTime = 0;
        AnimEvt_ComboFinished();
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
    void Skill()
    {
        if (skillCdTimer <= 0)
        {
            isSkillCd = true;
        }
        if (!isSkillCd)
        {
            skillCdTimer -= Time.deltaTime;
        }
        if (!isSkillCd || !canSkill)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.SetTrigger(skill_id);
            canAttack = false;
            canMove = false;
            canEvade = false;
            canSkill = false;
            isSkillCd = false;
            skillCdTimer = skillCd;
        }
    }

    void AnimEvt_SkillFinished()
    {
        canAttack = true;
        canMove = true;
        canEvade = true;
        canSkill = true;
    }

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
            GameObject _nearestEnemy = GetNearestEnemy();
            if (_nearestEnemy)
            {
                Vector3 _tmp = _nearestEnemy.transform.position - transform.position;
                _tmp.y = 0;
                transform.forward = _tmp;
            }
            else if (IsMovePressed())
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
    void AnimEvt_AttackDamage(int index)
    {
        switch (index)
        {
            case 1:
                damageZone_01.SetActive(true);
                break;
            case 2:
                damageZone_02.SetActive(true);
                break;
            case 3:
                damageZone_03.SetActive(true);
                break;
            case 4:
                damageZone_04.SetActive(true);
                break;
            default:
                break;
        }
        impulseSource_01.GenerateImpulse();
    }
    void AnimEvt_Skill_01()
    {

    }
    void AnimEvt_Skill_02()
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
        //Debug.Log("结束连招！"  + combo);
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

    #region 声音
    void PlaySound()
    {
        try
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Run" && effectSound.clip != effectSound_Run)
            {
                effectSound.clip = effectSound_Run;
                effectSound.Play();
            }
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Run" && effectSound.clip == effectSound_Run)
            {
                effectSound.clip = effectSound_Attack_01;
            }
        }
        finally { }
    }

    void AnimEvt_PlayEffectSound(string sound_name)
    {
        switch (sound_name)
        {
            case "effectSound_Evade":
                effectSound.clip = effectSound_Evade;
                break;
            case "effectSound_Attack_01":
                effectSound.clip = effectSound_Attack_01;
                break;
            case "effectSound_Attack_02":
                effectSound.clip = effectSound_Attack_02;
                break;
            case "effectSound_Attack_04":
                effectSound.clip = effectSound_Attack_04;
                break;
            case "effectSound_Attack_05":
                effectSound.clip = effectSound_Attack_05;
                break;
            default:
                break;
        }
        effectSound.Play();
    }

    void AnimEvt_PlayGirlSound(string sound_name)
    {
        switch (sound_name)
        {
            case "girlSound_01_01":
                girlSound.clip = girlSound_01_01;
                break;
            case "girlSound_01_02":
                girlSound.clip = girlSound_01_02;
                break;
            case "girlSound_01_03":
                girlSound.clip = girlSound_01_03;
                break;
            case "girlSound_01_04":
                girlSound.clip = girlSound_01_04;
                break;
            case "girlSound_01_05":
                girlSound.clip = girlSound_01_05;
                break;
            case "girlSound_02_03":
                girlSound.clip = girlSound_02_03;
                break;
            case "girlSound_02_04":
                girlSound.clip = girlSound_02_04;
                break;
            default:
                break;
        }
        girlSound.Play();
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
}
