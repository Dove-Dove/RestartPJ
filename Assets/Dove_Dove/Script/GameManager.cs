using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;



public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    // �̱��� �ν��Ͻ�
    public static GameManager Instance { get; private set; }

    //�÷��̾�
    private GameObject Player;
    private int playerLevel;
    private int PlayerEx;
    private float playerMaxHp;
    private float playerNowHp;
    private int playerMoney;

    private PlayerSkillData playerSkill_GameManger;
    //public List<UserItem> userGetItemData = new List<UserItem>(3);

    public ItemData playerWeapon;
    public ItemData playerArmor;
    public ItemData playerShoes;

    public List<UserStateCard> userStateData = new List<UserStateCard>();



    //ī�޶�
    private GameObject Cam;

    //UI
    private GameObject ui;


    //��Ÿ ����
    bool GameStopKeyDown = false;

    //������ ���
    public PlayerSkillData[] skillDatas;
    public StatCardData[] statCardDatas;

    public ItemData[] itemData;


    //���� ī�� 
    [System.Serializable]
    public class UserStateCard
    {
        public StatCardData stateData;
        public int count;
    }
   


    //�÷��� Ÿ��
    //[HideInInspector]
    //public float playTime = 0;


    private void Awake()
    {
        //������ ����
        Application.targetFrameRate = 120;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �Ŀ��� GameManager ����
            
        }
        else
        {
            Destroy(gameObject); // �ߺ��� GameManager�� ������ ����
        }

        Player = GameObject.Find("Player");
        Cam = GameObject.Find("Main Camera");
    }

    void Start()
    {
        FindObj();

        playerMaxHp = Player.GetComponent<PlayerMove>().PlayerGetMaxHp();
        playerNowHp = Player.GetComponent<PlayerMove>().PlayerGetHp();
        GetHp(playerNowHp);
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
            Player = GameObject.Find("Player"); // �� �ε� �� �ٽ� Player ã��
        }

        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
        }

        if(ui == null)
        {
            ui = GameObject.Find("UI_Canvas");
        }


        {//�ٽ� Ȯ��
            if (Player == null)
            {
                Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�.");
                return;
            }


            if (Cam == null)
            {
                Debug.LogWarning("Camera ������Ʈ�� ã�� �� �����ϴ�.");
                return;
            }

            if (Cam == null)
            {
                Debug.LogWarning("UI ������Ʈ�� ã�� �� �����ϴ�.");
                return;
            }
        }
    }

    //������ ��ų�� �ҷ��� �ߴ´� �̹� ������� �Ѱ� �־ ���� ����
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
        playerNowHp = hp;


        ui.GetComponent<UIManager>().setPlayerHit(playerNowHp);
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



    public void AddItem(ItemData getItem)
    {
        //UserItem found = null;

        ////�������� �ִ��� Ȯ�� 
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

    //public void AddState(StatCardData getState)
    //{
    //    UserStateCard found = null;

    //    //�������� �ִ��� Ȯ�� 
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
