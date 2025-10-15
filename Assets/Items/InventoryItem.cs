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
    //[SerializeField] bool canEquip;

    public static event Action<string> ChangeDescription;

    ItemButton[] buttons = { new ItemButton(0, "Use/Equip"), new ItemButton(1, "Inspect") };

    public void ItemAction(int actionID, PlayerInformation playerInfo)
    {
        switch (actionID)
        {
            case 0: UseEquipItem(playerInfo); break;
            case 1: InspectItem(); break;
            default: break;
        }
    }

    public abstract void UseEquipItem(PlayerInformation playerInfo);

    public string InspectItem()
    {
        Debug.Log("inspected??");

        ChangeDescription(altDescription);
        return altDescription;
    }

    public int GetId() => id;
    public string GetName() => name;
    public string GetDescription() => description;
    //public string GetAltDescription() => altDescription;
    public Sprite GetSprite() => inventorySprite;

    public ItemButton[] GetButtons() => buttons;
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