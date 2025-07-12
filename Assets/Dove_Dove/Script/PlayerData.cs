using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable/Player Data", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    //플레이어 기본 세팅
    [SerializeField]
    private float playerMaxHp;
    public float PlayerMaxHp => playerMaxHp;

    [SerializeField]
    private float playerMaxMp;
    public float PlayerMaxMp => playerMaxMp;

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [SerializeField]
    private float jumpForce;
    public float JumpForce => jumpForce;

    [SerializeField]
    private float attackDemage;
    public float AttackDemage => attackDemage;

    [SerializeField]
    private float attackDeley;
    public float AttackDeley => attackDeley;

    [SerializeField]
    private float attackSpeed;
    public float AttackSpeed => attackSpeed;

    [SerializeField]
    private float rollSpeed;
    public float RollSpeed => rollSpeed;

    [SerializeField]
    private float rollDuration;
    public float RollDuration => rollDuration;

    [SerializeField]
    private PlayerSkillData skill;
    public PlayerSkillData Skill => skill;

    [SerializeField]
    private ItemData playerWeapon;
    public ItemData PlayerWeapon => playerWeapon;

    [SerializeField]
    private ItemData playerArmor;
    public ItemData PlayerArmor => playerArmor;

    [SerializeField]
    private ItemData playerShoes;
    public ItemData PlayerShoes => playerShoes;

}
