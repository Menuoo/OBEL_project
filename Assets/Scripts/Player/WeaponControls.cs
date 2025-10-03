using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControls : MonoBehaviour
{
    [SerializeField] GameObject knife, pistol;

    CurrentWeapon currentWeapon = CurrentWeapon.Knife;
    PlayerAnimationLogic playerAnim;
    PlayerInput playerInput;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<PlayerAnimationLogic>();
        SwitchKnife();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchKnife();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchPistol();
        }

        if (playerInput.AimPressed)
        {
            HandleAim(true);
        }
        else { HandleAim(false); }

        if (playerInput.WeaponActionPressed)
        {
            HandleWeaponAction();
        }
    }


    void HandleAim(bool state)
    {
        if (currentWeapon == CurrentWeapon.Pistol)
        {
            playerAnim.Aiming(state);
            if (!state)
            {
                playerAnim.ResetAdd();
            }
        }
    }

    void HandleWeaponAction()
    {
        playerAnim.WeaponAction(currentWeapon);
    }


    void SwitchKnife()
    {
        currentWeapon = CurrentWeapon.Knife;

        pistol.SetActive(false);
        knife.SetActive(true);
    }

    void SwitchPistol()
    {
        currentWeapon = CurrentWeapon.Pistol;

        knife.SetActive(false);
        pistol.SetActive(true);
    }
}

public enum CurrentWeapon { Knife, Pistol }
