using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    
    private Animator animator;
    private bool isGrounded;

    private bool jumping = false;

    private Rigidbody2D rb;

    private Vector2 movePos = Vector2.zero;

    private bool attack = false;

    public float attackDeley;

    private bool attackCombo = false;

    private bool attacking = false;
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
    }

    void Update()
    {

        float move = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attack = true;

            if (attackCombo)
                animator.SetTrigger("NextAttack");
            else
                animator.SetTrigger("Attack");
        }


        if (Input.GetAxisRaw("Horizontal") < 0 && isGrounded)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("Move", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && isGrounded)
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
            animator.SetTrigger("Jump");
            jumping = true;
        }


            rb.velocity = movePos;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumping = false;
            animator.SetBool("IsGround", true);// ← 착지 시 점프 상태 해제
            Debug.Log("지면에서 떨어짐!");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGround", false);
        }
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

    }
}
