using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStopPenal : MonoBehaviour
{
    private PlayerSkillData skillData;

    public Image skillImg;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillText;


    public void StopSkillPenal()
    {
        skillData = GameManager.Instance.SetSkillData();
        if(skillData != null)
        {
            skillImg.GetComponent<Image>().sprite = skillData.StatCardImg;
            SkillName.text = skillData.name;
            SkillText.text = skillData.StatText;
        }


    }
}
