using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroud : MonoBehaviour
{
    public Transform cameraTransforms;
    public float backgroundSize = 25.5f;



    // Update is called once per frame
    void Update()
    {
        if(cameraTransforms.position.x > gameObject.transform.position.x + backgroundSize -0.1f)
        {
            Vector3 moveMap = gameObject.transform.position;
            moveMap.x = transform.position.x + (backgroundSize *2);
            gameObject.transform.position = moveMap;

        }
        else if(cameraTransforms.position.x < gameObject.transform.position.x - backgroundSize  )
        {
            Vector3 moveMap = gameObject.transform.position;
            moveMap.x = transform.position.x - (backgroundSize * 2);
            gameObject.transform.position = moveMap;
        }
    }


}
