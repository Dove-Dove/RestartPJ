using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class PlayerStats
{
    public float playerMaxHp;
    public float playerNowHp;
    public float playerMaxMp;
    public float playerNowMp;
    public float playerMoveSpeed;
    public float attackDemage;
    public float attackSpeed;
    public float attackDeley;
    public string cType;
    public float cPower;
    public float cTime;
    public float rollSpeed;
    public float rollDuration;
}

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    //플레이어
    private GameObject Player;
    private int playerLevel;
    private int PlayerEx;

    public PlayerStats Stats { get; private set; } = new PlayerStats();

    //----------------
    //private float playerMaxHp;
    //private float attackSpeed;
    //private float attackTime;
    //private float cPower;
    //private float cTime;
    //private float rollSpeed;
    //private float rollDuration;

    //-------------------

    private float playerNowHp;
    private float playerNowMp;
    private int playerMoney;

    private PlayerSkillData playerSkill_GameManger;

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
    public PlayerSkillData[] skillDatas;
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

        playerNowHp = Player.GetComponent<PlayerMove>().PlayerGetHp();
        playerNowMp = Player.GetComponent<PlayerMove>().PlayerGetMp();
        GetHp(playerNowHp);
        GetMp(playerNowMp);
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

    //원래는 스킬로 할려고 했는대 이미 어느정도 한게 있어서 현상 유지
    public void GetEx(int ex)
    {
        PlayerEx += ex;
        if (PlayerEx >= 100)
        {
            PlayerEx -= 100;
            playerLevel ++;
            ui.GetComponent<UIManager>().openStateCard();
           
        }
        
    }

    public void GetHp(float hp)
    {
        playerNowHp = hp;
        Stats.playerNowHp = playerNowHp;

        ui.GetComponent<UIManager>().setPlayerHit(playerNowHp);
    }

    public void GetMp(float mp)
    {
        playerNowMp = mp;
        Stats.playerNowMp = playerNowMp;

        ui.GetComponent<UIManager>().UIGetMp(playerNowHp);
    }

    public ItemData RanItemData()
    {
        ItemData data;
        int randomNum = Random.Range(1, itemData.Length);
        data = itemData[randomNum];

        return data;

    }

    public bool BuyItem(int money)
    {
        playerMoney = Player.GetComponent<PlayerMove>().setMoney();
        if (playerMoney >= money)
        {
            playerMoney -= money;
            return true;
        }
        else
            return false;
    }

    //public StatCardData RanStatCardDate(int getNum)
    //{
    //    StatCardData skillCard;
    //    skillCard = statCardDatas[getNum];
    //    return skillCard;

    //}

    public PlayerSkillData RanSkillCardData(int getNum)
    {
        PlayerSkillData statCard;
        statCard = skillDatas[getNum];
        return statCard;
    }

    public void SetPlayerSkill(PlayerSkillData setData)
    {
        playerSkill_GameManger = setData;

        Player.GetComponent<PlayerMove>().setPlayerSkill(playerSkill_GameManger);
        ui.GetComponent<UIManager>().SetSkillImg(playerSkill_GameManger.StatCardImg);
    }

    public PlayerSkillData SetSkillData()
    {
        return playerSkill_GameManger;
    }



    public void AddItem(ItemData getItem)
    {

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

    //public void AddState(StatCardData getState)
    //{
    //    UserStateCard found = null;

    //    //아이템이 있는지 확인 
    //    foreach (UserStateCard state in userStateData)
    //    {
    //        if (state.stateData == getState)
    //        {
    //            found = state;
    //            break;
    //        }
    //    }

    //    //var found = userGetItemData.Find(x => x.itemData == get);

    //    if (found != null)
    //        found.count++;
    //    else
    //        userStateData.Add(new UserStateCard { stateData = getState, count = 1 });
    //}

    public ItemData NullItem()
    {
        return itemData[0];
    }


    public List<UserStateCard> SetUserStateData()
    {
        return userStateData;
    }

}
