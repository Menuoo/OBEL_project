using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] PlayerInformation playerInformation;
    [SerializeField] ItemDatabase ItemDatabase;

    List<InventoryItem> items = new List<InventoryItem>();

    void Start()
    {
        ItemDatabase.InitializeList();
        //this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        DisplayInventory();
    }

    private void OnDisable()
    {
        
    }


    void DisplayInventory()
    { 
        foreach(var item in playerInformation.inventory)
        {
            Debug.Log(item.Key + ", quantity: " + item.Value);
        }
    }


    void QuitMenu()
    { 
        this.gameObject.SetActive(false);
    }
}
