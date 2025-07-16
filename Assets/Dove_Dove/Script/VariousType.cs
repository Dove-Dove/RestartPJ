using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousType : MonoBehaviour
{
    private Condition condition = Condition.None;

    private float Damages =0f;
    private float typeAttackTime = 0.1f;

    private float setTime = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        switch(condition)
        {
            case Condition.poison:
                Posison();
                break;
            case Condition.brun:
                Brun();
                break;
        }

    }

    void Posison()
    {
        typeAttackTime += Time.deltaTime;
        float attackTime = 1.0f;
        if (typeAttackTime >= setTime)
            condition = Condition.None;
        else
            StartCoroutine(RunTimeDamages(attackTime));
    }

    void Brun()
    {
        typeAttackTime += Time.deltaTime;
        float attackTime = 2.0f;
        if (typeAttackTime >= 10.0f)
            condition = Condition.None;
        else
            StartCoroutine(RunTimeDamages(attackTime));
    }

    IEnumerator RunTimeDamages(float waitTiem)
    {
        yield return new WaitForSeconds(waitTiem);
        gameObject.GetComponent<MonsterController>().TypeHit(Damages);
    }

    public void HitType(Condition HitType,float time, float damage)
    {
        condition = HitType;
        setTime = time;
        Damages = damage;
    }
}
