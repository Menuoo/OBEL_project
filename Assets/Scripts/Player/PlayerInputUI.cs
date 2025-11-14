using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputUI : MonoBehaviour, PlayerActions.IUIActions
{
    public PlayerInput input { get; private set; }
    public bool ConfirmPressed { get; private set; }

    public static event Action<Vector2> DirectionPressedEvent;


    private void LateUpdate()
    {
        ConfirmPressed = false;
    }


    private void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.performed)
            ConfirmPressed = true;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("triggered");
            input.PauseGame();
        }
    }

    public void OnDirections(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DirectionPressedEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public PlayerInput GetInput() => input;
}
