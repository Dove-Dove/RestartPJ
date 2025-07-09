using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkill
{
    None = 0,
    Cuting = 1 << 1,
    Thunder = 1 << 2,
    HolyCross = 1 << 3,

}

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable/Skill Data", order = int.MaxValue)]

public class PlayerSkillData : ScriptableObject
{
    [SerializeField]
    private PlayerSkill playerSkill;
    public PlayerSkill PlayerSkill => playerSkill;

    [SerializeField]
    private Sprite statCardImg;
    public Sprite StatCardImg => statCardImg;

    [SerializeField]
    private string statName;
    public string StatName => statName;

    [SerializeField]
    private string statText;
    public string StatText => statText;
}