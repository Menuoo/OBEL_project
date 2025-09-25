using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerInput : MonoBehaviour, PlayerActions.IMainActions
{
    public Vector2 Walk { get; private set; }
    public Vector2 Look { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool WeaponActionPressed { get; private set; }
    public bool ReloadPressed { get; private set; }
    public bool AimPressed { get; private set; }

    public PlayerActions playerActions { get; private set; }



    private void OnEnable()
    {
        playerActions = new PlayerActions();
        playerActions.Enable();

        playerActions.Main.Enable();
        playerActions.Main.SetCallbacks(this);
    }

    private void OnDisable()
    {
        playerActions.Main.Disable();
        playerActions.Main.RemoveCallbacks(this);
    }




    public void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        Walk = context.ReadValue<Vector2>();
    }




    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            SprintPressed = true;
        else if (context.canceled)
            SprintPressed = false;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
            AimPressed = true;
        else if (context.canceled)
            AimPressed = false;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
            ReloadPressed = true;
        else if (context.canceled)
            ReloadPressed = false;
    }

    public void OnWeaponAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            WeaponActionPressed = true;
        }
        else if (!context.performed)
        {
            WeaponActionPressed = false;
        }
    }
}
