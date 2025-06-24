using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public GameObject itemDescription;
    public Image slotImg;
    private ItemData itemData;

    private Vector2 ViewUIPos = Vector2.zero;

    RectTransform rect;

    void Start()
    {
        slotImg = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        ViewUIPos = rect.position;
        ViewUIPos.x += 145;
        ViewUIPos.y = -85;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(ItemData setItemData)
    {
        itemData = setItemData;
        slotImg.sprite = itemData.ItemImg;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemData == null)
            return;
        itemDescription.SetActive(true);
        itemDescription.GetComponent<ItemDescription>().SettingDescription(itemData);
        itemDescription.GetComponent<ItemDescription>().SetPos(ViewUIPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemData == null)
            return;
        itemDescription.GetComponent<ItemDescription>().BackPos();
        itemDescription.SetActive(false);
    }

}
