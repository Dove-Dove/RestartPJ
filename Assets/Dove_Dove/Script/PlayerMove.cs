using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        idle,
        move,
        jump,
        attack,
        roll,
        skill,
        dead
    }
    [Header("==PlayerSetting")]
    private float maxHp;
    private float maxMp;
    private float moveSpeed;
    private float jumpForce;
    public int money = 0;

    public PlayerData playerData;

    private PlayerState playerState = PlayerState.idle;
    
    private float nowHp;
    private float nowMp;

    private bool jumping = false;
    private GameObject ui;

    [Header("==PlayerAttack")]
    //--공격관련---
    private bool attacking = false;
    private int attackNum = 1;
    private float attackDeley ;
    private float delayTime = 0f;
    private float attackSpeed = 1f;
    private bool delayStart = false;

    [Header("==PlayerSkill")]
    //플레이어 스킬
    public GameObject PlayerSkillBox;
    private PlayerSkillData skill;
    private bool skillOn = false;

    private List<GameObject> ThinderHitList = new List<GameObject>();

    public GameObject TestSkill;

    Vector2 skillLeft = new Vector2(2.14f, 0.31f);
    Vector2 skillRight = new Vector2(-2.14f, 0.31f);
    //----

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 movePos = Vector2.zero;



    //-- 구르기 
    private float rollSpeed = 5f;
    private float rollDuration = 0.5f;

    //구르기 속도
    private float rollAimeSpeed = 1f;
    private bool rolling = false;
    private float rollTimer = 0f;
    private Vector2 rollDirection;
    //-------히트 

    private bool hitCheck = false;
    public float hitAnimeEndTime = 1.0f;
    private bool isInvincible = false;
    //---------

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [SerializeField] private GameObject attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private float attackTime = 0.2f;

    void Start()
    {
        startSetting(); 
    }

    void Update()
    {
        //isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, groundMask);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);
        if (delayStart)
        {
            delayTime += Time.deltaTime;
            if(delayTime >= attackDeley)
            {
                delayStart = false;
            }
        }

        if (isGrounded)
        {
            animator.SetBool("IsGround", true);
            jumping = false;
            rb.drag = 5f;
        }
        else
            rb.drag = 0f;

        KeyController();

        switch (playerState)
        {
            case PlayerState.idle:
                PlayerIdle();
                break;
            case PlayerState.move:
                PlayerMoveing();
                break;
            case PlayerState.attack:
                Attack();
                break;
            case PlayerState.jump:
                Jumping();
                break;
            case PlayerState.roll:
                PlayerRoll();
                break;
            case PlayerState.skill:
                SkillAttack();
                break;
        }


    }

    private void startSetting()
    {
        maxHp = playerData.PlayerMaxHp;       
        maxMp = playerData.PlayerMaxMp;

        moveSpeed = playerData.MoveSpeed;
        jumpForce = playerData.JumpForce;

        nowHp = maxHp;
        nowMp = maxMp;

        attackCollider.GetComponent<PlayerAttack>().SetDamages(playerData.AttackDemage);
        attackDeley = playerData.AttackDeley;
        attackSpeed = playerData.AttackSpeed;


        skill = playerData.Skill;

        rollSpeed = playerData.RollSpeed;
        rollDuration = playerData.RollDuration;

        InstanceSettingStat();

        ui = GameObject.Find("UI_Canvas");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        attackCollider.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void InstanceSettingStat()
    {
        GameManager.Instance.Stats.playerMaxHp = maxHp;
        GameManager.Instance.Stats.playerMaxMp = maxMp;
        GameManager.Instance.Stats.playerMoveSpeed = moveSpeed;
        GameManager.Instance.Stats.attackSpeed = attackSpeed;
        GameManager.Instance.Stats.attackDeley = attackDeley;
        GameManager.Instance.Stats.rollSpeed = rollSpeed;
        GameManager.Instance.Stats.rollDuration = rollDuration;

    }


    private void Attack()
    {
        if (delayStart)
        {
            return;
        }
        Vector2 offset = sr.flipX ? leftOffset : rightOffset;
        attackCollider.GetComponent<BoxCollider2D>().offset = offset;

        if (!attacking)
        {
            //animator.ResetTrigger("Attack"); // 중복 방지
            if(attackNum == 1)
                animator.SetTrigger("Attack1");
            else if(attackNum == 2)
                animator.SetTrigger("Attack2");
            attacking = true;
            animator.speed = attackSpeed;
        }

    }
    //skill------------------
    private void SkillAttack()
    {
        if (delayStart || skillOn)
            return;

  
        if (skill.PlayerSkill == PlayerSkill.Cuting && !skillOn)
        {
            animator.SetTrigger("Skill");
            animator.speed = 0.65f;
            skillOn = true;
        }

    }

    private void spawnSkill()
    {
        if (skill.PlayerSkill == PlayerSkill.Thunder)
        {
            TRay();
            for (int i = 0 ; i < ThinderHitList.Count; i++)
            {
                Vector2 spawnPoint = ThinderHitList[i].transform.position;
                spawnPoint.y += 0.67f;
                GameObject clone = Instantiate(TestSkill, spawnPoint, transform.rotation);
                SpawnSkill spawn = clone.GetComponent<SpawnSkill>();
                if (spawn != null)
                {
                    spawn.StartSkill(skill);
                }
            }
            ThinderHitList.Clear();
            playerState = PlayerState.idle;
            animator.SetBool("Move", false);
        }
        else if (skill.PlayerSkill == PlayerSkill.HolyCross)
        {

            Vector2 playerPos = transform.position;
            playerPos.y += 1.93f;

            GameObject clone = Instantiate(TestSkill, playerPos, transform.rotation);
            SpawnSkill spawn = clone.GetComponent<SpawnSkill>();
            if (spawn != null)
            {
                spawn.StartSkill(skill);
            }
            ui.GetComponent<UIManager>().CallDonwSkill(4.0f);
            playerState = PlayerState.idle;
            animator.SetBool("Move", false);
        }
    }

    private void TRay()
    {
        //if (ThinderHitList.Count >= 5)
        //    return;

        //RaycastHit2D[] hit1 = Physics2D.Raycast(transform.position, new Vector2(-1, 0), 7, 10);
        //RaycastHit2D[] hit2 = Physics2D.Raycast(transform.position, new Vector2(1, 0), 7, 10);

        //Debug.DrawRay(transform.position, Vector3.left * 4, Color.red);
        //Debug.DrawRay(transform.position, Vector3.right * 4, Color.blue);

        //if (hit1.collider != null && hit1.collider.CompareTag("Monster"))
        //{
        //    ThinderHitList.Add(hit1.collider.gameObject);
        //}

        //else if (hit2.collider != null && hit2.collider.CompareTag("Monster"))
        //{
        //    ThinderHitList.Add(hit2.collider.gameObject);
        //}
        int monsterLayer = LayerMask.GetMask("Monster");

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.left, 4f, monsterLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, 4f, monsterLayer);

        Debug.DrawRay(transform.position, Vector2.left * 4f, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * 4f, Color.red);

        if (hit1.collider != null && hit1.collider.CompareTag("Monster"))
        {
            Debug.Log("몬스터 감지 - 왼쪽: " + hit1.collider.name);
            ThinderHitList.Add(hit1.collider.gameObject);
        }

        if (hit2.collider != null && hit2.collider.CompareTag("Monster"))
        {
            Debug.Log("몬스터 감지 - 오른쪽: " + hit2.collider.name);
            ThinderHitList.Add(hit2.collider.gameObject);
        }
    }

    //----------Controller--------------

    private void KeyController()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && !attacking && 
            playerState != PlayerState.jump && !jumping && !rolling)
        {
            playerState = PlayerState.attack;
        }

        else if(Input.GetKeyDown(KeyCode.LeftShift) && !attacking && 
            playerState != PlayerState.jump && !jumping && !rolling)
        {
            playerState = PlayerState.roll;
            PlayerRollStart();
        }

        else if (Input.GetKeyDown(KeyCode.Space) && isGrounded && 
            playerState != PlayerState.jump && !rolling)
        {
            playerState = PlayerState.jump;
            PlayerJump();
        }

        else if(Input.GetKeyDown(KeyCode.Q) && !attacking &&
            playerState != PlayerState.jump && !jumping && !rolling)
        {
            if(skill.PlayerSkill == PlayerSkill.Cuting)
            {
                playerState = PlayerState.skill;
            }
            else if(skill.PlayerSkill != PlayerSkill.None)
            {
                spawnSkill();
            }

        }

    }
    //-----------Idle-------------
    private void PlayerIdle()
    {
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0)
        {
            playerState = PlayerState.move;
        }

        animator.SetBool("Move", false);

    }

    //------------Move-------------
    private void PlayerMoveing()
    {
        if (playerState != PlayerState.move)
            return;
        float move = Input.GetAxisRaw("Horizontal");

        if (move == 0)
        {
            playerState = PlayerState.idle;
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        sr.flipX = move < 0;

        animator.SetBool("Move", true);

        movePos = new Vector2(move * moveSpeed, rb.velocity.y);
        rb.velocity = movePos;
    }

    //------------Jump-------------
    private void PlayerJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
        animator.SetBool("Move", false);
        jumping = true;
    }

    private void Jumping()
    {
        if (isGrounded && jumping)
        {
            jumping = false;
            playerState = PlayerState.idle;
            animator.SetBool("IsGround", true);
            return;
        }

        animator.SetBool("Move", false);
        animator.SetBool("IsGround", false);
    }
    //------------Roll-------------

    private void PlayerRollStart()
    {
        animator.SetTrigger("Roll");
        animator.SetBool("Move", false);

        rollDirection = sr.flipX ? Vector2.left : Vector2.right;
        rollTimer = 0f;
        rolling = true;
    }

    private void PlayerRoll()
    {
        if(rolling)
        {
            rb.velocity = rollDirection * rollSpeed;
            rollTimer += Time.deltaTime * rollAimeSpeed;

            animator.speed = rollDuration / rollAimeSpeed ;

            if (rollTimer >= rollDuration)
            {
                rb.velocity = Vector2.zero;
                rolling = false;
                playerState = PlayerState.idle;
                animator.speed = 1f;
            }
        }

    }
  

    public void Hit(float Damages)
    {
        if(!hitCheck && playerState != PlayerState.roll)
        {
            nowHp -= Damages;
            if (nowHp <= 0)
                return;
            GameManager.Instance.GetHp(nowHp);

            animator.SetTrigger("Hit");
            //playerState = PlayerState.idle;

            StartCoroutine(HitStart());
        }
        
    }

    private bool UsingMp(float useMp)
    {
        if( (nowMp-useMp) > 0)
        {
            nowMp -= useMp;

            return true;
        }
        return false;

    }

    public void Healing(float heal)
    {
        nowHp += heal;
        if (nowHp >= maxHp)
            nowHp = maxHp;
        GameManager.Instance.GetHp(nowHp);
    }

    //--------애니메이션 특정한 부분에서 작동------------
    public void AttackEnd()
    {
        attacking = false;
        attackCollider.GetComponent<BoxCollider2D>().enabled = false;
        playerState = PlayerState.idle;
        delayStart = true;
        attackNum++;
        animator.speed = 1.0f;
        if (attackNum == 3)
            attackNum = 1;
    }
    
    public void AttackBoxOn()
    {
        attackCollider.GetComponent<BoxCollider2D>().enabled = true;
    }


    public void JumpUp()
    {
        jumping = true;
        animator.SetBool("IsGround", false);
        animator.SetBool("Move", false);
    }

    //이쪽 스킬은 플레이어쪽에 붙어있는 스킬임
    public void SkillStart()
    {
        PlayerSkillBox.SetActive(true);
        Vector2 offset = sr.flipX ? skillRight : skillLeft; //일부러 반대로 넣었음 
        PlayerSkillBox.transform.localPosition = offset;
        PlayerSkillBox.GetComponent<SpriteRenderer>().flipX = sr.flipX ? false: true;
    }
    public void SkillEnd()
    {
        playerState = PlayerState.idle;
        animator.speed = 1f;
        animator.SetBool("Move", false);

    }

    public void SkillSetOn()
    {
        skillOn = false;
    }
    //------------코루틴--------------------------

    IEnumerator HitStart( )
    {
        isInvincible = true;
        float blinkInterval = 0.1f;

        float elapsed = 0f;
        while (elapsed < hitAnimeEndTime)
        {
            sr.enabled = false; // 스프라이트 비활성화
            yield return new WaitForSeconds(blinkInterval);
            sr.enabled = true;  // 다시 켬
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2;
        }
        hitCheck = false;
        isInvincible = false;
    }
    //---------------

    public float PlayerGetHp()
    {
        return nowHp;
    }

    public float PlayerGetMp()
    {
        return nowMp;
    }

    public void getMoney(int getMoeny)
    {
        money += getMoeny;
    }

    public int setMoney()
    {
        return money;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, boxSize);
        }
    }

    public void SettingItem(ItemData data)
    {
        attackCollider.GetComponent<PlayerAttack>().playerAttackType(data.ItemCondition);
        foreach (var itemEffect in data.Effects)
        {
            switch(itemEffect.effectType)
            {
                case ItemEffect.AttackDelay:
                    attackDeley += itemEffect.value;
                    break;
                case ItemEffect.AttackSpeed:
                    attackSpeed+= itemEffect.value;
                    break;
                case ItemEffect.AttackDamage:
                    attackCollider.GetComponent<PlayerAttack>().SetDamages(itemEffect.value);
                    break;
                case ItemEffect.ConditionPower:
                    attackCollider.GetComponent<PlayerAttack>().setCDamages(itemEffect.value);
                    break;
                case ItemEffect.ConditionTime:
                    attackCollider.GetComponent<PlayerAttack>().SetCTime(itemEffect.value);
                    break;
                case ItemEffect.RollingSpeed:
                    attackSpeed += itemEffect.value;
                    break;
                case ItemEffect.RollDuration:
                    rollDuration += itemEffect.value;
                    break;
            }
        }
        InstanceSettingStat();
    }

    public void RemoveItem(ItemData data)
    {
        attackCollider.GetComponent<PlayerAttack>().playerAttackType(data.ItemCondition);
        foreach (var itemEffect in data.Effects)
        {
            switch (itemEffect.effectType)
            {
                case ItemEffect.AttackDelay:
                    attackDeley -= itemEffect.value;
                    break;
                case ItemEffect.AttackSpeed:
                    attackSpeed -= itemEffect.value;
                    break;
                case ItemEffect.AttackDamage:
                    attackCollider.GetComponent<PlayerAttack>().SetDamages(-itemEffect.value);
                    break;
                case ItemEffect.ConditionPower:
                    attackCollider.GetComponent<PlayerAttack>().setCDamages(-itemEffect.value);
                    break;
                case ItemEffect.ConditionTime:
                    attackCollider.GetComponent<PlayerAttack>().SetCTime(-itemEffect.value);
                    break;
                case ItemEffect.RollingSpeed:
                    attackSpeed -= itemEffect.value;
                    break;
                case ItemEffect.RollDuration:
                    rollDuration -= itemEffect.value;
                    break;
            }
        }
        InstanceSettingStat();
    }

    public void setPlayerSkill(PlayerSkillData SetSkill)
    {
        skill = SetSkill;
    }

}
