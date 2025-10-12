using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventoryItem : ScriptableObject
{
    [Header("Base Item properties")]
    [SerializeField] int id;
    [SerializeField] string itemName;
    [SerializeField] Sprite inventorySprite;
    [SerializeField] string description;
    [SerializeField] string altDescription;
    [SerializeField] bool canEquip;

    ItemButton[] buttons = { new ItemButton(0, "Use/Equip"), new ItemButton(1, "Inspect") };

    public void ItemAction(int actionID)
    {
        switch (actionID)
        {
            case 0: UseEquipItem(); break;
            case 1: InspectItem(); break;
            default: break;
        }
    }

    public abstract void UseEquipItem();

    public void InspectItem()
    {
        // Change display text from description to inspectDescription
    }

    public int GetId() => id;
    public string GetName() => name;
    public string GetDescription() => description;
    public string GetAltDescription() => altDescription;

}


public struct ItemButton 
{
    public ItemButton(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int id { get; private set; }
    public string name { get; private set; }
}


[CreateAssetMenu(menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] InventoryItem[] items;

    Dictionary<int , InventoryItem> itemsDictionary = new Dictionary<int , InventoryItem>();

    public void InitializeList()
    {
        foreach (InventoryItem item in items)
        {
            itemsDictionary.Add(item.GetId(), item);
        }
    }

    public InventoryItem GetItem(int id)
    { 
        return itemsDictionary[id];
    }
}