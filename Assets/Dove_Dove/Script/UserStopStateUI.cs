using UnityEngine;
using UnityEngine.UI;

public class UserStopStateUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] PlayerItemSlot;

    public Text SkillName;
    public Text SkillText;

    

    public void SetItem()
    {
        for (int i = 0; i < PlayerItemSlot.Length; i++)
        {
            PlayerItemSlot[i].GetComponent<ItemSlot>().SetItem();
        }


    }

}
