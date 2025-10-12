using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using Unity.Mathematics;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] PlayerInformation playerInformation;
    [SerializeField] ItemDatabase ItemDatabase;
    [SerializeField] Image[] invImages;

    List<InventoryItem> items = new List<InventoryItem>();
    int lastId;
    int currentIndex;

    void Start()
    {
        ItemDatabase.InitializeList();
        //this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        HandleInventory();
    }

    private void OnDisable()
    {
        items.Clear();
    }


    void HandleInventory()
    {
        int i = 0;
        currentIndex = 0;
        foreach(var item in playerInformation.inventory)
        {
            items.Add(ItemDatabase.GetItem(item.Key));

            if (item.Key == lastId)
            {
                currentIndex = i;
            }

            i++;
            //Debug.Log(item.Key + ", quantity: " + item.Value);
        }

        DisplayInventory();
    }

    void DisplayInventory()
    {
        if (items.Count == 0)
            return;

        int itemIndex = currentIndex - 2;

        if (itemIndex < 0)
        {
            itemIndex = math.abs(itemIndex) % items.Count;
        }

        for (int i = 0; i < 5; i++)
        {
            itemIndex = (itemIndex + i) % items.Count;
            invImages[i].sprite = items[itemIndex].GetSprite();
        }
    }


    void QuitMenu()
    { 
        this.gameObject.SetActive(false);
    }
}
