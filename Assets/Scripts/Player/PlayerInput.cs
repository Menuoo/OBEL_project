using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, PlayerActions.IMainActions
{
    [SerializeField] PlayerInputUI ui;
    [SerializeField] LayerMask mouseLayers;
    [SerializeField] AudioSource playerSource;

    public Vector3 mousePosition { get; private set; }

    public Vector2 Walk { get; private set; }
    public Vector2 Look { get; private set; }

    public bool ControlsEnabled { get; private set; }
    public bool GamePaused { get; private set; }


    public bool SprintPressed { get; private set; }
    public bool AimPressed { get; private set; }


    public static event Action<PlayerInput> OnWeaponActionEvent;
    public static event Action<PlayerInput> OnInteractEvent;
    public static event Action<bool> OnFlashlightEvent;
    public static event Action<bool> PauseGameEvent;

    public PlayerActions playerActions { get; private set; }


    private void OnEnable()
    {
        mousePosition = Vector3.zero;

        ControlsEnabled = true;
        GamePaused = false;

        playerActions = new PlayerActions();
        playerActions.Enable();

        playerActions.Main.Enable();
        playerActions.Main.SetCallbacks(this);

        playerActions.UI.Disable();
        playerActions.UI.RemoveCallbacks(ui);
    }

    private void OnDisable()
    {
        playerActions.Main.Disable();
        playerActions.Main.RemoveCallbacks(this);

        playerActions.UI.Disable();
        playerActions.UI.RemoveCallbacks(ui);
    }


    public void Update()
    {
        // Mouse World Position
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool mouseHit = Physics.Raycast(ray, out hit, 1000f, mouseLayers);

        if (mouseHit)
        {
            mousePosition = hit.point;
        }
    }

    public void LateUpdate()
    {
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
        else if (state == -1) // disable all controls
        {
            playerActions.Main.Disable();
            playerActions.Main.RemoveCallbacks(this);

            playerActions.UI.Disable();
            playerActions.UI.RemoveCallbacks(ui);
        }
        else if (state == -2)
        {
            // enable previous conrols
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
            OnWeaponActionEvent?.Invoke(this);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInteractEvent?.Invoke(this);
    }
    public void OnFlashlight(InputAction.CallbackContext context)
    {
        if (context.started)
            OnFlashlightEvent(true);
    }



    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            PauseGame();
    }


    public void PauseGame()
    {
        //PauseHandler.FlipTime();
        PauseGameEvent?.Invoke(!PauseHandler.pauseState);
        //SwapControls(PauseHandler.pauseState ? 0 : 1);
    }


    public void PlaySound(int id)
    {
        playerSource.PlayOneShot(SoundManager.instance.GetSound(id));
    }
}