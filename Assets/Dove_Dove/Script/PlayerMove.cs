using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    enum PlayerState
    {
        idle,
        move,
        attack,
        hit,
        dead
    }

    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float maxHp = 100;
    private float nowHp ;
    
    private Animator animator;

    private bool jumping = false;

    private Rigidbody2D rb;

    private Vector2 movePos = Vector2.zero;

    private bool attack = false;

    [SerializeField] private float attackDeley = 0.4f;

    private bool attackCombo = false;


    private bool attacking = false;

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
        //attackCollider.enabled = false;
    }

    void Update()
    {
        //isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, groundMask);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);

        Attack();

        float move = Input.GetAxisRaw("Horizontal");


        if (jumping && !isGrounded)
        {
            animator.SetBool("IsGround", false);
            animator.SetBool("Move", false);
        }
        else if (jumping && isGrounded)
        {
            jumping = false;
            animator.SetBool("IsGround", true);// ← 착지 시 점프 상태 해제
            Debug.Log("지면에서 떨어짐!");
        }

        if (Input.GetAxisRaw("Horizontal") < 0 && isGrounded && !jumping)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("Move", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && isGrounded && !jumping)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("Move", true);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !isGrounded)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && !isGrounded)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            animator.SetBool("Move", false);
        }
        movePos = new Vector2(move * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !jumping)
        {
            movePos = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("Move", false);
            animator.SetTrigger("Jump");
      
        }


         rb.velocity = movePos;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    isGrounded = true;
        //    jumping = false;
        //    animator.SetBool("IsGround", true);// ← 착지 시 점프 상태 해제
        //    Debug.Log("지면에서 떨어짐!");
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    isGrounded = false;
        //    animator.SetBool("IsGround", false);
        //}
    }
    private void Attack()
    {
        Vector2 offset = GetComponent<SpriteRenderer>().flipX ? leftOffset : rightOffset;
        attackCollider.offset = offset;


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attack = true;

            if (attackCombo)
                animator.SetTrigger("NextAttack");
            else
                animator.SetTrigger("Attack");
        }

    }

    public void Hit(float Damages)
    {
        nowHp -= Damages;
    }

    public void ComboEnable()
    {
        attackCombo = true;
    }

    public void ComboDisable()
    {
        attackCombo = false;
    }

    public void AttackEnd()
    {
       animator.SetTrigger("Idle");
       attack = false;
       attackCollider.enabled = false;

    }
    
    public void AttackBoxOn()
    {
        attack = true;
        attackCollider.enabled = true;
    }

    public void JumpUp()
    {
        jumping = true;
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
