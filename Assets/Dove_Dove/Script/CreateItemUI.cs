using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static ItemData;

public class CreateItemUI : MonoBehaviour
{
    [Header("==Image==")]
    public Image itemImg;

    [Header("==Button==")]
    public Button itemImgChangeButton;
    public Button itemCreateButton;

    [Header("==InputField==")]
    public TMP_InputField nameInput;
    public TMP_InputField itmePrice;
    public TMP_InputField itmeText;

    [Header("==WarningText==")]
    public GameObject nameWText;
    public GameObject priceWText;
    public GameObject textWText;
    public GameObject imgWText;
    public GameObject typeWText;
    public GameObject CWText;

    [Header("==DropDown==")]
    public TMP_Dropdown itemTypeDropDown;
    public TMP_Dropdown itemConditioDropDown;
    private List<ItemTpyes> itemTypeList= new List<ItemTpyes>();
    private List<Condition> ConditionList = new List<Condition>();
    private int TypeCheck;
    private int ConditionCheck;

    [Header("==ScrollGameObject==")]
    public GameObject EffectSlotP;
    public GameObject ScrollObj;

    [Header("==ScrollImageGameObject==")]
    public GameObject itemImgDataUI;
    public GameObject ItemImgSorillObj;
    public GameObject ItemImgSlot;

    private Sprite settingSprite;

    [Header("==DataList==")]
    public List<GameObject> createEffectObj = new List<GameObject>();
    public Sprite[] itemImgData;
    private List<ItemDatas> itemDs = new List<ItemDatas>();
    
    private bool itemImgDataOpen = false;
    private Sprite StartImg;

    [Header("==OrderEventObj==")]
    public GameObject Stamp;
    public Sprite Nullimg;


    //전달 해야하는 변수들
    private string createItemName;
    private int createItemprice;
    private string createItemText;
    ItemTpyes createItemtype;
    Condition createItemcon;

    // Start is called before the first frame update
    void Start()
    {
        itemImgChangeButton.onClick.AddListener(OpenItemDataObj);
        itemCreateButton.onClick.AddListener(CreateItem);
        StartImg = itemImg.sprite;
        //OpneCreateItem();
    }

    public void OpneCreateItem()
    {
        CreateScrollObj();
        CraeteItemImgScrollObj();
        DropDownData();
        ItemTpyeDropDown();
        ItemConditionDropDown();

        itemImg.sprite = Nullimg;
        nameInput.text = "";
        itmePrice.text = "";
        itmeText.text = "";
        
    }

    private void CreateScrollObj()
    {
        foreach (Transform child in ScrollObj.transform)
        {
            Destroy(child.gameObject);
        }

        int itemECount = System.Enum.GetNames(typeof(ItemEffect)).Length;

        ItemEffect[] values = (ItemEffect[])System.Enum.GetValues(typeof(ItemEffect));
        List<ItemEffect> validEffects = new List<ItemEffect>();

        foreach (var effect in values)
        {
            if (effect != ItemEffect.None) 
                validEffects.Add(effect);
        }

        for (int count = 0; count < validEffects.Count; count++) 
        {
            GameObject IEC = Instantiate(EffectSlotP, ScrollObj.transform);
            ItemEffect selected = validEffects[count];
            IEC.GetComponentInChildren<CreateItemSlotEffect>().settingSlotEffect(selected);
            createEffectObj.Add(IEC); 
        }
    }

    private void CraeteItemImgScrollObj()
    {
        int dataCount = itemImgData.Length;

        for (int count = 0; count < dataCount; count++)
        {
            GameObject IID = Instantiate(ItemImgSlot, ItemImgSorillObj.transform);
            IID.GetComponentInChildren<CreateItemImgSlot>().SetingCrateItemImg(itemImgData[count]);
        }
    }

    private void ItemTpyeDropDown()
    {
        int dropCount = 0;
        foreach (ItemTpyes type in itemTypeList)
        {
            string dropDownStr = $"{type}";
            itemTypeDropDown.options.Add(new TMP_Dropdown.OptionData(dropDownStr));
            dropCount++;
        }

        itemTypeDropDown.value = 0; 
        itemTypeDropDown.RefreshShownValue();
    }

    private void ItemConditionDropDown()
    {
        int dropCount = 0;
        foreach (Condition condition in ConditionList)
        {
            string dropDownStr = $"{condition}";
            itemConditioDropDown.options.Add(new TMP_Dropdown.OptionData(dropDownStr));
            itemConditioDropDown.value = dropCount;
            dropCount++;
        }
        itemConditioDropDown.value = 0;
        itemTypeDropDown.RefreshShownValue();
    }

