using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] int damage = 10;
    int attackId = 0;
    Collider knifeCollider;

    private void Start()
    {
        knifeCollider = GetComponent<Collider>();
        knifeCollider.enabled = false;
    }

    private void OnEnable()
    {
        PlayerAnimationLogic.KnifeAction += SetAttack;
    }

    private void OnDisable()
    {
        PlayerAnimationLogic.KnifeAction -= SetAttack;
    }


    public void SetAttack(bool state)
    {
        attackId = (attackId + 1) % 8;
        knifeCollider.enabled = state;
    }

    public int GetDamage(out int id)
    {
        id = attackId;
        return damage;
    }
}
