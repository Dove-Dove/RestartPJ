using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    PlayerSkillData playerSkillData;

    GameObject skills;

    BoxCollider2D boxCollider;


    Vector3 skillPos; 


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //public void SetSkillType(PlayerSkillData setSkill)
    //{
    //    playerSkillData = setSkill;
    //}

    public void SkillAttackStart()
    {
        boxCollider.enabled = true;
    }

    public void SkillAttackEnd()
    {
        boxCollider.enabled = false;
        gameObject.SetActive(false);
    }

}
