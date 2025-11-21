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
        if (weaponType == CurrentWeapon.Knife)
            SoundManager.instance.PlaySound(3);
        else SoundManager.instance.PlaySound(0);
            
        playerInfo.Equip(weaponType);
    }
}
