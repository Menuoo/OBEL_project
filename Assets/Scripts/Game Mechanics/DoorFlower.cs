using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorFlower : IInteractable
{
    [SerializeField] DoorObjectFlower door;
    [SerializeField] bool isFirst = false;
    [SerializeField] DoorFlower otherFlower;

    bool isActive = false;
    int lastId = -1;

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("works on " + gameObject.name);
        Knife knif = collision.gameObject.GetComponent<Knife>();
        if (isActive && knif != null)
        {
            knif.GetDamage(out int id);

            if (lastId != id)
            {
                this.gameObject.SetActive(false);
                if (isFirst)
                {
                    otherFlower.Activate(id);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("works on " + gameObject.name);
        Knife knif = other.gameObject.GetComponent<Knife>();
        if (isActive && knif != null)
        {
            knif.GetDamage(out int id);

            if (lastId != id)
            {
                this.gameObject.SetActive(false);
                if (isFirst)
                {
                    otherFlower.Activate(id);
                }
            }
        }
    }

    public void Activate(int id)
    { 
        isActive = true;
        lastId = id;
    }

    public void Wither(Material withr)
    {
        GetComponent<Renderer>().material = withr;
    }

    public override void OnInteract(PlayerInteractions interactions)
    {
        door.OnInteract(interactions);
    }
}
