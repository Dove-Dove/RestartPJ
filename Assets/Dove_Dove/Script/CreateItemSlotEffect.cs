using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateItemSlotEffect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI effectName;

    private ItemEffect itemEffect;

    public void settingSlotEffect(ItemEffect setEffect)
    {
        itemEffect = setEffect;
        effectName.text = CreateItemEffectName.GetName(itemEffect);
    }

}
