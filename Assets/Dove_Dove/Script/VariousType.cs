using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousType : MonoBehaviour
{
    public enum Type
    {
        None,
        poison,
        brun,
        frostbite,

    }
    public Type types = Type.None;
    public float typeDuration = 10f;
    public float Damages =10.0f;
    private float typeAttackTime = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        switch(types)
        {
            case Type.poison:
                Posison();
                break;
            case Type.brun:
                Brun();
                break;
        }
    }

    void Posison()
    {
        typeAttackTime += Time.deltaTime;
        float attackTime = 1.0f;
        if (typeAttackTime >= 10.0f)
            types = Type.None;
        else
            StartCoroutine(RunTimeDamages(attackTime));
    }

    void Brun()
    {
        typeAttackTime += Time.deltaTime;
        float attackTime = 2.0f;
        if (typeAttackTime >= 10.0f)
            types = Type.None;
        else
            StartCoroutine(RunTimeDamages(attackTime));
    }

    IEnumerator RunTimeDamages(float waitTiem)
    {
        yield return new WaitForSeconds(waitTiem);
        gameObject.GetComponent<MonsterController>().TypeHit(Damages);
    }
}
