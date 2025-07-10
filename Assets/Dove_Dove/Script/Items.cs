using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static ItemData;

public enum ItemType
{
    Exp,
    hp,
    coin,

}

public class Items : MonoBehaviour
{
    public float detectionRadius = 3f; // 감지 반지름

    public float speed = 1.5f;

    public ItemType item;

    public int ex;

    Animator animator;

    Vector3 movePos;
    bool moveingItem = false;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        // 360도 감지: 지정 반경 내 모든 콜라이더 검색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.tag == "Player")
            {         
                movePos = (hit.transform.position - transform.position).normalized;
                Debug.Log(movePos);
                moveingItem = true;
            }
        }
    
        if(moveingItem)
        {
            transform.Translate(movePos * Time.deltaTime * speed);
        }
    }

    // 디버그용: 감지 범위 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(item == ItemType.Exp)
            {
                GameManager.Instance.GetEx(ex);
            }
            else if(item == ItemType.hp)
            {

            }
            Destroy(this.gameObject);
        }
    }

    public void SpawnItem()
    {
        int rand = Random.Range(0, 10);

        if(rand <= 6)
        {
            item = ItemType.Exp;

        }
        else if (rand <= 6)
        {
            item = ItemType.coin;

        }
    }

    //일반 아이템일 경우 
    public void GetItemData()
    {

        GetComponent<SpriteRenderer>().sprite = GameManager.Instance.RanItemData().ItemImg;

    }
}
