using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float attackDamages;

    [SerializeField] private BoxCollider2D attackCollider;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private float attackDelay = 0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
