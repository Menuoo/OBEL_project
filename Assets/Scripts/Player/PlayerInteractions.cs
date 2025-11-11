using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] PlayerInformation playerInfo;
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

    public void DisplayItem(int id, int quantity, FieldItem item)
    {
        uiControls.StartItemBox(id, quantity, item);
    }

    public void AddItemToInventory(int id, int quantity)
    {
        playerInfo.AddItem(id, quantity);
    }




    void HandleInteract(PlayerInput input)
    {
        interactTriggered = true;

        RaycastHit[] hits = Physics.BoxCastAll(transform.position,
        interactCollider.size / 2f, transform.parent.forward, transform.parent.rotation, 0f, interactLayers);


        IInteractable finalTarget = null;
        float minDist = 100f;
        foreach (RaycastHit hit in hits)
        {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                float distance = Vector3.Magnitude(interactable.transform.position - this.transform.position);
                if (distance < minDist)
                {
                    finalTarget = interactable;
                    minDist = distance;
                }
            }
        }

        if (interactTriggered && finalTarget != null)
        {
            interactTriggered = false;
            finalTarget.OnInteract(this);
        }
    }

    public bool CheckKey(int keyId)
    {
        if (playerInfo.inventory.ContainsKey(keyId))
        {
            playerInfo.inventory.Remove(keyId);
            return true;
        }
        else
        {
            return false;
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
