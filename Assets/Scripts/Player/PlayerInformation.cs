using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int health { get; private set; }

    public Dictionary<int, int> inventory { get; private set; }
    

    void Start()
    {
        inventory = new Dictionary<int, int>();
        health = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //health -= 10;
        }
    }

    public void AddItem(int id, int quantity)
    { 
        if (inventory.ContainsKey(id))
            inventory[id] += quantity;
        else 
            inventory.Add(id, quantity);
    }

    public void RemoveItem(int id)
    {
        if (inventory[id] <= 1)
            inventory.Remove(id);
        else 
            inventory[id] -= 1;
    }

    public void AlterHP(int amount)
    { 
        health = Math.Clamp(health + amount, 0, maxHealth);
    }
}
