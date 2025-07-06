using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]

public enum ItemEffect
{
    None = 0,
    AttackDamage = 1 << 1,
    AttackDelay = 1 << 2,
    AttackSpeed = 1 << 3,
    ConditionPower = 1 << 4,
    ConditionTime = 1 << 5,
    RollingSpeed = 1 << 6,
    RollDuration = 1 << 7,

}

public enum ItemTpyes
{
    None = 0,
    Weapon = 1 << 1,
    Armor = 1 << 2,
    Shoes = 1 << 3,
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

    [SerializeField]
    private ItemTpyes itemType;
    public ItemTpyes ItemType => itemType;

    [SerializeField]
    private Condition itemCondition;
    public Condition ItemCondition => itemCondition;

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
