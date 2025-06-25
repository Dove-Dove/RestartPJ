using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterController>().Hit(10.0f);  // 안전하게 null 체크도!
            print(other.name);
        }
    }

}
