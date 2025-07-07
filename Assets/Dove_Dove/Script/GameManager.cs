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
    //public List<UserItem> userGetItemData = new List<UserItem>(3);

    public ItemData playerWeapon;
    public ItemData playerArmor;
    public ItemData playerShoes;

    public List<UserStateCard> userStateData = new List<UserStateCard>();



    //카메라
    private GameObject Cam;

    //UI
    private GameObject ui;


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
        ui.GetComponent<UIManager>().UIGetMp(PlayerEx);
    }

    public void GetHp(float hp)
    { 
        playerHp = hp;
        ui.GetComponent<UIManager>().setPlayerHit(playerHp);
    }



    public ItemData RanItemData()
    {
        ItemData data;
        int randomNum = Random.Range(1, itemData.Length);
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
        //UserItem found = null;

        ////아이템이 있는지 확인 
        //foreach (UserItem item in userGetItemData)
        //{
        //    if (item.itemData.ItemType == getItem.ItemType)
        //    {
        //        found = item;
        //        break;
        //    }
        //}

        ////if (found != null) 
        ////    found.count++;
        //if(found == null)
        //{
        //    //userGetItemData.Add(new UserItem { itemData = getItem, count = 1 });
        //    Player.GetComponent<PlayerMove>().SettingItem(getItem);
        //}

        switch(getItem.ItemType)
        {
            case ItemTpyes.Weapon:
                playerWeapon = getItem;
                break;
            case ItemTpyes.Armor:
                playerArmor = getItem;
                break;
            case ItemTpyes.Shoes:
                playerShoes = getItem;
                break;
        }
        Player.GetComponent<PlayerMove>().SettingItem(getItem);

    }

    public void RemoveItem(ItemData getItem)
    {

        Player.GetComponent<PlayerMove>().SettingItem(getItem);
    }

    public ItemData SetPlayerWapon()
    {
        return playerWeapon;
    }
    public ItemData SetPlayerArmor()
    {
        return playerArmor;
    }
    public ItemData SetPlayerShoes()
    {
        return playerShoes;
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

    public ItemData NullItem()
    {
        return itemData[0];
    }


    public List<UserStateCard> SetUserStateData()
    {
        return userStateData;
    }

}
