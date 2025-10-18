using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControls : MonoBehaviour
{
    [SerializeField] Knife knife;
    [SerializeField] GameObject pistol;

    CurrentWeapon currentWeapon = CurrentWeapon.Knife;
    PlayerAnimationLogic playerAnim;
    PlayerInput playerInput;


    private void OnEnable()
    {
        PlayerInput.OnWeaponActionEvent += HandleWeaponAction;
    }

    private void OnDisable()
    {
        PlayerInput.OnWeaponActionEvent -= HandleWeaponAction;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<PlayerAnimationLogic>();
        SetNone();
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

    void HandleWeaponAction(PlayerInput input)
    {
        playerAnim.WeaponAction(currentWeapon);
    }


    public void SetWeapon(CurrentWeapon newWeap)
    {
        switch (newWeap)
        { 
            case CurrentWeapon.Pistol: SwitchPistol(); break;
            case CurrentWeapon.Knife: SwitchKnife(); break;
            default: SetNone(); break;
        }
    }

    void SetNone()
    {
        currentWeapon = CurrentWeapon.None;
        pistol.SetActive(false);
        knife.gameObject.SetActive(false);
    }

    void SwitchKnife()
    {
        currentWeapon = CurrentWeapon.Knife;

        pistol.SetActive(false);
        knife.gameObject.SetActive(true);
    }

    void SwitchPistol()
    {
        currentWeapon = CurrentWeapon.Pistol;

        knife.gameObject.SetActive(false);
        pistol.SetActive(true);
    }
}

public enum CurrentWeapon { None, Knife, Pistol }
