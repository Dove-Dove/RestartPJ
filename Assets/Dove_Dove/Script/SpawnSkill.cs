using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnSkill : MonoBehaviour
{

    Vector2 SpawnPos;
    bool skillOn = false;
    Animator animator;
    PlayerSkillData skillData;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(SkillContinuing());
    }
    void Update()
    {

    }

    public void StartSkill(Vector2 getPos)
    {
        transform.position = getPos;
        skillOn = true;
        StartCoroutine(SkillContinuing());
    }

    public void SkillEnd()
    {
        gameObject.SetActive(false);
    }

    public void SetSkill(PlayerSkillData set)
    {
        skillData = set;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMove>().Healing(0.5f);
        }
        //if (skillData.PlayerSkill == PlayerSkill.HolyCross)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        other.GetComponent<PlayerMove>().Healing(2.0f);
        //    }
        //}
    }

    IEnumerator SkillContinuing()
    {
        yield return new WaitForSeconds(15.0f);
        animator.SetTrigger("HolyEnd");
        SkillEnd();
    }
}
