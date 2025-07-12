using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class StatStopPanel : MonoBehaviour
{
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI ATSpeedText;
    public TextMeshProUGUI CPTypeText;
    public TextMeshProUGUI CPowerText;
    public TextMeshProUGUI CTimeText;
    public TextMeshProUGUI RollSpeedText;
    public TextMeshProUGUI RollDurationText;


    public void SettingText()
    {
        DamageText.text = Instance.Stats.attackDemage.ToString();
        ATSpeedText.text = Instance.Stats.attackSpeed.ToString();
        CPTypeText.text = Instance.Stats.cType;
        CPowerText.text = Instance.Stats.cPower.ToString();
        CTimeText.text = Instance.Stats.cTime.ToString();
        RollSpeedText.text = Instance.Stats.rollSpeed.ToString();
        RollDurationText.text = Instance.Stats.rollDuration.ToString();
    }

}
