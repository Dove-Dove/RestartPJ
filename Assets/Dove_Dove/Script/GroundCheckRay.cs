using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GroundCheckRay : MonoBehaviour
{
    public GameObject[] rayObj;
    [SerializeField]
    private LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(rayObj[0].transform.position, new Vector3(0,-90,0)
            , 1.0f, enemyLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(rayObj[1].transform.position, new Vector3(0, -90, 0)
    , 1.0f, enemyLayer);
        Debug.DrawRay(rayObj[0].transform.position, new Vector3(0, -90, 0) * 1.0f, Color.red);
        Debug.DrawRay(rayObj[1].transform.position, new Vector3(0, -90, 0) * 1.0f, Color.red);
        if (hit1.collider == null
            || hit2.collider == null)
        {
            gameObject.GetComponent<MonsterController>().Trun(true);
        }

    }


}
