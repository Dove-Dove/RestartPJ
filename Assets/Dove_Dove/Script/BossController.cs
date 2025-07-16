using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    enum BossState
    {
        idle,
        move,
        attack,
        casting,
        downAttack,
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

    //캐스팅
    public GameObject Spell;
    private int castingPattern = 2;
    private bool spellAttack = false;
    private int randomCast;

    public float teleportDis = 5.0f;
    public float teleportCooldown = 5.0f;
    private float teleportTimer = 0f;
    private bool teleportStart = false;

    public GameObject Explosion;
    


    //시간 관련
    private float timeDelay = 0;
    private bool delayStart = false;
    [SerializeField] 
    private float attackDelay = 1.0f;


    //플레이어 지정
    public GameObject Player;
    private Vector2 distance;

   
   
    public float attackDamages;

    [SerializeField] private GameObject attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;

    [SerializeField] private float mapMinX;
    [SerializeField] private float mapMaxX;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Mathf.Abs(Player.transform.position.x - transform.position.x);
        if (dist < teleportDis && !teleportStart)
        {
            PlayerCloseDistance();
        }
        else if(teleportStart)
        {
            teleportTimer += Time.deltaTime;
            if (teleportTimer >= teleportCooldown)
            {
                teleportStart = false;
                teleportTimer = 0;
            }
        }

        if (delayStart)
        {
            timeDelay += Time.deltaTime;
            if (timeDelay >= attackDelay)
            {
                delayStart = false;
                timeDelay = 0;

            }
        }

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
        //animator.SetBool("Walk", false);


    }
    private void Move()
    {
        if (state != BossState.move)
            return;

        float attackDis = distance.x;
        if (attackDis < 0)
            attackDis = -attackDis;

        if (attackDis <= attackDistance && !delayStart )
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Walk", false);
            animator.SetTrigger("Attack");
            state = BossState.attack;

            return;
        }
        else if(delayStart)
        {
            animator.SetBool("Walk", false);
            return;
        }

        animator.SetBool("Walk", true);
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

    }
    private void Attack()
    {
        if (state != BossState.attack || delayStart)
            return;

        Vector2 offset = GetComponent<SpriteRenderer>().flipX ? rightOffset : leftOffset;
        attackCollider.GetComponent<SpriteRenderer>().enabled = true;
        attackCollider.transform.localPosition = offset;
       
    }
    private void Casting()
    {
        if (state != BossState.casting)
            return;

        randomCast = Random.Range(0, castingPattern);
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
        if(state == BossState.attack || state == BossState.casting ||
            state == BossState.downAttack)
            return ;

 

        distance.x = transform.position.x - Player.transform.position.x;

        attackCollider.GetComponent<SpriteRenderer>().enabled = false;
        animator.ResetTrigger("Attack");
        if (attackCount >= 2)
        {
            state = BossState.casting;
            animator.SetBool("Casting",true);        
            return;
        }


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
        delayStart = true;      
        timeDelay = 0;
    }

    public void SpellStart()
    {
        if (randomCast == 0)
            StartCoroutine(CastingAttack2(1.5f, 0));
        //GameObject.Find("spellMapAttack").GetComponent<SpellMapAttack>().mapAttackStart();
        else
        {
            spellAttack = true;
            StartCoroutine(CastingAttack2(1.5f , 1));
        }

    }

    private void SpellAttack2()
    {
        Vector3 playerPos = Player.transform.position;
        playerPos.y += 1.35f;
        Quaternion spawnRot = Quaternion.identity;
        Instantiate(Spell, playerPos, spawnRot);
    }

    private void PlayerCloseDistance()
    {
        if (teleportStart && state != BossState.attack && state != BossState.casting )
            return;


        Teleport();
        teleportStart = true;
    }

    private void Teleport()
    {
        Vector3 movePos = transform.position;          // Y 고정
        int dir = Random.Range(0, 2) == 0 ? -1 : 1;    // -1 왼 / 1 오

        if (dir == -1 && transform.position.x - 15f > mapMinX)
            movePos.x -= 15f;
        else if (dir == 1 && transform.position.x + 15f < mapMaxX)
            movePos.x += 15f;
        else
            return;

        transform.position = movePos;
    }


    //--코루틴
    IEnumerator CastingAttack2(float delay,int spellType)
    {
        
        float castingDelay = 0.3f;
        float spawnSpell = 0;
        if(spellType == 0)
        {
            yield return new WaitForSeconds(delay);
            GameObject.Find("spellMapAttack").GetComponent<SpellMapAttack>().mapAttackStart();
            spawnSpell += delay;                    
        }
        else
        {
            while (spawnSpell <= delay)
            {
                SpellAttack2();
                yield return new WaitForSeconds(castingDelay);
                spawnSpell += castingDelay;
            }
        }


        attackCount = 0;
        state = BossState.idle;
        animator.SetBool("Casting", false);
        animator.SetTrigger("Idle");
        delayStart = true;
        spellAttack = false;
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
