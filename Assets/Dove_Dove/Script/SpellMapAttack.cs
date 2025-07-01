using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMapAttack : MonoBehaviour
{
    public GameObject[] allSpell;

    // Start is called before the first frame update

    public void mapAttackStart()
    {
        for(int i = 0; i< allSpell.Length; i++)
        {
            allSpell[i].SetActive(true);

        }
    }
}
