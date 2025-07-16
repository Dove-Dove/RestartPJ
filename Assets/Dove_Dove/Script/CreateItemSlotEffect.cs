using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class CreateItemSlotEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI effectName;

    [SerializeField]
    private TMP_InputField inputField;

    private ItemEffect itemEffect;


    public void settingSlotEffect(ItemEffect setEffect)
    {
        itemEffect = setEffect;
        effectName.text = CreateItemEffectName.GetName(itemEffect);
    }

    public ItemEffect settingName()
    {
        return itemEffect;
    }
    public float settingEffect()
    {
        float turnNum;
        float.TryParse(inputField.text, out turnNum);
        return turnNum;
    }
}
