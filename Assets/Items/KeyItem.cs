using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/KeyItem")]
public class KeyItem : InventoryItem
{
    //[Header("Equip Item")]

    public override void UseEquipItem(PlayerInformation playerInfo)
    {
        // doesnt do anything i guess???


        if (GetId() == 2999) // only if keyitem is flashlight
        {
            playerInfo.TriggerFlashlight(true);
        }
    }
}
