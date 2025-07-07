using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Condition condition = Condition.None;
    private float conditionTime = 0.0f;
    private float conditionDamages = 0.0f;

    public void playerAttackType(Condition getType)
    {
        switch(getType)
        {
            case Condition.None:
                condition = Condition.None;
                break;
            case Condition.poison:
                condition = Condition.poison;
                break;
            case Condition.brun:
                condition = Condition.brun;
                break;
        }
    }

    public void setDamages(float damage )
    {
        conditionDamages += damage;
    }

    public void SetTime(float time)
    {
        conditionTime += time;
    }

    public void RemoveDamages(float damage)
    {
        conditionDamages -= damage;
    }

    public void RemoveTime(float time)
    {
        conditionTime -= time;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterController>().Hit(10.0f);
            other.GetComponent<VariousType>().HitType(condition, conditionTime , conditionDamages);
            print(other.name);
        }
    }


}
