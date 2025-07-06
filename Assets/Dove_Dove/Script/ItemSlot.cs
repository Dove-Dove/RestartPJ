using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemData;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler

{
    // Start is called before the first frame update
    public GameObject itemDescription;
    public Image slotImg;
    private ItemData itemData;

    private Vector2 ViewUIPos = Vector2.zero;


    RectTransform rect;

    private Transform originalParent;
    private Canvas canvas;

    void Start()
    {
        slotImg = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        itemData = GameManager.Instance.NullItem();

        SetItem(itemData);

        //아이템 설명 창 나오는 위치
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
        if(itemData.ItemName == "null")
            return;
        itemDescription.SetActive(true);
        itemDescription.GetComponent<ItemDescription>().SettingDescription(itemData);
        itemDescription.GetComponent<ItemDescription>().SetPos(ViewUIPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemData.ItemName == "null")
            return;
        itemDescription.GetComponent<ItemDescription>().BackPos();
        itemDescription.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemData.ItemName == "null")
        {
            Debug.Log("현재 비여있음");
            return;
        }

        originalParent = transform.parent;
        transform.SetParent(canvas.transform); 
        slotImg.raycastTarget = false;        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemData.ItemName == "null") 
            return;

        rect.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemData.ItemName == "null")
            return;
        transform.SetParent(originalParent);
        rect.localPosition = Vector3.zero;
        slotImg.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherSlot = eventData.pointerDrag?.GetComponent<ItemSlot>();
        if (otherSlot != null && otherSlot != this && otherSlot.itemData != null)
        {
            SwapItem(otherSlot);
        }
    }

    private void SwapItem(ItemSlot other)
    {
        if (this.itemData.ItemName == "null" && other.itemData.ItemName == "null")
            return;
        ItemData temp = other.itemData;
        other.SetItem(this.itemData);
        this.SetItem(temp);
    }

}
