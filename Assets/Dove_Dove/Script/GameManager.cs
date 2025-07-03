using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;



public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    //플레이어
    private GameObject Player;

    private int playerLevel;

    private int PlayerEx;

    private float playerHp;

    //카메라
    private GameObject Cam;

    //UI
    private GameObject ui;

    //현제 레벨 
    public int nowLeval;

    //기타 세팅
    bool GameStopKeyDown = false;

    //데이터 등록

    public StatCardData[] statCardDatas;

    public ItemData[] itemData;


    //스텟 카드 
    [System.Serializable]
    public class UserStateCard
    {
        public StatCardData stateData;
        public int count;
    }
    //유저 아이템 가지고 있는지 확인 
    [System.Serializable]
    public class UserItem
    {
        public ItemData itemData;
        public int count;
    }
    



    public List<UserItem> userGetItemData = new List<UserItem>();

    public List<UserStateCard> userStateData = new List<UserStateCard>();


    //플레이 타임
    //[HideInInspector]
    //public float playTime = 0;


    private void Awake()
    {
        //프레임 고정
        Application.targetFrameRate = 120;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 후에도 GameManager 유지
            
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager가 있으면 삭제
        }

        Player = GameObject.Find("Player");
        Cam = GameObject.Find("Main Camera");
    }

    void Start()
    {
        FindObj();
        playerHp = Player.GetComponent<PlayerMove>().PlayerGetHp();
        GetHp(playerHp);
    }

    // Update is called once per frame
    void Update()
    {
        //FindObj();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStopKeyDown = !GameStopKeyDown;
            if (GameStopKeyDown)
            {
                GameStop();
            }
            else
                GameReStart();
        }
        
        
    }

    public void GameStop()
    {
        ui.GetComponent<UIManager>().escStopGame(true);
    }
    public void GameReStart()
    {

        ui.GetComponent<UIManager>().escStopGame(false);
    }

    public void getLevel(int GetLevel)
    {
        playerLevel += GetLevel;

    }

    public void StartGame()
    {
        nowLeval = 0;
        playerLevel = 0;


    }

    private void FindObj()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Player"); // 씬 로드 시 다시 Player 찾기
        }

        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
        }

        if(ui == null)
        {
            ui = GameObject.Find("UI_Canvas");
        }


        {//다시 확인
            if (Player == null)
            {
                Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
                return;
            }


            if (Cam == null)
            {
                Debug.LogWarning("Camera 오브젝트를 찾을 수 없습니다.");
                return;
            }

            if (Cam == null)
            {
                Debug.LogWarning("UI 오브젝트를 찾을 수 없습니다.");
                return;
            }
        }
    }

    public void GetEx(int ex)
    {
        PlayerEx += ex;
        if (PlayerEx >= 100)
        {
            PlayerEx -= 100;
            playerLevel ++;
            ui.GetComponent<UIManager>().openStateCard();
           
        }
        ui.GetComponent<UIManager>().UIGetEx(PlayerEx);
    }

    public void GetHp(float hp)
    { 
        playerHp = hp;
        ui.GetComponent<UIManager>().setPlayerHit(playerHp);
    }



    public ItemData RanItemData()
    {
        ItemData data;
        int randomNum = Random.Range(0, itemData.Length);
        data = itemData[randomNum];

        return data;

    }

    public StatCardData RanStatCardDate(int getNum)
    {
        StatCardData statCard;
        statCard = statCardDatas[getNum];
        return statCard;

    }





    public void AddItem(ItemData getItem)
    {
        UserItem found = null;

        //아이템이 있는지 확인 
        foreach (UserItem item in userGetItemData)
        {
            if (item.itemData == getItem)
            {
                found = item;
                break;
            }
        }

        //var found = userGetItemData.Find(x => x.itemData == get);

        if (found != null) 
            found.count++;
        else 
            userGetItemData.Add(new UserItem { itemData = getItem, count = 1 });
    }

    public List<UserItem> SetUserItemData()
    {
        return userGetItemData;
    }

    public void AddState(StatCardData getState)
    {
        UserStateCard found = null;

        //아이템이 있는지 확인 
        foreach (UserStateCard state in userStateData)
        {
            if (state.stateData == getState)
            {
                found = state;
                break;
            }
        }

        //var found = userGetItemData.Find(x => x.itemData == get);

        if (found != null)
            found.count++;
        else
            userStateData.Add(new UserStateCard { stateData = getState, count = 1 });
    }

    public List<UserStateCard> SetUserStateData()
    {
        return userStateData;
    }

}
