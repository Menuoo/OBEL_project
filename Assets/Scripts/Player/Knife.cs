using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] float damage = 10f;

    public float GetDamage()
    { 
        return damage;
    }
}
