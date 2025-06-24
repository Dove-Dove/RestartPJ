using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UserStopStateUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] ItemImg;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(List<UserItem> settingItem)
    {
        if(settingItem == null)
        {
            Debug.LogWarning("NULL»óÅÂ.");
            return;
        }
        for (int i = 0; i < settingItem.Count; i++)
        {
            ItemImg[i].GetComponent<ItemSlot>().SetItem(settingItem[i].itemData);
        }
    }
}
