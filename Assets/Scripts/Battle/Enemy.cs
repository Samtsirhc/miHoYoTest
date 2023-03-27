using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    #region  属性 真假血量 血量恢复
    public string unit_name = "示例敌人";
    [Header("战斗相关")]
    public float maxHp = 1000f;
    public float realHp
    {
        get => _realHp;
        set
        {
            if (value <= 0)
            {
                _realHp = 0;
                Die();
            }
            else
            {
                _realHp = value;
            }
        }
    }
    private float _realHp;

    public float fakeToRealRate = 0.5f;
    public float fakeHp
    {
        get => _fakeHp;
        set
        {
            if (value >= realHp)
            {
                _fakeHp = realHp;
            }
            else if (value <= 0)
            {
                float v = - value - _fakeHp;
                realHp -= v * fakeToRealRate;
                _fakeHp = 0;
            }
            else
            {
                _fakeHp = value;
            }
        }
    }
    private float _fakeHp;

    public float startRecoverTime = 10f;
    private float recoverTimer = 0;
    public float recoverRate = 30f;
    #endregion

    #region Unity函数
    private void Start()
    {
        Init();
        StartCoroutine(sss());
    }
    IEnumerator sss()
    {
        AddAttackGoal(clips[Random.Range(0, clips.Count)].name, 2.5f, 30);
        yield return new WaitForSeconds(6);
        StartCoroutine(sss());
    }
    private void FixedUpdate()
    {
        RecoverFakeHp();
        Move();
        AIUpdate();
        if (weakFlag)
        {
            weakObj.SetActive(true);
        }
        else
        {
            weakObj.SetActive(false);
        }
    }
    #endregion

    #region 战斗相关
    public Player player => BattleManager.Instance.player as Player;
    public List<GameObject> damageZones;
    public override void Init()
    {
        base.Init();
        realHp = maxHp;
        fakeHp = maxHp;
    }
    public override void Die()
    {
        base.Die();
        animator.SetTrigger("die");
    }
    public void RecoverFakeHp() // 在fixed update中使用
    {
        recoverTimer -= Time.fixedDeltaTime;
        if (recoverTimer <= 0)
        {
            fakeHp += recoverRate * Time.fixedDeltaTime;
        }
        else
        {
            fakeHp += recoverRate * Time.fixedDeltaTime * 0.25f;
        }
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        recoverTimer = startRecoverTime;

        AudioManager.Instance.PlayAudio($"主角打击{Random.Range(1, 6)}");

        float fake_dmg;
        fake_dmg = damage.fakeDamage;
        if (weakFlag)   // 是否是弱点
        {
            fakeHp -= fake_dmg * 1.5f;
            Debug.Log("结构伤害: " + fake_dmg * 1.5f + " " + damage.sourceUnit + " => " + this);
        }
        else
        {
            fakeHp -= fake_dmg;
            Debug.Log("结构伤害: " + fake_dmg + " " + damage.sourceUnit + " => " + this);
        }

        float hp_dif = realHp - fakeHp;
        float real_dmg;
        if (damage.realDamage >= hp_dif)
        {
            real_dmg = hp_dif;
        }
        else
        {
            real_dmg = damage.realDamage;
        }
        realHp -= real_dmg;
        Debug.Log("消解伤害: " + real_dmg + " " + damage.sourceUnit + " => " + this);

        Player p = damage.sourceUnit as Player;
        p.nuclearValue += damage.getNuclearValue;

        // 判断是否打断
        if (damage.unbalanceLevel - curBalanceLevel >= 10)
        {
            Hit_H();
        }
        else if (weakFlag && damage.unbalanceLevel - curBalanceLevel + 10 > 0)
        {
            // 处于弱点的时候 可以打断攻击
            animator.speed = animSpeed;
            weakFlag = false;
            Hit_H();
        }
        else if (damage.unbalanceLevel - curBalanceLevel > 0)
        {
            Hit_L();
        }
    }
    public void HitReact(int level)
    {
        if (level - curBalanceLevel > 10)
        {
            Hit_H();
        }
        else if (level - curBalanceLevel > 0)
        {
            Hit_L();
        }
    }
    public void Hit_L()
    {
        animator.SetBool(hit_l_id, true);
        animator.SetTrigger(hit_l_trigger_id);
        animator.SetBool(is_attack_id, false);
    }
    public void Hit_H()
    {
        animator.SetBool(hit_h_id, true);
        animator.SetTrigger(hit_h_trigger_id);
        animator.SetBool(is_attack_id, false);
    }
    public bool weakFlag = false;
    public float weakDuration = 2f;
    public GameObject weakObj;
    public void ShowWeak()
    {
        StartCoroutine(SlowDownAnim(0.1f, 1f, 1.5f));
    }
    public float animSpeed = 1;
    IEnumerator SlowDownAnim(float speed, float duration_1, float duration_2)
    {
        weakFlag = true;
        animSpeed = animator.speed;
        animator.speed = speed;
        yield return new WaitForSeconds(duration_1);
        animator.speed = animSpeed;
        yield return new WaitForSeconds(duration_2 - duration_1);
        weakFlag = false;
    }

    #endregion

    #region 动画参数ID
    private int attack_id = Animator.StringToHash("attack");
    private int is_attack_id = Animator.StringToHash("is_attack");
    private int hit_l_id = Animator.StringToHash("hit_l");
    private int hit_h_id = Animator.StringToHash("hit_h");
    private int hit_l_trigger_id = Animator.StringToHash("hit_l_trigger");
    private int hit_h_trigger_id = Animator.StringToHash("hit_h_trigger");
    private int die_id = Animator.StringToHash("die");
    #endregion

    #region 动画相关
    public Animator animator;
    public override void AnimEvt_AttackDamage(int index)
    {
        base.AnimEvt_AttackDamage(index);
        CreatDamageZone(damageZones[index]);
    }
    public void AnimEvt_Die()
    {
        Destroy(gameObject);
    }
    public void AnimEvt_StopHit()
    {
        animator.SetBool(hit_l_id, false);
        animator.SetBool(hit_h_id, false);
    }
    public void AnimEvt_MoveClose()
    {
        canMove = false;
    }
    public void AnimEvt_MoveOpen()
    {
        canMove = true;
    }
    public void AnimEvt_AttackClose()
    {
        canAttack = false;
    }
    public void AnimEvt_AttackOpen()
    {
        canAttack = true;
    }
    public void AnimEvt_AttackStart(int balance_level)
    {
        canAttack = false;
        canMove = false;
        animator.SetBool(is_attack_id, true);
        curBalanceLevel = balance_level;
    }
    public void AnimEvt_AttackEnd()
    {
        canAttack = true;
        canMove = true;
        animator.SetBool(is_attack_id, false);
        curBalanceLevel = constantBalanceLevel;
    }

    // 要替换Clip
    public List<AnimationClip> clips;
    public string clip_name = "Die";

    [EditorButton]
    public void Attack(string clip_name)
    {
        animator.Play(clip_name);
    }
    #endregion

    #region 状态控制开关
    private bool canMove = true;
    private bool canAttack = true;
    #endregion

    #region AI相关 都在Fix中使用
    public CharacterController characterController;
    [Header("AI控制参数")]
    public bool isAIWorking = true;
    public float runSpeed = 2;
    public float walkSpeed = 2.5f;
    public float strategyInterval = 1;
    public float attackBrainInterval = 3;
    private float moveSpeed;
    private Vector3 moveDirection;
    private Vector3 faceDirection;

    private void Move()
    {

        if (!canMove)
        {
            moveSpeed = 0;
        }
        TrunSmooth(faceDirection.normalized);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    [Header("AI显示参数，拒绝修改")]
    public float disToPlayer;
    public int curStrategy;
    private void AIUpdate()
    {
        if (!isAIWorking)
        {
            return;
        }
        Vector3 _dis = player.transform.position - transform.position;
        _dis.y = 0;
        disToPlayer = _dis.magnitude;

        AIStrategy();
        AIAttackBrain();
        AIMovement();
        AIAttack();
    }

    private float strategyTimer;
    private void AIStrategy()
    {
        // 0 是进攻 1 是对峙 2 是靠近 3 是远离
        // 打破策略的条件
        if (curStrategy == 2 && disToPlayer <= 5.5)
        {
            // 完成靠近任务
            strategyTimer = -1;
        }
        if (curStrategy == 3 && disToPlayer >= 7.5)
        {
            // 完成远离任务
            strategyTimer = -1;
        }
        // 重置计时器
        strategyTimer -= Time.fixedDeltaTime;
        if (strategyTimer >= 0)
        {
            return;
        }
        strategyTimer = strategyInterval;


        if (disToPlayer >= 10)
        {
            // 太远了先靠近
            curStrategy = 2;
        }
        else if(disToPlayer >= 6)
        {
            if (curStrategy == 1)
            {
                // 已经是对峙，有概率切攻击
                var dice = Random.Range(0, 100);
                if (dice >= 50)
                {
                    curStrategy = 0;
                }           
            }
            else
            {
                // 不是对峙先对峙
                curStrategy = 1;
            }
        }
        else if (disToPlayer >= 2.5)
        {
            if (curStrategy == 0)
            {
                // 已经是攻击，有概率切远离
                var dice = Random.Range(0, 100);
                if (dice >= 20)
                {
                    curStrategy = 3;
                }
            }
        }
        else
        {
            float dice_1 = 1 - (disToPlayer / 6);
            int dice_2 = (int)(dice_1 * 100);
            if (Random.Range(1,100) < dice_2)
            {
                curStrategy = 3;
            }
        }
    }
    private void AIMovement()
    {
        faceDirection = player.transform.position - transform.position;
        faceDirection.y = 0;
        // 0 是进攻 1 是对峙 2 是靠近 3 是远离
        if (curStrategy == 0)
        {
            moveDirection = player.transform.position - transform.position;
            moveDirection.y = 0;
            moveSpeed = runSpeed;
        }
        if (curStrategy == 1)
        {
            moveDirection = player.transform.position - transform.position;
            moveDirection.y = 0;
            moveDirection = Vector3.Cross(moveDirection, new Vector3(0, 1, 0));
            moveSpeed = walkSpeed;
        }
        if (curStrategy == 2)
        {
            moveDirection = player.transform.position - transform.position;
            moveDirection.y = 0;
            moveSpeed = walkSpeed;
        }
        if (curStrategy == 3)
        {
            moveDirection = - player.transform.position + transform.position;
            moveDirection.y = 0;
            moveSpeed = walkSpeed;
        }
    }

    private float attackBrainTimer;
    private void AIAttackBrain()
    {
        attackBrainTimer -= Time.fixedDeltaTime;
        if (attackBrainTimer >= 0)
        {
            return;
        }
        attackBrainTimer = attackBrainInterval;
    }
    public List<AttackGoal> attackGoals = new List<AttackGoal>();
    private void AIAttack()
    {
        if (canAttack)
        {
            foreach (var item in attackGoals)
            {
                if (disToPlayer <= item.attackDis)
                {
                    Attack(item.clip.name);
                    attackGoals.Remove(item);
                    break;
                }
            }
        }
        // 清除到时间的
        foreach (var item in attackGoals)
        {
            item.durationTimer -= Time.fixedDeltaTime;
        }
        attackGoals.RemoveAll(i => i.durationTimer <= 0);
    }
    public void AddAttackGoal(string clip_name, float dis, float duration, int priority = 0)
    {
        if (attackGoals.Exists(i => i.name == clip_name))
        {
            attackGoals.Find(i => i.name == clip_name).ResetTimer();
            return;
        }
        AnimationClip clip = clips.Find(i => i.name == clip_name);
        attackGoals.Add(new AttackGoal(clip, dis, duration, priority));
        attackGoals.Sort((x, y) => -x.Compare(x, y));
    }
    private void ClearAttackGoal()
    {
        attackGoals = new List<AttackGoal>();
    }
    #endregion

    #region AI Attack Goal
    public class AttackGoal : Comparer<AttackGoal>
    {
        public float attackDis;
        public float duration;
        public int priority;
        public float durationTimer;
        public string name;
        public AnimationClip clip;
        
        public AttackGoal(AnimationClip clip, float dis, float duration, int priority)
        {
            this.clip = clip;
            attackDis = dis;
            this.duration = duration;
            durationTimer = duration;
            this.priority = priority;
            name = clip.name;
        }
        public void ResetTimer()
        {
            durationTimer = duration;
        }

        public override int Compare(AttackGoal x, AttackGoal y)
        {
            if (x.priority > y.priority)
            {
                return 1;
            }
            else if (x.priority == y.priority)
            {
                return 0;
            }
            return -1;
        }
    }
    #endregion

    void TrunSmooth(Vector3 target)
    {
        transform.forward += (target - transform.forward) * 35 * Time.deltaTime;
    }
}
