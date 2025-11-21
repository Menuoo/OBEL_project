using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : IInteractable
{
    [SerializeField] int id;
    [SerializeField] int quantity;

    SceneItemSave itemSave;

    private void Start()
    {
        itemSave = GetComponent<SceneItemSave>();
        if (itemSave != null)
        {
            if (!itemSave.state.isActive)
                this.gameObject.SetActive(false);
        }
    }

    public override void OnInteract(PlayerInteractions interactions)
    {
        SoundManager.instance.PlaySound(7);

        //Debug.Log("touched " + this.gameObject.name);

        interactions.DisplayItem(id, quantity, this);

        /*interactions.AddItemToInventory(id, quantity);
        Destroy(this.gameObject);*/
    }

    public void Confirm(PlayerInteractions interactions)
    {
        interactions.AddItemToInventory(id, quantity);
        Destroy(this.gameObject);

        if (itemSave != null)
        { 
            itemSave.state.isActive = false;
        }
    }
}
