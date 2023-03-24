using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    #region 真假血量 血量恢复
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

    public float fakeHp
    {
        get => _fakeHp;
        set
        {
            if (value >= realHp)
            {
                _fakeHp = realHp;
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
    }
    private void FixedUpdate()
    {
        RecoverFakeHp();
        Move();
    }
    #endregion

    #region 战斗相关
    public Player player => BattleManager.Instance.player as Player;
    public override void Init()
    {
        base.Init();
        realHp = maxHp;
        fakeHp = maxHp;
    }
    public void RecoverFakeHp() // 在fixed update中使用
    {
        recoverTimer -= Time.fixedDeltaTime;
        if (recoverTimer <= 0)
        {
            fakeHp += recoverRate * Time.fixedDeltaTime;
        }
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        recoverTimer = startRecoverTime;

        float fake_dmg;
        if (damage.fakeDamage >= fakeHp)
        {
            fake_dmg = fakeHp;
        }
        else
        {
            fake_dmg = damage.fakeDamage;
        }
        fakeHp -= fake_dmg;
        Debug.Log("结构伤害: " + fake_dmg + " " + damage.sourceUnit + " => " + this);

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
            animator.SetBool(hit_h_id, true);
        }
        else if (damage.unbalanceLevel - curBalanceLevel > 0)
        {
            animator.SetBool(hit_l_id, true);
            animator.SetTrigger(hit_l_trigger_id);
        }
    }
    #endregion

    #region 动画参数ID
    private int attack_id = Animator.StringToHash("attack");
    private int is_attack_id = Animator.StringToHash("is_attack");
    private int hit_l_id = Animator.StringToHash("hit_l");
    private int hit_h_id = Animator.StringToHash("hit_h");
    private int hit_l_trigger_id = Animator.StringToHash("hit_l_trigger");
    private int die_id = Animator.StringToHash("die");
    #endregion

    #region 动画相关
    public Animator animator;
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
    public void AnimEvt_AttackStart()
    {
        canAttack = false;
        canMove = false;
        animator.SetBool(is_attack_id, true);
    }
    public void AnimEvt_AttackEnd()
    {
        canAttack = true;
        canMove = true;
        animator.SetBool(is_attack_id, false);
    }

    // 要替换Clip
    public AnimationClip clip;
    public string clip_name = "Die";

    [EditorButton]
    public void Attack(AnimationClip clip)
    {
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[animator.layerCount];
        for (int i = 0; i < animator.layerCount; i++)
        {
            layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
        }

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        overrideController[clip_name] = clip;

        animator.runtimeAnimatorController = overrideController;
        animator.Update(0.0f);
        animator.SetTrigger(attack_id);
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }

        clip_name = clip.name;
    }
    #endregion

    #region 状态控制开关
    private bool canMove = true;
    private bool canAttack = true;
    #endregion

    #region AI相关
    public CharacterController characterController;
    [Header("AI、运动参数")]
    public float runSpeed = 2;
    public float walkSpeed = 2.5f;
    private float moveSpeed;
    private Vector3 moveDirection;
    private void Move()
    {
        moveDirection = player.transform.position - transform.position;
        moveDirection.y = 0;
        if (canMove)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = 0;
        }
        TrunSmooth(moveDirection.normalized);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    #endregion

    void TrunSmooth(Vector3 target)
    {
        transform.forward += (target - transform.forward) * 35 * Time.deltaTime;
    }
}
