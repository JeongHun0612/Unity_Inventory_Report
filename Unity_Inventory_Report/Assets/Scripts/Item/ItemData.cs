using UnityEngine;

public enum EItemType
{
    Equipment,
    Consumable
}

public enum EItemRarity
{
    UnCommon,
    Common,
    Rare,
    Unique,
    Legendary,
    Epic,

    None,
}

public enum EItemCode
{
    Weapon,

    Helmet,
    Chest,
    Pants,
    Gloves,
    Shoes,
    Cape,

    None,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemID;
    public string ItemName;
    public EItemType ItemType;
    public EItemRarity ItemRarity;
    public EItemCode ItemCode;
    public int RequiredLevel;
    public Sprite ItemIcon;
    public string Description;

    public string Print()
    {
        return
            $"ItemID   : {ItemID}\n" +
            $"Name     : {ItemName}\n" +
            $"Type     : {ItemType}\n" +
            $"Rarity   : {ItemRarity}\n" +
            $"Code     : {ItemCode}\n" +
            $"Level    : {RequiredLevel}\n" +
            $"Desc     : {Description}"
        ;
    }
}
