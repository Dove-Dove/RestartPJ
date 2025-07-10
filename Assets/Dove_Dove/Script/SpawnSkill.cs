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

        }
        void Update()
        {

        }


        public void StartSkill(PlayerSkillData setData)
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            skillData = setData;
            skillOn = true;
            animator.GetComponent<Animator>().SetTrigger("HolyStart");
        
            StartCoroutine(SkillContinuing());
        }

        public void SkillEnd()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D other)
        {

            if (skillData.PlayerSkill == PlayerSkill.HolyCross)
            {
                if (other.CompareTag("Player"))
                {
                    other.GetComponent<PlayerMove>().Healing(0.5f);
                }
            }

            else if (skillData.PlayerSkill == PlayerSkill.Thunder)
            {
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<PlayerMove>().Hit(10.0f);
                }
            }
        }

        IEnumerator SkillContinuing()
        {
            yield return new WaitForSeconds(15.0f);
            animator.GetComponent<Animator>().SetTrigger("HolyEnd");
            SkillEnd();
        }

    }
