using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using static GameManager;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI timeText;


    [SerializeField]
    private Slider HP_Slider;
    [SerializeField]
    private Slider MP_Slider;

    public GameObject[] statUiAll;

    public GameObject stopPanel;
    public GameObject pauseMenu;

    public GameObject GKeyUi;
    public GameObject itemDescription;

    public GameObject userItemInven;

    //표시용 리스트

    public int DataCount = 0;


    private float gamePlayTime = 0;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gamePlayTime += Time.deltaTime;

        // F1 -> 소수점 한자리까지 표시
        timeText.text = gamePlayTime.ToString("F1");

    }

    public void UIGetMp(float mp)
    {
        MP_Slider.value = mp / 100;
    }

    public void setPlayerHit(float playerHP)
    {
        HP_Slider.value = playerHP / 100;
    }


    public void SettingStateCard()
    {

        List<int> usedIndices = new List<int>();


        for (int count = 0; count < statUiAll.Length; count++)
        {
            int randNum;

            // 중복되지 않는 랜덤 번호를 뽑을 때까지 반복
            do
            {
                randNum = Random.Range(0, DataCount);
            }
            while (usedIndices.Contains(randNum));

            usedIndices.Add(randNum); // 중복 방지용 저장
            //StatCardData stat = Instance.RanStatCardDate(randNum);
            PlayerSkillData skillCard = Instance.RanSkillCardData(randNum);

            //statUiAll[count].GetComponent<StatCardUI>().SetingStatUi(stat);
            //statUiAll[count].SetActive(true);

            statUiAll[count].GetComponent<StatCardUI>().SetingSkillCardUi(skillCard);
            statUiAll[count].SetActive(true);
        }
    }

    public void escStopGame(bool stop)
    {

        if (stop)
        {
            Time.timeScale = 0;
            ActiveStopPanel(true);
            pauseMenu.GetComponent<StopUi>().moveUi(true);
            userItemInven.GetComponent<UserStopStateUI>().SetItem();
        }
        else
        {
            pauseMenu.GetComponent<StopUi>().moveUi(false);

        }
    }

    public void openStateCard()
    {
        //stopPanel.SetActive(true);
        
        SettingStateCard();
        Time.timeScale = 0;

    }

    public void closeStateCard()
    {
        for (int count = 0; count < statUiAll.Length; count++)
        {
            statUiAll[count].GetComponent<StatCardUI>().NotClick();

        }

        Time.timeScale = 1;
    }

    public void GKeyActive(bool active, GameObject gameObj , ItemData itemData)
    {
        GKeyUi.SetActive(active);
        itemDescription.SetActive(active);
        itemDescription.GetComponent<ItemDescription>().SettingDescription(itemData);
        if (Input.GetKeyDown(KeyCode.G) && active)
        {       
            gameObj.SetActive(false);
            GKeyUi.SetActive(false);        
            itemDescription.SetActive(false);
            Instance.AddItem(itemData);

        }

    }

    public void ActiveStopPanel(bool stop)
    {
        stopPanel.SetActive(stop);
        pauseMenu.SetActive(stop);
        if(!stop)
            Time.timeScale = 1;
    }

}
