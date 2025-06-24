using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]

public enum ItemEffect
{
    None = 0,
    AttackTime = 1 << 1,
    projectileCount = 1 << 2,
    projectileCountSize =1 << 3,
    pickupRange = 1 << 4,
    targetRange = 1 << 5,
    projectileSize = 1 << 6,

}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable/Item Data", order = int.MaxValue)]

public class ItemData : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;

    [SerializeField]
    private Sprite itemImg;
    public Sprite ItemImg => itemImg;

    [SerializeField]
    private int itemPrice;
    public int ItemPrice => itemPrice;

    [SerializeField]
    private string itemText;
    public string ItemText => itemText;

    [System.Serializable]
    public class ItemDatas
    {
        public ItemEffect effectType; // 
        public float value;           //
    }

    [SerializeField]
    private List<ItemDatas> effects;
    public List<ItemDatas> Effects => effects;
}
