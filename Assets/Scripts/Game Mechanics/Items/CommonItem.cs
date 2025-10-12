using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/HealingItem")]
public class HealingItem : InventoryItem
{
    [Header("Healing Item")]
    [SerializeField] int healingAmount = 20;

    public override void UseEquipItem()
    { 
        // call healing method with the value healingAmount
    }
}