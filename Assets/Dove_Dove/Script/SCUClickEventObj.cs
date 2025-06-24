using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCUClickEventObj : MonoBehaviour
{
    float speed = 450.0f;

    Vector2 target = new Vector2(850, 450);

    RectTransform rect;

    bool eventStart = false;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();

        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(eventStart)
        {
            Debug.Log("이동 중... 현재: " + rect.anchoredPosition + " → 목표: " + target);

            rect.anchoredPosition = Vector2.MoveTowards(
                rect.anchoredPosition,
                target,
                speed * Time.unscaledDeltaTime
                );
            if(rect.anchoredPosition.x >= (target.x -20))
            {
                gameObject.SetActive(false);

            }    
        }
    }

    public void startMove(Vector2 startPos)
    {
        gameObject.SetActive(true);

        if (rect == null)
            rect = GetComponent<RectTransform>();

        rect.anchoredPosition = startPos;
        eventStart = true;
    }


}
