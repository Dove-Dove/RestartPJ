using System.Collections;
using System.Collections.Generic;
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

    public float attackDamages;

    public float moveSpeed;

    public float attackDistance;

    private float distance;

    [SerializeField] 
    private float RayRange = 5f;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private float attackTime = 0.2f;

    Vector3 startVec3;

    Vector3 target;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        startVec3 = gameObject.transform.position;

        nowHp = maxHP;
        target = Vector2.zero;

        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RayOn();

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
        }
    }

    void Idle()
    {
        animator.SetTrigger("Idle");

    }
    void Move()
    {
        if(state == monsterState.attack)
            return;



        animator.SetBool("Walk", true);
        float distance = Vector2.Distance(transform.position, target);

        if(distance >= RayRange)
        {
            state = monsterState.idle;
            animator.SetBool("Walk", false);
            return;
        }

        if (distance <= attackDistance)
        {
            rb.velocity = Vector2.zero;
            state = monsterState.attack;
            //animator.SetBool("Walk", false);
            return;
        }

        Vector2 direction = (target - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);



    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Vector2 offset = GetComponent<SpriteRenderer>().flipX ? leftOffset : rightOffset;
        attackCollider.offset = offset;
        state = monsterState.idle;
    }


    void RayOn()
    {
        // 몬스터가 바라보는 방향 계산 (Sprite 방향에 따라 flipX 판단)
        Vector2 direction = GetComponent<SpriteRenderer>().flipX ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, RayRange, enemyLayer);

        Debug.DrawRay(transform.position, direction * RayRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Player") && !(state == monsterState.attack))
        {
            state = monsterState.move;
            target = hit.transform.position;
        }
    }

    public void AttackBoxOn()
    {
        attackCollider.enabled = true;
    }

    public void AttackBoxOff()
    {
        attackCollider.enabled = false;
    }

    public void HitAimeEnd()
    {
        state = monsterState.idle;
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

    void Dead()
    {

    }

}