    private void DropDownData()
    {
        itemTypeList.Clear();

        //Enum.GetValues(typeof(ItemType)) ->enum 타입의 정의된 모든 값을 배열로 반환하는 함수
        foreach (ItemTpyes type in Enum.GetValues(typeof(ItemTpyes)))
        {
            itemTypeList.Add(type);
        }

        ConditionList.Clear();

        foreach (Condition condition in Enum.GetValues(typeof(Condition)))
        {
            ConditionList.Add(condition);
        }

    }

    public void TypeDropDownCheck(int set)
    {
        TypeCheck = set;
    }
    public void ConditionDropDownCheck(int set)
    {
        ConditionCheck = set;
    }

    public void OpenItemDataObj()
    {
        if(!itemImgDataOpen)
            itemImgDataUI.SetActive(true);
        else
            itemImgDataUI.SetActive(false);

        itemImgDataOpen = !itemImgDataOpen;
    }

    public void ItemImgSet(Sprite setSprite)
    {
        settingSprite= setSprite;
        itemImg.sprite = settingSprite;
        OpenItemDataObj();
        
    }

    private void CreateItem()
    {
        bool nullCheck = createSetting();

        if (nullCheck)
            return;


        ItemData tempItem = CreateTempItem();
        GameManager.Instance.GetItemData(tempItem);
        ItemSaveTool.SaveItemAsset(tempItem, createItemName);
        itemImgDataOpen = false;
        startSaveEvent();
        
    }

    private bool createSetting()
    {
        bool nullText = false;
        createItemName = nameInput.text;
        //숫자를 내보냄
        int.TryParse(itmePrice.text, out createItemprice);
        createItemText = itmeText.text;

        createItemtype = itemTypeList[TypeCheck];
        createItemcon = ConditionList[ConditionCheck];

        for (int count = 0; count < createEffectObj.Count; count++)
        {
            float value = createEffectObj[count].GetComponent<CreateItemSlotEffect>().settingEffect();
            if (value == 0)
                continue;
            else
            {
                ItemDatas itemSet = new ItemDatas();

                ItemEffect rootItemitemEffects =
                    createEffectObj[count].GetComponent<CreateItemSlotEffect>().settingName();
                itemSet.effectType = rootItemitemEffects;
                itemSet.value = value;

                itemDs.Add(itemSet);
            }
        }
        {
            if (string.IsNullOrEmpty(createItemName))
            {
                nullText = true;
                nameWText.SetActive(true);
            }
            else
                nameWText.SetActive(false);

            if (string.IsNullOrEmpty(itmePrice.text))
            {
                nullText = true;
                priceWText.SetActive(true);
            }
            else
                priceWText.SetActive(false);

            if (string.IsNullOrEmpty(createItemText))
            {
                nullText = true;
                textWText.SetActive(true);
            }
            else
                textWText.SetActive(false);

            if (itemImg.sprite == Nullimg)
            {
                nullText = true;
                imgWText.SetActive(true);
            }
            else
                imgWText.SetActive(false);
                    
        }
        if(nullText)
        {
            nullText = false;
            return true;
        }
        else
            return false;    
    }

    public ItemData CreateTempItem()
    {
        ItemData temp = ScriptableObject.CreateInstance<ItemData>();

        void SetField(string fieldName, object value)
        {
            typeof(ItemData).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(temp, value);
        }


        SetField("itemName", createItemName);
        SetField("itemImg", settingSprite);
        SetField("itemPrice", createItemprice);
        SetField("itemText", createItemText);
        SetField("itemType", createItemtype);
        SetField("itemCondition", createItemcon);
        SetField("effects", itemDs);

        return temp;
    }
    private void startSaveEvent()
    {
        Stamp.SetActive(true);
    }



    //전꺼 
    //public ItemData CreateTempItem(string name, Sprite img, int price)
    //{
    //    ItemData temp = ScriptableObject.CreateInstance<ItemData>();
    //이 코드는 리플렉션(Reflection) 을 이용해서 ItemData 클래스 내부의 private 변수에 직접 값을 넣는 방식
    //    // 리플렉션이나 JsonUtility로 내부 필드 접근 가능
    //    typeof(ItemData).GetField("itemName", BindingFlags.NonPublic | BindingFlags.Instance)
    //        ?.SetValue(temp, name);

    //    typeof(ItemData).GetField("itemImg", BindingFlags.NonPublic | BindingFlags.Instance)
    //        ?.SetValue(temp, img);

    //    typeof(ItemData).GetField("itemPrice", BindingFlags.NonPublic | BindingFlags.Instance)
    //        ?.SetValue(temp, price);

    //    return temp;
    //}
}

#if UNITY_EDITOR
//using UnityEditor;
#endif

public static class ItemSaveTool
{
#if UNITY_EDITOR
    public static void SaveItemAsset(ItemData itemData, string fileName)
    {
        string dirPath = "Assets/Datas/ItemData";
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string path = $"{dirPath}/{fileName}.asset";
        AssetDatabase.CreateAsset(itemData, path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}
