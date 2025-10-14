using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using Unity.Mathematics;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] PlayerInformation playerInformation;
    [SerializeField] ItemDatabase ItemDatabase;
    [SerializeField] Image[] invImages;
    [SerializeField] Image healthVisual;

    [SerializeField] TMP_Text nameField;
    [SerializeField] TMP_Text quantityField;
    [SerializeField] TMP_Text descriptionField;

    List<InventoryItem> items = new List<InventoryItem>();
    int lastId;
    int currentIndex;

    void Awake()
    {
        ItemDatabase.InitializeList();
        //this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerInputUI.DirectionPressedEvent += HandleDirectionEvent;
        HandleInventory();
        HealthVisuals();
    }

    private void OnDisable()
    {
        PlayerInputUI.DirectionPressedEvent -= HandleDirectionEvent;
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

        // index of first item in line
        int itemIndex = currentIndex - 2;
        int finalIndex = 0;

        if (itemIndex < 0)
        {
            itemIndex = math.abs(itemIndex) % items.Count;
        }

        for (int i = 0; i < 5; i++)
        {
            finalIndex = (itemIndex + i) % items.Count;
            invImages[i].sprite = items[finalIndex].GetSprite();

            if (i == 2)
                currentIndex = finalIndex;
        }

        DisplayItem();
    }

    void DisplayItem()
    {
        lastId = items[currentIndex].GetId();

        nameField.text = "Item: " + items[currentIndex].GetName();
        descriptionField.text = items[currentIndex].GetDescription();

        playerInformation.inventory.TryGetValue(items[currentIndex].GetId(), out var quant);
        quantityField.text = "Quantity: " + quant.ToString();
    }
    

    void HandleDirectionEvent(Vector2 dir)
    {
        Debug.Log(dir.x);

        if (dir.x > 0)
        {
            currentIndex++;
            DisplayInventory();
        }
        else if (dir.x < 0)
        {
            currentIndex--;
            DisplayInventory();
        }
    }






    void HealthVisuals()
    {
        //healthVisual.color = Color.Lerp(Color.red, Color.green, playerInformation.health / 100.0f);
        healthVisual.color = Color.HSVToRGB(Mathf.Lerp(0f, 0.33f, playerInformation.health / 100.0f), 1f, 1f);
    }


    void QuitMenu()
    { 
        this.gameObject.SetActive(false);
    }
}
