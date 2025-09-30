using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float health = 20;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Console.WriteLine(other.name + " has attacked!");

        Knife knife = other.gameObject.GetComponent<Knife>();
        if (knife != null)
        { 
            health -= knife.GetDamage();
        }
    }
}
