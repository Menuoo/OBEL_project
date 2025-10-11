using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] UIcontroller uiControls;
    [SerializeField] PlayerInput input;
    [SerializeField] LayerMask interactLayers;


    BoxCollider interactCollider;

    bool interactTriggered = false;


    private void OnEnable()
    {
        PlayerInput.OnInteractEvent += HandleInteract;
    }

    private void OnDisable()
    {
        PlayerInput.OnInteractEvent -= HandleInteract;
    }

    private void Start()
    {
        interactCollider = GetComponent<BoxCollider>();
    }


    public void DisplayMessage(string[] text)
    { 
        uiControls.StartTextBox(text);
    }





    void HandleInteract(PlayerInput input)
    {
        interactTriggered = true;

        RaycastHit[] hits = Physics.BoxCastAll(transform.position,
        interactCollider.size / 2f, transform.parent.forward, transform.parent.rotation, 0f, interactLayers);


        foreach (RaycastHit hit in hits)
        {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
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

    /*private void OnTriggerStay(Collider other)
    {
        if (interactTriggered)
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
    }*/
}
