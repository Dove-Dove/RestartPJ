using UnityEngine;
using UnityEngine.UI;

public class UserStopStateUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] PlayerItemSlot;

    public GameObject SkillPenal;

    public GameObject StatPenal;

    [SerializeField]
    private Slider HP_Slider;
    [SerializeField]
    private Slider MP_Slider;

    public void SetItem()
    {
        for (int i = 0; i < PlayerItemSlot.Length; i++)
        {
            PlayerItemSlot[i].GetComponent<ItemSlot>().SetItem();
        }

        SkillPenal.GetComponent<SkillStopPenal>().StopSkillPenal();
        StatPenal.GetComponent<StatStopPanel>().SettingText();

        HP_Slider.value = GameManager.Instance.Stats.playerNowHp /100;
        MP_Slider.value = GameManager.Instance.Stats.playerNowMp / 100;
    }

}
