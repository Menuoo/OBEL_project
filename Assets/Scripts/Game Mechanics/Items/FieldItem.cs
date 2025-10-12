using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour, IInteractable
{
    [SerializeField] InventoryItem item;
    [SerializeField] int quantity;

    public void OnInteract(PlayerInteractions interactions)
    {
        interactions.AddItemToInventory(item, quantity);
        Destroy(this.gameObject);
    }
}
