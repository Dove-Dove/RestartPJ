using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DealerNpc : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject spawnEffect;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            SpawnDealer();
    }
    public void SpawnDealer()
    {
        if (spawnEffect != null)
        {
            // 이펙트 생성 후 2초 후 자동 삭제
            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }
}
