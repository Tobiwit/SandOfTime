using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New BaseItem", menuName = "Inventory System/new Item")]

public class BaseItem : ScriptableObject
{
    public string displayName;
    public GameObject prefab;
    [TextArea(15, 20)]
    public string description;

    public Sprite image = null;

    public int goldCost;

    public int crystalCost;

    public ItemType itemType;

    public bool inHeroInventory = false;

    public UnitType belongingHero;


}

public enum ItemType {
    Consumable,
    Equipment,
    Weapon
}
