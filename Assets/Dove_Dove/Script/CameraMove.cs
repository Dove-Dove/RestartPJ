using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public Vector2 maxVector;
    public Vector2 minVector;

    float vecX;
    float vecY;

    public GameObject targetPos;

    public Vector2 center;

    void Start()
    {

    }

    void Update()
    {

        //카메라가 밖으로 나오지 않도록 함

        if (maxVector.x < targetPos.transform.position.x)
        {
            transform.position = new Vector3(maxVector.x, targetPos.transform.position.y, -10);

            vecX = maxVector.x;
        }
        else if (minVector.x > targetPos.transform.position.x)
        {
            transform.position = new Vector3(minVector.x, targetPos.transform.position.y, -10);

            vecX = minVector.x;
        }
        else
            vecX = targetPos.transform.position.x;

        if (maxVector.y < targetPos.transform.position.y)
        {
            transform.position = new Vector3(targetPos.transform.position.x, maxVector.y, -10);

            vecY = maxVector.y;
        }
        else if (minVector.y > targetPos.transform.position.y)
        {
            transform.position = new Vector3(targetPos.transform.position.x, minVector.y, -10);

            vecY = minVector.y;
        }
        else
            vecY = targetPos.transform.position.y;

        transform.position = new Vector3(vecX, vecY, -10);



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, maxVector * 2);

    }
}
