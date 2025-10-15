using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipItem")]
public class EquipItem : InventoryItem
{
    [Header("Equip Item")]
    [SerializeField] CurrentWeapon weaponType;

    public override void UseEquipItem(PlayerInformation playerInfo)
    {
        playerInfo.Equip(weaponType);
    }
}
