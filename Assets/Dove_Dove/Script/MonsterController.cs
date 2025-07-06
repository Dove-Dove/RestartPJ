using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : MonoBehaviour
{
    // Start is called before the first frame update
    enum monsterState
    {
        idle,
        move,
        attack,
        hit,
        dead
    }

    private Animator animator;
    private Rigidbody2D rb;

    private monsterState state = monsterState.idle;
    public float maxHP = 100;
    private float nowHp;
    public float moveSpeed;

    private bool moveSearch = true;
    private Vector3 secrchMove;
    private float moveTime = 0.0f;
    public float searchTime = 1.5f;


    private bool turnRay = true;
    private float turnDalay = 0;
    public float turnTime = 1.5f;

    private float theTime =0;
    public float deadTime = 2.0f;

    private float distance;

    public float attackDistance;
    public float attackDamages;
    [SerializeField] private float scanRange = 5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private float attackTime = 0.2f;

    Vector3 target;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        nowHp = maxHP;
        target = Vector2.zero;

        attackCollider.enabled = false;

        secrchMove = transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        RayOn();
        if(!turnRay)
        {
            turnDalay += Time.deltaTime;
            if(turnDalay >= turnTime)
            {
                gameObject.GetComponent<GroundCheckRay>().enabled = true;
                turnDalay = 0;
                turnRay = true;
            }

        }


        switch (state)
        {
            case monsterState.idle:
                Idle();
                break;
            case monsterState.move:
                Move();
                break;
            case monsterState.attack:
                Attack();
                break;
            case monsterState.hit:
                monsterHit();
                break;
            case monsterState.dead:
                Dead();
                break;
        }
    }

    void Idle()
    {
        animator.SetTrigger("Idle");

        if(!moveSearch)
        {
            moveTime += Time.deltaTime;
            if(moveTime >= searchTime)
            {
                moveSearch = true;
                moveTime = 0;
                Trun(false);
            }
           
        }
    }
    void Move()
    {
        if(state == monsterState.attack || state == monsterState.hit || state == monsterState.dead)
            return;

        animator.SetBool("Walk", true);

        float distance = Vector2.Distance(transform.position, target);

        if(distance >= scanRange)
        {

            if (moveSearch)
            {
                moveTime += Time.deltaTime;
                if (moveTime > searchTime)
                {
                    moveSearch = false;
                    moveTime = 0;
                    state = monsterState.idle;
                    animator.SetBool("Walk", false);
                    return;
                }
                bool flip = GetComponent<SpriteRenderer>().flipX ? true : false;
                if(flip)
                    secrchMove.x = gameObject.transform.position.x - 2;
                else
                    secrchMove.x = gameObject.transform.position.x + 2;

                Vector2 directions = (secrchMove - transform.position).normalized;
                rb.velocity = new Vector2(directions.x * moveSpeed, rb.velocity.y);
                return;
            }
            else
            {
                state = monsterState.idle;
                animator.SetBool("Walk", false);
                return;
            }

        }

        else if (distance <= attackDistance)
        {
            rb.velocity = Vector2.zero;
            state = monsterState.attack;
            moveSearch = false;
            //animator.SetBool("Walk", false);
            return;
        }

        Vector2 direction = (target - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Vector2 offset = GetComponent<SpriteRenderer>().flipX ? rightOffset : leftOffset;
        attackCollider.offset = offset;
        state = monsterState.idle;
    }

    void monsterHit()
    {
        theTime += Time.deltaTime;
        if(theTime > 0.4f)
        {
            state = monsterState.idle;
            theTime = 0;
        }
    
    }

    void Dead()
    {
        theTime += Time.deltaTime;
        GetComponent<Rigidbody2D>().simulated = false;
        if (theTime > deadTime)
        {
            Destroy(gameObject);
        }
    }


    void RayOn()
    {
        if (state == monsterState.hit || state == monsterState.dead)
            return;
        // 몬스터가 바라보는 방향 계산 (Sprite 방향에 따라 flipX 판단)
        Vector2 direction = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, scanRange, enemyLayer);

        Debug.DrawRay(transform.position, direction * scanRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player") && !(state == monsterState.attack))
        {
            state = monsterState.move;
            target = hit.transform.position;
        }
        else if (moveSearch)
            state = monsterState.move;

    }

    public void AttackBoxOn()
    {
        attackCollider.enabled = true;
    }

    public void AttackBoxOff()
    {
        attackCollider.enabled = false;
        ReSearch();
    }

    public void HitAimeEnd()
    {
        state = monsterState.idle;
        ReSearch();
    }

    public void Hit(float Damages)
    {
        nowHp -= Damages;

        if(nowHp > 0)
        {
            animator.SetTrigger("Hit");
            state = monsterState.hit;
        }
        else
        {
            animator.SetTrigger("Dead");
            state = monsterState.dead;
        }

    }
    
    private void ReSearch()
    {

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position,new Vector2(-1,0), scanRange, enemyLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(1, 0), scanRange, enemyLayer);
        Debug.DrawRay(transform.position,new Vector2(-1, 0) * scanRange, Color.red);
        Debug.DrawRay(transform.position,new Vector2(1, 0) * scanRange, Color.red);
        if (hit1.collider != null && hit1.collider.CompareTag("Player") && !(state == monsterState.attack))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            state = monsterState.move;
            target = hit1.transform.position;
        }

        if (hit2.collider != null && hit2.collider.CompareTag("Player") && !(state == monsterState.attack))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            state = monsterState.move;
            target = hit2.transform.position;
        }
    }

    public void TypeHit(float Damages)
    {
        nowHp -= Damages;

        if (nowHp < 0)
        {
            animator.SetTrigger("Dead");
            state = monsterState.dead;
        }
    }


    public void Trun(bool dropObj)
    {
        bool turn = GetComponent<SpriteRenderer>().flipX? true : false;
        GetComponent<SpriteRenderer>().flipX = !turn;
        if(dropObj)
            gameObject.GetComponent<GroundCheckRay>().enabled = false;
        turnRay = false ;
    }

}
