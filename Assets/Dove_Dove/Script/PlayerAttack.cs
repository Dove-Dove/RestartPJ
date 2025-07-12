using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Condition condition = Condition.None;
    private float attackDemage =0.0f;
    private float conditionTime = 0.0f;
    private float conditionDamages = 0.0f;
    private string types = " ";

    public void playerAttackType(Condition getType)
    {
        switch (getType)
        {
            case Condition.None:
                condition = Condition.None;
                break;
            case Condition.poison:
                condition = Condition.poison;
                types = "poison";
                break;
            case Condition.brun:
                condition = Condition.brun;
                types = "brun";
                break;
        }
    }

    public void SetDamages(float damage)
    {
        attackDemage += damage;
        SetingWarponStat();
    }

    public void setCDamages(float damage)
    {
        conditionDamages += damage;
        SetingWarponStat();
    }

    public void SetCTime(float time)
    {
        conditionTime += time;
        SetingWarponStat();
    }



    private void SetingWarponStat()
    {
        GameManager.Instance.Stats.attackDemage = attackDemage;
        GameManager.Instance.Stats.cPower = conditionDamages;
        GameManager.Instance.Stats.cTime = conditionTime;
        GameManager.Instance.Stats.cType = types;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterController>().Hit(attackDemage);
            other.GetComponent<VariousType>().HitType(condition, conditionTime , conditionDamages);
            print(other.name);
        }
    }


}
