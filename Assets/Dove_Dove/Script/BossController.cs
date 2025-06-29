using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossController : MonoBehaviour
{
    enum BossState
    {
        idle,
        move,
        attack,
        casting,
        dead
    }

    //기본 세팅
    private Animator animator;
    private Rigidbody2D rb;
    private BossState state = BossState.idle;

    //체력
    public float maxHP = 100;
    private float nowHp;

    //공격 관련
    public float scanDistance = 100;
    public float attackDistance = 15;
    private int attackCount = 0;


    public float moveSpeed = 2.0f;


    //시간 관련
    private float timeDelay = 0;

    //플레이어 지정
    public GameObject Player;
    private Vector2 distance;

   
   
    public float attackDamages;

    [SerializeField] private GameObject attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private float attackDelay = 0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCalculation();

        switch (state)
        {
            case BossState.idle:
                Idle();
                break;
            case BossState.move:
                Move();
                break;
            case BossState.attack:
                Attack();
                break;
            case BossState.casting:
                Casting();
                break;
            case BossState.dead:
                Dead();
                break;
        }
    }

    //-----------state----------
    private void Idle()
    {
        state = BossState.move;


    }
    private void Move()
    {
        if (state != BossState.move)
            return;

        float attackDis = distance.x;
        if (attackDis < 0)
            attackDis = -attackDis;

        if (attackDis <= attackDistance)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Walk", false);
            animator.SetTrigger("Attack");
            state = BossState.attack;
            return;
        }

        animator.SetBool("Walk", true);
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

    }
    private void Attack()
    {
        if (state != BossState.attack)
            return;

        Vector2 offset = GetComponent<SpriteRenderer>().flipX ? rightOffset : leftOffset;
        attackCollider.GetComponent<SpriteRenderer>().enabled = true;
        attackCollider.transform.localPosition = offset;
       
    }
    private void Casting()
    {
        if (state != BossState.casting)
            return;

        //animator.SetTrigger("Casting");

    }
    private void Dead()
    {
        if (state != BossState.dead)
            return;
    }


    //-------------------------------
    //지정된 플레이어 
    private void PlayerCalculation()
    {
        if(state == BossState.attack || state == BossState.casting)
            return ;
        attackCollider.GetComponent<SpriteRenderer>().enabled = false;
        animator.ResetTrigger("Attack");
        if (attackCount >= 2)
        {
            state = BossState.casting;
            animator.SetTrigger("Casting");
            return;
        }
        distance.x = transform.position.x - Player.transform.position.x ;

        if (distance.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        if((distance.x < scanDistance || -distance.x < scanDistance ) && state != BossState.move)
            state = BossState.move;
        

    }

    //---------애니메이션 특정 모션-------------

    public void AttackBoxOn()
    {
        attackCollider.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void AttackBoxOff()
    {
        attackCollider.GetComponent<BoxCollider2D>().enabled = false;

    }

    public void AttackAimeEnd()
    {
        attackCount++;
        animator.ResetTrigger("Attack");
        state = BossState.idle;
    }
    public void CastingAimeEnd()
    {
        attackCount = 0;
        animator.ResetTrigger("casting");
        state = BossState.idle;
    }

    public void Hit(float Damages)
    {
        nowHp -= Damages;
        if (nowHp > 0)
        {

        }
        else
        {
            animator.SetTrigger("Dead");
            state = BossState.dead;
        }
    }

}
