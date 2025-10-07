using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputUI : MonoBehaviour, PlayerActions.IUIActions
{

    public void OnConfirm(InputAction.CallbackContext context)
    {
        Debug.Log("chill");
    }
}
