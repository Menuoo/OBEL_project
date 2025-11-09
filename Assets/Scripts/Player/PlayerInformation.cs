using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] WeaponControls weaponControls;
    [SerializeField] int maxHealth = 100;

    [SerializeField] Light flashlight;
    bool flashlightOn = false;

    public int health { get; private set; }

    public Dictionary<int, int> inventory { get; private set; }
    public bool equipChange { get; private set; }
    public int equipId { get; private set; }

    void Start()
    {
        inventory = new Dictionary<int, int>();
        // adds gun and knife to inventory
        inventory.Add(2001, 1);
        inventory.Add(2002, 1);

        health = maxHealth;
        equipChange = false;

        flashlight.enabled = flashlightOn;
    }

    private void OnEnable()
    {
        PlayerInput.OnFlashlightEvent += TriggerFlashlight;
    }

    private void OnDisable()
    {
        PlayerInput.OnFlashlightEvent -= TriggerFlashlight;
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //health -= 10;
        }

        if (health <= 0)
            Die();
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
        if (amount < 0)
        {
            TakeDamage();
        }

        Debug.Log("health changed by: " + amount);

        health = Math.Clamp(health + amount, 0, maxHealth);
    }

    void TakeDamage()
    {
        CameraEffects.instance.Shake(10f);
        CameraEffects.instance.Aberrate(4f);
        Instantiate(EffectManager.instance.GetParticles(0), transform.position, Quaternion.identity);
    }



    public void Equip(CurrentWeapon weaponToEquip, int id)
    { 
        weaponControls.SetWeapon(weaponToEquip);
        equipChange = true;
        equipId = id;
    }

    public void ResetEquip()
    { 
        equipChange = false;
    }


    public void TriggerFlashlight(bool state)
    { 
        flashlightOn = !flashlightOn;
        flashlight.enabled = flashlightOn;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
