using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateItemUI : MonoBehaviour
{
   
    public Image itemImg;
    public Button itemImgChangeButton;

    public TMP_InputField nameInput;
    public TMP_InputField itmePrice;
    public TMP_InputField itmeText;

    public TMP_Dropdown itemTypeDropDown;
    public TMP_Dropdown itemConditioDropDown;

    public GameObject EffectSlotP;
    public GameObject ScrollObj;
    public List<GameObject> createGameObj = new List<GameObject>();

    public GameObject itemImgDataUI;
    public GameObject ItemImgSorillObj;
    public GameObject ItemImgSlot;

    public Sprite[] itemImgData;

    private Sprite settingSprite;
    private ItemEffect itemEffect;
    
    private bool itemImgDataOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateScrollObj();
        CraeteItemImgScrollObj();
        itemImgChangeButton.onClick.AddListener(OpenItemDataObj);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateScrollObj()
    {
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
            createGameObj.Add(IEC); 
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
}
