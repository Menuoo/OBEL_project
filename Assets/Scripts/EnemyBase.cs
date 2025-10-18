using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float health = 20;
    int lastAttack = -1;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " has attacked!");

        Knife knife = other.gameObject.GetComponent<Knife>();
        if (knife != null)
        {
            int attackId;
            int dmgTaken = knife.GetDamage(out attackId);

            if (lastAttack != attackId)
            { 
                lastAttack = attackId;
                health -= dmgTaken;
            }
        }
    }
}
