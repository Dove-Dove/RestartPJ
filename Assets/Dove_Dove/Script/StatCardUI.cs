using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    Left ,
    mid ,
    Right

}

public class StatCardUI : MonoBehaviour
{
    public TextMeshProUGUI statName;

    public TextMeshProUGUI statText;

    public Image UiImg;

    public Button cardButton;

    public Animator animator;


    public Direction direction;

    public GameObject evnetMoveObj;

    private StatCardData statCard;

    //기타 애니메이션 코드로 추가
    RectTransform rect;

    bool startDown = false;

    float speed = 500.0f;

    Vector2 move ;

    // Start is called before the first frame update
    void Start()
    {
        cardButton.onClick.AddListener(clickEvent);

        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        rect = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startDown)
        {
 

            rect.anchoredPosition = Vector2.MoveTowards(
            rect.anchoredPosition,
            move,
            speed * Time.unscaledDeltaTime
            );

            if(rect.anchoredPosition.y < -900.0f )
            {
                gameObject.SetActive(false);
                startDown = false;
            }
        }
    }

    public void SetingStatUi(StatCardData getState)
    {
        statCard = getState;
        animator.enabled = true;
        statName.text = statCard.StatCardName;
        statText.text = statCard.StatExplanation;
        UiImg.sprite = statCard.StatCardImg;

    }

    void clickEvent()
    {
        //여기에 넣을꺼나 전달할거 추가
        RectTransform rect;
        rect = GetComponent<RectTransform>();
        evnetMoveObj.GetComponent<SCUClickEventObj>().startMove(rect.anchoredPosition);

        GameManager.Instance.AddState(statCard);

        //닫기
        GameObject ui;
        ui = GameObject.Find("UI_Canvas");
        ui.GetComponent<UIManager>().closeStateCard();

        gameObject.SetActive(false);

    }

    public void DirectionMove()
    {
        if(direction == Direction.Left)
            animator.SetTrigger("Left");
        else if(direction == Direction.Right)
            animator.SetTrigger("Right");
    }
    public void NotClick()
    {

        animator.enabled = false;
        move = new Vector2(rect.anchoredPosition.x, -910.0f);
        startDown = true;
    
    }

}
