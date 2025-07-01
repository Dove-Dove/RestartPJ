using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject AttackBox;

    public float Damages = 10;

    void Start()
    {
        AttackBox.SetActive(true);
        AttackBox.GetComponent<SpriteRenderer>().enabled = true;
        AttackBox.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMove>().Hit(Damages); 
            print(other.name);
        }
    }

    public void CastingOn()
    {
        AttackBox.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void AttackBoxOn()
    {
        AttackBox.GetComponent<BoxCollider2D>().enabled = true;
        AttackBox.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void AttackBoxOff()
    {
        AttackBox.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void CastingEnd()
    {
        gameObject.SetActive(false);
    }


}
