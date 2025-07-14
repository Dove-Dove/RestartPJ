using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateItemImgSlot : MonoBehaviour, IPointerClickHandler
{
    public Image img;
    private Sprite spr;
    

    public void SetingCrateItemImg(Sprite setItemImg)
    {
        spr = setItemImg;
        img.GetComponent<Image>().sprite = spr;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject gameObj = GameObject.Find("CreateItemCanvas");
        gameObj.GetComponent<CreateItemUI>().ItemImgSet(spr);
    }
}
