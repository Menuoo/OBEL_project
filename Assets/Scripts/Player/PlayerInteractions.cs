using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] UIcontroller uiControls;
    [SerializeField] PlayerInput input;

    BoxCollider interactCollider;

    bool interactTriggered = false;


    private void Start()
    {
        interactCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        interactCollider.enabled = input.InteractPressed;
        if (input.InteractPressed)
        { 
            interactTriggered = true;
        }
    }

    public void DisplayMessage(string[] text)
    { 
        uiControls.StartTextBox(text);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (interactTriggered)
            {
                interactTriggered = false;
                interactable.OnInteract(this);
            }
        }
    }
}
