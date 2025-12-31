using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] Collider attackCollider;
    [SerializeField] int damage = 20;

    bool isActive = false;
    //int id = 0;
    //int attackId = 0;

    private void Start()
    {
        attackCollider.enabled = false;
    }

    public void Begin()
    {
        attackCollider.enabled = true;
        isActive = true;
    }

    public void End()
    {
        attackCollider.enabled = false;
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInformation player = other.GetComponent<PlayerInformation>();
        if (player != null)
        {
            if (isActive)
            {
                player.AlterHP(-damage);
                isActive = false;
            }
        }
    }

    /*public void SetId(int newId)
    { 
        id = newId;
    }*/
}
