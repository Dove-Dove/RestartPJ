using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ItemData;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler                                                         
{
    public enum EquipmentType
    {
        None = 0,
        Weapon = 1 << 1,
        Armor = 1 << 2,
        Shoes = 1 << 3,
    }
    // Start is called before the first frame update
    public Image slotImg;
    private ItemData itemData;

    public TextMeshProUGUI itemText;

    private Vector2 ViewUIPos = Vector2.zero;

    RectTransform rect;

    public EquipmentType playerType = EquipmentType.None;

    void Start()
    {
        slotImg = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        StartSetting();

        //������ ���� â ������ ��ġ
        ViewUIPos = rect.position;
        ViewUIPos.x += 145;
        ViewUIPos.y = -85;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void StartSetting()
    {
        itemData = GameManager.Instance.NullItem();
        slotImg.sprite = itemData.ItemImg;
    }

    public void SetItem()
    {
        switch(playerType)
        {
            case EquipmentType.Weapon:
                itemData = GameManager.Instance.SetPlayerWapon();
                break;
            case EquipmentType.Armor:
                itemData = GameManager.Instance.SetPlayerArmor();
                break;
            case EquipmentType.Shoes:
                itemData = GameManager.Instance.SetPlayerShoes();
                break;
            default:
                itemData = GameManager.Instance.NullItem();
                break;
        }

         slotImg.sprite = itemData.ItemImg;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemData.ItemName == "null")
            return;

        itemText.text = itemData.ItemName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemData.ItemName == "null")
            return;
        itemText.text = " "; 

    }



}

/* -- ���� (���� : ���� �ð��� �Ѥ�ġ�� �̰� �α� ����ũ ó�� ������� �ߴ´� �׷��⿡�� ���⸦ �ǽð�����
 * �����ؼ� �����Ұ� ����)
 //, IDragHandler, IEndDragHandler, IDropHandler
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemData.ItemName == "null")
        {
            Debug.Log("���� ������");
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
 */
