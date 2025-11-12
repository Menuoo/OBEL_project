using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using Unity.Mathematics;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] PlayerInformation playerInformation;
    [SerializeField] ItemDatabase ItemDatabase;
    [SerializeField] Image[] invImages;
    [SerializeField] Image healthVisual;

    [SerializeField] TMP_Text nameField;
    [SerializeField] TMP_Text quantityField;
    [SerializeField] TMP_Text descriptionField;

    [SerializeField] Button button1;
    [SerializeField] Button button2;

    InventoryItem equippedItem;
    [SerializeField] Image equipImage;
    [SerializeField] TMP_Text gunAmmo;


    List<InventoryItem> items = new List<InventoryItem>();
    int lastId;
    int currentIndex;

    void Awake()
    {
        //ItemDatabase.InitializeList();
        //this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerInputUI.DirectionPressedEvent += HandleDirectionEvent;
        InventoryItem.ChangeDescription += ChangeDesc;
        HandleInventory();
        HealthVisuals();

        Camera.main.GetComponent<PostProcessLayer>().enabled = false;
    }

    private void OnDisable()
    {
        PlayerInputUI.DirectionPressedEvent -= HandleDirectionEvent;
        InventoryItem.ChangeDescription -= ChangeDesc;
        //items.Clear();

        Camera.main.GetComponent<PostProcessLayer>().enabled = true;
    }


    void HandleInventory()
    {
        HealthVisuals();

        int i = 0;
        currentIndex = 0;
        items.Clear();

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
        {
            descriptionField.text = "ERROR:EMPTY  ERROR:EMPTY  ERROR:EMPTY  ERROR:EMPTY  \nERROR:EMPTY  ERROR:EMPTY  ERROR:EMPTY  ERROR:EMPTY  ERROR:EMPTY  ";
            return;
        }

        // index of first item in line
        int itemIndex = currentIndex - 2;
        int finalIndex = 0;

        if (itemIndex < 0)
        {
            itemIndex = math.abs(items.Count + itemIndex) % items.Count;
        }

        // handle main inventory slots
        for (int i = 0; i < 5; i++)
        {
            finalIndex = (itemIndex + i) % items.Count;
            invImages[i].sprite = items[finalIndex].GetSprite();

            if (i == 2)
                currentIndex = finalIndex;
        }


        // handle equip image
        if (playerInformation.equipChange)
        {
            equippedItem = ItemDatabase.GetItem(playerInformation.equipId);
        }
        if (equippedItem != null)
        {
            if (equippedItem.GetId() == 2002)
            {
                int quant = 0;
                gunAmmo.gameObject.SetActive(true);
                playerInformation.inventory.TryGetValue(2002, out quant);
                gunAmmo.text = "x" + quant;
            }
            else
            {
                gunAmmo.gameObject.SetActive(false);
            }

            equipImage.sprite = equippedItem.GetSprite();
        }


        DisplayItem();
    }

    void DisplayItem()
    {
        lastId = items[currentIndex].GetId();

        nameField.text = "Item: " + items[currentIndex].GetName();
        descriptionField.text = items[currentIndex].GetDescription();

        playerInformation.inventory.TryGetValue(items[currentIndex].GetId(), out var quant);
        quantityField.text = (lastId == 2002 ? "Ammo: " : "Quantity: ") + quant.ToString();

        button1.GetComponentInChildren<TMP_Text>().text = items[currentIndex].GetButtons()[0].name;
        button2.GetComponentInChildren<TMP_Text>().text = items[currentIndex].GetButtons()[1].name;
    }
    

    void HandleDirectionEvent(Vector2 dir)
    {
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




    public void UseButton(int buttonNum)
    {
        items[currentIndex].ItemAction(buttonNum, playerInformation);

        // some bullshit because handleinventory resets the desc
        if (buttonNum == 1)
            return;

        HandleInventory();
    }

    public void ChangeDesc(string newDesc)
    { 
        descriptionField.text = newDesc;
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
