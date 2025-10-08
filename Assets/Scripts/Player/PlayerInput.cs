using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.VisualScripting;

public class PlayerInput : MonoBehaviour, PlayerActions.IMainActions
{
    [SerializeField] PlayerInputUI ui;

    public Vector2 Walk { get; private set; }
    public Vector2 Look { get; private set; }

    public bool ControlsEnabled { get; private set; }


    public bool JumpPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool WeaponActionPressed { get; private set; }
    public bool AimPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    public PlayerActions playerActions { get; private set; }


    private void OnEnable()
    {
        ControlsEnabled = true;

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


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwapControls(ControlsEnabled ? 0 : 1);
        }
    }

    public void LateUpdate()
    {
        WeaponActionPressed = false;
        InteractPressed = false;
    }


    public void SwapControls(int state)
    {
        if (state == 0)
        {
            playerActions.UI.Enable();
            playerActions.UI.SetCallbacks(ui);

            playerActions.Main.Disable();
            playerActions.Main.RemoveCallbacks(this);

            ControlsEnabled = !ControlsEnabled;
        }
        else if (state == 1)
        {
            playerActions.Main.Enable();
            playerActions.Main.SetCallbacks(this);

            playerActions.UI.Disable();
            playerActions.UI.RemoveCallbacks(ui);

            ControlsEnabled = !ControlsEnabled;
        }
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

    public void OnWeaponAction(InputAction.CallbackContext context)
    {
        if (context.performed)
            WeaponActionPressed = true;

        if (context.canceled)
            WeaponActionPressed = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            InteractPressed = true;
    }
}