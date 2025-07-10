using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI ItemName;

    public Image ItemImg;

    public TextMeshProUGUI ItemPrice;

    public TextMeshProUGUI ItemText;


    RectTransform rect;

    Vector2 backPos = Vector2.zero;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        backPos = rect.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingDescription(ItemData itemData , bool SaleItem)
    {
        ItemName.text = itemData.name;
        ItemImg.sprite = itemData.ItemImg;
        if(SaleItem)
        {
            ItemPrice.text = itemData.ItemPrice.ToString();
        }
        else
        {
            ItemPrice.text = itemData.ItemPrice.ToString(" ");
        }
        ItemText.text = itemData.ItemText;

    }
    public void SetPos(Vector2 Pos)
    {
        rect.anchoredPosition = Pos;
    }

    public void BackPos()
    {
        rect.anchoredPosition = backPos;
    }

}
