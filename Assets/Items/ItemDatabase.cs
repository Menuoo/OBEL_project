using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] InventoryItem[] items;

    Dictionary<int, InventoryItem> itemsDictionary = new Dictionary<int, InventoryItem>();

    //bool initialized = false;

    public void InitializeList()
    {
        itemsDictionary.Clear();

        foreach (InventoryItem item in items)
        {
            itemsDictionary.Add(item.GetId(), item);
        }
    }

    public InventoryItem GetItem(int id)
    {
        //InitializeList();
        return itemsDictionary[id];
    }
}
