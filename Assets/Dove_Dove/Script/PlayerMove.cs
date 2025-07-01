using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        idle,
        move,
        jump,
        attack,
        roll,
        dead
    }

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float maxHp = 100;

    private PlayerState playerState = PlayerState.idle;
    
    private float nowHp;

    private bool jumping = false;

    private bool attacking = false;
    private int attackNum = 1;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 movePos = Vector2.zero;

    [SerializeField] 
    private float attackDeley = 0.4f;

    //-- 구르기 
    [SerializeField] 
    float rollSpeed = 5f;
    [SerializeField] 
    float rollDuration = 0.5f;

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

    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private float attackTime = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
        nowHp = maxHp;
        sr = GetComponent<SpriteRenderer>();

        attackCollider.enabled = false;
    }

    void Update()
    {
        //isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, groundMask);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);

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

        }


    }


    private void Attack()
    {
        Vector2 offset = sr.flipX ? leftOffset : rightOffset;
        attackCollider.offset = offset;

        if (!attacking)
        {
            //animator.ResetTrigger("Attack"); // 중복 방지
            if(attackNum == 1)
                animator.SetTrigger("Attack1");
            else if(attackNum == 2)
                animator.SetTrigger("Attack2");
            attacking = true;
        }
    }


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
            rollTimer += Time.deltaTime;
            if (rollTimer >= rollDuration)
            {
                rb.velocity = Vector2.zero;
                rolling = false;
                playerState = PlayerState.idle;
            }
        }

    }
  


    //--------애니메이션 특정한 부분에서 작동------------

    public void Hit(float Damages)
    {
        if(!hitCheck)
        {
            nowHp -= Damages;
            animator.SetTrigger("Hit");
            //playerState = PlayerState.idle;

            StartCoroutine(HitStart());
        }

    }


    public void AttackEnd()
    {
        attacking = false;
        attackCollider.enabled = false;
        playerState = PlayerState.idle;

        attackNum++;
        if (attackNum == 3)
            attackNum = 1;
    }
    
    public void AttackBoxOn()
    {
        attackCollider.enabled = true;
    }


    public void JumpUp()
    {
        jumping = true;
        animator.SetBool("IsGround", false);
        animator.SetBool("Move", false);
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


    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, boxSize);
        }
    }

}
