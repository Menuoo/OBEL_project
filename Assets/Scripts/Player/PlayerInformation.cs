using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] AudioSource audioSrc;
    [SerializeField] PlayerController controller;
    [SerializeField] UIcontroller controllerUI;
    [SerializeField] PlayerAnimationLogic anim;
    [SerializeField] WeaponControls weaponControls;
    [SerializeField] int maxHealth = 100;

    [SerializeField] Light flashlight;
    bool flashlightOn = false;

    public int health { get; private set; }

    public Dictionary<int, int> inventory { get; private set; }

    public bool equipChange { get; private set; }
    //public int equipId { get; private set; }

    public CurrentWeapon currentWeapon = CurrentWeapon.None; // careful with this one


    void Start()
    {
        //   OLD    CODE    MAY NEED AT SOME POINT
        /*
        inventory = new Dictionary<int, int>();
        // adds gun and knife to inventory
        inventory.Add(2001, 1); // knife
        inventory.Add(2002, 10); // gun /
        inventory.Add(2003, 30); // ammo

        health = maxHealth;
        equipChange = false;

        flashlight.enabled = flashlightOn;
        */


        // load player data
        PlayerVariables playerVars = DataVariables.data.PlayerVars;
        health = playerVars.health;

        currentWeapon = playerVars.currWeap;
        weaponControls.SetWeapon(currentWeapon);

        flashlightOn = playerVars.flashlightOn;
        flashlight.enabled = flashlightOn;

        inventory = playerVars.inventory;

        controller.SetPosition(new Vector3(playerVars.rotPos[0], playerVars.rotPos[1], playerVars.rotPos[2]));
        controller.SetRotation(playerVars.rotPos[3]);
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (inventory.TryGetValue(2003, out int val))
            {
                if (val > 0)
                {
                    Reload(2002); // reload gun
                    SoundManager.instance.PlaySound(2);
                }
            }
        }

        // MUST REMOVE AFTER TESTING
        if (Input.GetKeyDown(KeyCode.P))               //     REMOVE        AFTER        TESTING        ALERT
        {
            PlayerSaveLogic();

            StartCoroutine(DataVariables.TakeScreenshot(false, 2));
            //DataVariables.Save(1); // saving usual
        }

        // MUST REMOVE AFTER TESTING
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            StartCoroutine(DataVariables.TakeScreenshot(true, 2)); // saving death
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
        {
            if (id != 2002)
                inventory.Remove(id);
        }
        else
            inventory[id] -= 1;
    }

    public void AlterHP(int amount)
    {
        if (amount < 0)
        {
            TakeDamage();
        }

        //Debug.Log("health changed by: " + amount);

        health = Math.Clamp(health + amount, 0, maxHealth);

        if (health <= 0)
            Die();
    }

    void TakeDamage()
    {
        controllerUI.EndTextBox();

        CameraEffects.instance.Shake(10f);
        CameraEffects.instance.Aberrate(4f);
        Instantiate(EffectManager.instance.GetParticles(0), transform.position, Quaternion.identity);

        anim.Flinch();
        PlaySound(8);
    }

    public void PlaySound(int id)
    {
        audioSrc.PlayOneShot(SoundManager.instance.GetSound(id), audioSrc.volume);
    }


    public bool TryShoot()
    {
        int bullets = inventory[2002];

        if (bullets > 0)
        {
            inventory[2002]--;
            return true;
        }
        return false;
    }

    public void Reload(int id)
    {
        int ammoCount = inventory[id];       // bullet max is 15
        int bulletCount = inventory[id + 1]; // follow this logic for any future weapons lol

        int reloadCount = Mathf.Min((10 - ammoCount), bulletCount);

        inventory[id] += reloadCount;
        inventory[id + 1] -= reloadCount;

        if (inventory[id + 1] < 1)
        {
            inventory.Remove(id + 1);
        }
    }

    public void Equip(CurrentWeapon weaponToEquip)
    { 
        currentWeapon = weaponToEquip;

        weaponControls.SetWeapon(weaponToEquip);
        equipChange = true;
        //equipId = id;
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
    public bool GetFlash() => flashlightOn;


    void Die()
    {
        //Destroy(this.gameObject);
        controller.Die();
        this.enabled = false;
    }


    public void PlayerSaveLogic()
    {
        float[] vec4 = { transform.position.x, transform.position.y, transform.position.z, transform.eulerAngles.y};

        PlayerVariables newPlayerVar = new PlayerVariables { 
            health = this.health, inventory = this.inventory, currWeap = this.currentWeapon,
            flashlightOn = this.flashlightOn, rotPos = vec4 
        };

        DataVariables.data.PlayerVars = newPlayerVar;
    }
}
