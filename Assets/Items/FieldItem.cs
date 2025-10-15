using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour, IInteractable
{
    [SerializeField] int id;
    [SerializeField] int quantity;

    public void OnInteract(PlayerInteractions interactions)
    {
        Debug.Log("touched " + this.gameObject.name);
        interactions.AddItemToInventory(id, quantity);
        Destroy(this.gameObject);
    }
}
