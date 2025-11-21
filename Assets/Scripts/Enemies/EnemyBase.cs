using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] Transform targetPoint;
    [SerializeField] float health = 20;
    int enemyId;
    int lastAttack = -1;


    public bool attackTaken;
    public bool isDead = false;
    public bool isAggro = false;

    private void Start()
    {
        enemyId = EnemyManager.instance.AddEnemy(this);
    }


    private void Update()
    {
        Die();
    }

    void Die()
    {
        if (health <= 0)
        {
            EnemyManager.instance.RemoveEnemy(enemyId);
            //Destroy(this.gameObject);
            isDead = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Knife knife = other.gameObject.GetComponent<Knife>();
        if (knife != null)
        {
            int attackId;
            int dmgTaken = knife.GetDamage(out attackId);

            TakeDamage(attackId, dmgTaken);
        }

        zPlayerAlert alert = other.gameObject.GetComponent<zPlayerAlert>();
        if (alert != null)
        {
            BecomeAggro();
        }
    }

    public void TakeDamage(int attackId, int dmgTaken)
    {
        if (lastAttack != attackId)
        {
            lastAttack = attackId;
            health -= dmgTaken;

            attackTaken = true;
            BecomeAggro();
        }
    }

    public void BecomeAggro()
    {
        isAggro = true;
    }

    public Vector3 GetTargetPos() => targetPoint.position;
}
