using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Reload")]
public class ReloadItem : InventoryItem
{
    [Header("Reload Item")]
    [SerializeField] int reloadId = 2002;

    public override void UseEquipItem(PlayerInformation playerInfo)
    {
        playerInfo.Reload(reloadId);

        SoundManager.instance.PlaySound(2);
    }
}